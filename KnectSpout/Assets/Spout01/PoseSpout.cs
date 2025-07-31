using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class PoseNetSpout : MonoBehaviour
{
    public enum ModelType
    {
        MobileNet
    }

    public enum EstimationType
    {
        MultiPose,
        SinglePose
    }

    [Tooltip("The ComputeShader that will perform the model-specific preprocessing")]
    public ComputeShader posenetShader;
    [Tooltip("Use GPU for preprocessing")]
    public bool useGPU = true;
    [Tooltip("The dimensions of the image being fed to the model")]
    public Vector2Int imageDims = new Vector2Int(256, 256);
    [Tooltip("The MobileNet model asset file to use when performing inference")]
    public NNModel mobileNetModelAsset;
    [Tooltip("The backend to use when performing inference")]
    public WorkerFactory.Type workerType = WorkerFactory.Type.Auto;
    [Tooltip("The type of pose estimation to be performed")]
    public EstimationType estimationType = EstimationType.SinglePose;
    [Tooltip("The maximum number of poses to estimate")]
    [Range(1, 20)]
    public int maxPoses = 20;
    [Tooltip("The score threshold for multipose estimation")]
    [Range(0, 1.0f)]
    public float scoreThreshold = 0.25f;
    [Tooltip("Non-maximum suppression part distance")]
    public int nmsRadius = 100;
    public float smoothingTime = 0.2f;

    public float score;

    private RenderTexture rTex;
    public RenderTexture Tex;

    private System.Action<float[]> preProcessFunction;
    private Tensor input;

    private struct Engine
    {
        public WorkerFactory.Type workerType;
        public IWorker worker;
        public ModelType modelType;

        public Engine(WorkerFactory.Type workerType, Model model, ModelType modelType)
        {
            this.workerType = workerType;
            worker = WorkerFactory.CreateWorker(workerType, model);
            this.modelType = modelType;
        }
    }

    private Engine engine;
    private string heatmapLayer;
    private string offsetsLayer;
    private string displacementFWDLayer;
    private string displacementBWDLayer;
    private string predictionLayer = "heatmap_predictions";

    private Utils.Keypoint[][] poses;
    private PoseSkeleton[] skeletons;

    // Dynamic array to store key points for each detected pose
    public Vector4[][] posePositionsArray;

    public float[] pers;

    private void InitializeBarracuda()
    {
        Model m_RunTimeModel;

        preProcessFunction = Utils.PreprocessMobileNet;
        m_RunTimeModel = ModelLoader.Load(mobileNetModelAsset);

        displacementFWDLayer = m_RunTimeModel.outputs[2];
        displacementBWDLayer = m_RunTimeModel.outputs[3];
        heatmapLayer = m_RunTimeModel.outputs[0];
        offsetsLayer = m_RunTimeModel.outputs[1];

        ModelBuilder modelBuilder = new ModelBuilder(m_RunTimeModel);
        modelBuilder.Sigmoid(predictionLayer, heatmapLayer);

        workerType = WorkerFactory.ValidateType(workerType);
        engine = new Engine(workerType, modelBuilder.model, ModelType.MobileNet);
    }

    private void InitializeSkeletons()
    {
        skeletons = new PoseSkeleton[maxPoses];
        for (int i = 0; i < maxPoses; i++)
            skeletons[i] = new PoseSkeleton();

        // Initialize the dynamic pose position array
        posePositionsArray = new Vector4[maxPoses][];
        for (int i = 0; i < maxPoses; i++)
            posePositionsArray[i] = new Vector4[17];

        // Initialize pers array to store confidence scores
        pers = new float[maxPoses];
    }

    void Start()
    {
        rTex = new RenderTexture(imageDims.x, imageDims.y, 24, RenderTextureFormat.ARGBHalf);
        InitializeBarracuda();
        InitializeSkeletons();
    }

    private void ProcessImageGPU(RenderTexture image, string functionName)
    {
        int numThreads = 8;
        int kernelHandle = posenetShader.FindKernel(functionName);

        RenderTexture result = RenderTexture.GetTemporary(image.width, image.height, 24, RenderTextureFormat.ARGBHalf);
        result.enableRandomWrite = true;
        result.Create();

        posenetShader.SetTexture(kernelHandle, "Result", result);
        posenetShader.SetTexture(kernelHandle, "InputImage", image);
        posenetShader.Dispatch(kernelHandle, result.width / numThreads, result.height / numThreads, 1);

        Graphics.Blit(result, image);
        RenderTexture.ReleaseTemporary(result);
    }

    private void ProcessImage(RenderTexture image)
    {
        if (useGPU)
        {
            ProcessImageGPU(image, preProcessFunction.Method.Name);
            input = new Tensor(image, channels: 3);
        }
        else
        {
            input = new Tensor(image, channels: 3);
            float[] tensorArray = input.data.Download(input.shape);
            preProcessFunction(tensorArray);
            input = new Tensor(input.shape.batch,
                               input.shape.height,
                               input.shape.width,
                               input.shape.channels,
                               tensorArray);
        }
    }

    private void ProcessOutput(IWorker engine)
    {
        Tensor heatmaps = engine.PeekOutput(predictionLayer);
        Tensor offsets = engine.PeekOutput(offsetsLayer);
        Tensor displacementFWD = engine.PeekOutput(displacementFWDLayer);
        Tensor displacementBWD = engine.PeekOutput(displacementBWDLayer);

        int stride = (imageDims.y - 1) / (heatmaps.shape.height - 1);
        stride -= (stride % 8);

        if (estimationType == EstimationType.SinglePose)
        {
            poses = new Utils.Keypoint[1][];
            poses[0] = Utils.DecodeSinglePose(heatmaps, offsets, stride);
        }
        else
        {
            poses = Utils.DecodeMultiplePoses(
                heatmaps, offsets,
                displacementFWD, displacementBWD,
                stride, maxPoses, scoreThreshold, nmsRadius);
        }

        heatmaps.Dispose();
        offsets.Dispose();
        displacementFWD.Dispose();
        displacementBWD.Dispose();
    }

    void Update()
    {
        Graphics.Blit(Tex, rTex);
        ProcessImage(rTex);
        engine.worker.Execute(input);
        input.Dispose();
        ProcessOutput(engine.worker);

        // Reset pers values
        for (int i = 0; i < pers.Length; i++)
            pers[i] = 0;

        // Store pose keypoints into the array
        for (int i = 0; i < poses.Length; i++)
        {
            skeletons[i].UpdateKeyPointPositions(poses[i], imageDims);

            for (int j = 0; j < 17; j++)
            {
                posePositionsArray[i][j] = new Vector4(
                    skeletons[i].keypoints[j].x,
                    skeletons[i].keypoints[j].y,
                    skeletons[i].keypoints[j].z,
                    1
                );

                // Keep track of the highest z-value (confidence)
                pers[i] = Mathf.Max(pers[i], skeletons[i].keypoints[j].z);
            }
        }
    }

    private void OnDisable()
    {
        engine.worker.Dispose();
    }
}
