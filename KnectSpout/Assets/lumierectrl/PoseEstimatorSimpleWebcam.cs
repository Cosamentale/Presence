using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Unity.Barracuda;
using RenderHeads.Media.AVProLiveCamera;
public class PoseEstimatorSimpleWebcam : MonoBehaviour
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
    public Material material;
    public AVProLiveCamera script2;
    //public PoseEstimator script3;
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
    [Tooltip("The maximum number of posees to estimate")]
    [Range(1, 20)]
    public int maxPoses = 20;
    [Tooltip("The score threshold for multipose estimation")]
    [Range(0, 1.0f)]
    public float scoreThreshold = 0.25f;
    [Tooltip("Non-maximum suppression part distance")]
    public int nmsRadius = 100;
    public float smoothingTime = 0.2f;
    public float score;
    [Tooltip("The minimum confidence level required to display the key point")]
    // The texture used to create input tensor
    private RenderTexture rTex;
    //public Texture Tex;
    // The preprocessing function for the current model type
    private System.Action<float[]> preProcessFunction;
    // Stores the input data for the model
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
    // The interface used to execute the neural network
    private Engine engine;
    // The name for the heatmap layer in the model asset
    private string heatmapLayer;
    // The name for the offsets layer in the model asset
    private string offsetsLayer;
    // The name for the forwards displacement layer in the model asset
    private string displacementFWDLayer;
    // The name for the backwards displacement layer in the model asset
    private string displacementBWDLayer;
    // The name for the Sigmoid layer that returns the heatmap predictions
    private string predictionLayer = "heatmap_predictions";
    // Stores the current estimated 2D keypoint locations in videoTexture
    private Utils.Keypoint[][] poses;
    // Array of pose skeletons
    private PoseSkeleton[] skeletons;
   // public Vector3[] posePositions;
    private PoseSkeleton[] skeletons2;
    //public Vector3[] posePositions2;
    private Vector3 smoothDampVelocity;
    private Vector3 smoothDampVelocity2;
    public int closestSkeletonIndex = 0;
    public int closestSkeletonIndex2 = 0;
    public int closestSkeletonIndex3 = 0;
    public int closestSkeletonIndex2Candidate = -1;
    public Vector2[] pos1 = new Vector2[12];
    public Vector2[] pos2 = new Vector2[12];
    public Vector2[] pos3 = new Vector2[12];
    private Vector2[] pos1s = new Vector2[12];
    private Vector2[] pos2s = new Vector2[12];
    private Vector2[] pos3s = new Vector2[12];
    public float[] score1 = new float[12];
    public float[] score2 = new float[12];
    public float[] score3 = new float[12];
    

    public float pbr;
    public float pcr;
    public float pr;
    public float pp;
    public float ppb;
    public float ppc;
    public float pvt1;
    public float pvt2;
    public float pvt3;
    public Vector3 previousHipPosition;
    public Vector3 previousHipPosition2;
    private Vector2 a = new Vector2(0.5f, 0.5f);
    private Vector4[] posePositionsArray = new Vector4[17];
    private Vector4[] posePositionsArray2 = new Vector4[17];
    private Vector4[] posePositionsArray3 = new Vector4[17];

    public float person1;
    public float person2;
    public float person3;
    public float nbperson;
    public float ptest;
    public float pa;
    public float pb;
    public float pc;
    public float mpa;
    public float mpb;
    public float mpc;
    public int framesConditionTrueA = 0; // Counter to keep track of frames the condition is true for mpa
    public int framesConditionTrueB = 0; // Counter to keep track of frames the condition is true for mpb
    public int framesConditionTrueC = 0; // Counter to keep track of frames the condition is true for mpc
    private bool isImageModified;

    public bool Smooth = false;
    //public Vector3[] pos;
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="mirrorScreen"></param>

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
        for (int i = 0; i < maxPoses; i++) skeletons[i] = new PoseSkeleton();
    }
    void Start()
    {     
        rTex = new RenderTexture(imageDims.x, imageDims.y, 24, RenderTextureFormat.ARGBHalf);
        InitializeBarracuda();
        InitializeSkeletons();
        material.SetFloat("_resx", imageDims.x);
        material.SetFloat("_resy", imageDims.y);
        for (int j = 0; j < 17; j++)
        {
            posePositionsArray3[j] = new Vector4(0, 0, 0, 0);
            posePositionsArray2[j] = new Vector4(0, 0, 0, 0);
            posePositionsArray[j] = new Vector4(0, 0, 0, 0);

        }
    }
    /// <param name="image"></param>
    /// <param name="functionName"></param>
    /// <returns></returns>
    private void ProcessImageGPU(RenderTexture image, string functionName)
    {
        int numthreads = 8;
        int kernelHandle = posenetShader.FindKernel(functionName);
        // Define a temporary HDR RenderTexture
        RenderTexture result = RenderTexture.GetTemporary(image.width, image.height, 24, RenderTextureFormat.ARGBHalf);
        result.enableRandomWrite = true;
        result.Create();
        posenetShader.SetTexture(kernelHandle, "Result", result);
        posenetShader.SetTexture(kernelHandle, "InputImage", image);
        posenetShader.Dispatch(kernelHandle, result.width / numthreads, result.height / numthreads, 1);
        Graphics.Blit(result, image);
        RenderTexture.ReleaseTemporary(result);
    }

    /// <param name="image"></param>
    private void ProcessImage(RenderTexture image)
    {
        if (useGPU)
        {
            // Apply preprocessing steps
            ProcessImageGPU(image, preProcessFunction.Method.Name);
            // Create a Tensor of shape [1, image.height, image.width, 3]
            input = new Tensor(image, channels: 3);
        }
        else
        {
            // Create a Tensor of shape [1, image.height, image.width, 3]
            input = new Tensor(image, channels: 3);
            // Download the tensor data to an array
            float[] tensor_array = input.data.Download(input.shape);
            // Apply preprocessing steps
            preProcessFunction(tensor_array);
            // Update input tensor with new color data
            input = new Tensor(input.shape.batch,
                               input.shape.height,
                               input.shape.width,
                               input.shape.channels,
                               tensor_array);
        }
    }

    /// <param name="engine"></param>
    private void ProcessOutput(IWorker engine)
    {
        // Get the model output
        Tensor heatmaps = engine.PeekOutput(predictionLayer);
        Tensor offsets = engine.PeekOutput(offsetsLayer);
        Tensor displacementFWD = engine.PeekOutput(displacementFWDLayer);
        Tensor displacementBWD = engine.PeekOutput(displacementBWDLayer);
        // Calculate the stride used to scale down the inputImage
        int stride = (imageDims.y - 1) / (heatmaps.shape.height - 1);
        stride -= (stride % 8);
        if (estimationType == EstimationType.SinglePose)
        {
            // Initialize the array of Keypoint arrays
            poses = new Utils.Keypoint[1][];
            // Determine the key point locations
            poses[0] = Utils.DecodeSinglePose(heatmaps, offsets, stride);
        }
        else
        {
            // Determine the key point locations
            poses = Utils.DecodeMultiplePoses(
                heatmaps, offsets,
                displacementFWD, displacementBWD,
                stride: stride, maxPoseDetections: maxPoses,
                scoreThreshold: scoreThreshold,
                nmsRadius: nmsRadius);
        }
        // Release the resources allocated for the output Tensors
        heatmaps.Dispose();
        offsets.Dispose();
        displacementFWD.Dispose();
        displacementBWD.Dispose();
    }
    void Update()
    {
        material.SetTexture("_MainTex", script2.OutputTexture);
        float[] posePositionsscore = new float[17];
        float[] posePositionsscore2 = new float[17];
        float[] posePositionsscore3 = new float[17];
        Graphics.Blit(script2.OutputTexture, rTex);
        ProcessImage(rTex);
        engine.worker.Execute(input);
        input.Dispose();
        ProcessOutput(engine.worker);
        float closestDistance = 2000.0f;
        float closestDistance2 = 3000.0f;

        for (int i = 0; i < poses.Length; i++)
        {
            skeletons[i].UpdateKeyPointPositions(poses[i], imageDims);
            //Vector3[] keyPoints = skeletons[i].keypoints;
            float psr = 5 * Vector2.Distance(new Vector2((skeletons[i].keypoints[11].x + skeletons[i].keypoints[12].x) * 0.5f, (skeletons[i].keypoints[11].y + skeletons[i].keypoints[12].y) * 0.5f)
                , new Vector2((posePositionsArray[5].x + posePositionsArray[6].x), (posePositionsArray[5].y + posePositionsArray[6].y)) * 0.5f);

            float distanceToHip = Vector3.Distance(new Vector3(skeletons[i].keypoints[0].x,
                skeletons[i].keypoints[0].y, psr), previousHipPosition);

            float distanceToHip2 = Vector3.Distance(new Vector3(skeletons[i].keypoints[0].x,
               skeletons[i].keypoints[0].y, psr), previousHipPosition2);

            if (distanceToHip < closestDistance)
            {
                closestDistance = distanceToHip;
                closestSkeletonIndex = i;
            }
            if (distanceToHip2 < closestDistance2)
            {
                closestDistance2 = distanceToHip2;
                closestSkeletonIndex2Candidate = i;
            }
        }
        if (closestSkeletonIndex2Candidate != closestSkeletonIndex)
        {
            closestSkeletonIndex2 = closestSkeletonIndex2Candidate;
        }
        if (closestSkeletonIndex == 0)
        {
            Vector3[] keyPoints0 = skeletons[0].keypoints;
            if (closestSkeletonIndex2 == 1)
            {
                Vector3[] keyPoints1 = skeletons[1].keypoints;
                Vector3[] keyPoints2 = skeletons[2].keypoints;
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray2[index] = new Vector4(keyPoints1[j].x, keyPoints1[j].y, 0, 1);
                    posePositionsscore2[index] = keyPoints1[j].z;
                }
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray3[index] = new Vector4(keyPoints2[j].x, keyPoints2[j].y, 0, 1);
                    posePositionsscore3[index] = keyPoints2[j].z;
                }
            }
            if (closestSkeletonIndex2 == 2)
            {
                Vector3[] keyPoints1 = skeletons[2].keypoints;
                Vector3[] keyPoints2 = skeletons[1].keypoints;
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray2[index] = new Vector4(keyPoints1[j].x, keyPoints1[j].y, 0, 1);
                    posePositionsscore2[index] = keyPoints1[j].z;
                }
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray3[index] = new Vector4(keyPoints2[j].x, keyPoints2[j].y, 0, 1);
                    posePositionsscore3[index] = keyPoints2[j].z;
                }
            }
            for (int j = 0; j < 17; j++)
            {
                int index = j;
                posePositionsArray[index] = new Vector4(keyPoints0[j].x, keyPoints0[j].y, 0, 1);
                posePositionsscore[index] = keyPoints0[j].z;
            }
        }
        if (closestSkeletonIndex == 1)
        {
            Vector3[] keyPoints0 = skeletons[1].keypoints;
            if (closestSkeletonIndex2 == 0)
            {
                Vector3[] keyPoints1 = skeletons[0].keypoints;
                Vector3[] keyPoints2 = skeletons[2].keypoints;
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray2[index] = new Vector4(keyPoints1[j].x, keyPoints1[j].y, 0, 1);
                    posePositionsscore2[index] = keyPoints1[j].z;
                }
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray3[index] = new Vector4(keyPoints2[j].x, keyPoints2[j].y, 0, 1);
                    posePositionsscore3[index] = keyPoints2[j].z;
                }
            }
            if (closestSkeletonIndex2 == 2)
            {
                Vector3[] keyPoints1 = skeletons[2].keypoints;
                Vector3[] keyPoints2 = skeletons[0].keypoints;
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray2[index] = new Vector4(keyPoints1[j].x, keyPoints1[j].y, 0, 1);
                    posePositionsscore2[index] = keyPoints1[j].z;
                }
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray3[index] = new Vector4(keyPoints2[j].x, keyPoints2[j].y, 0, 1);
                    posePositionsscore3[index] = keyPoints2[j].z;
                }
            }
            for (int j = 0; j < 17; j++)
            {
                int index = j;
                posePositionsArray[index] = new Vector4(keyPoints0[j].x, keyPoints0[j].y, 0, 1);
                posePositionsscore[index] = keyPoints0[j].z;
            }
        }
        if (closestSkeletonIndex == 2)
        {
            Vector3[] keyPoints0 = skeletons[2].keypoints;
            if (closestSkeletonIndex2 == 1)
            {
                Vector3[] keyPoints1 = skeletons[1].keypoints;
                Vector3[] keyPoints2 = skeletons[0].keypoints;
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray2[index] = new Vector4(keyPoints1[j].x, keyPoints1[j].y, 0, 1);
                    posePositionsscore2[index] = keyPoints1[j].z;
                }
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray3[index] = new Vector4(keyPoints2[j].x, keyPoints2[j].y, 0, 1);
                    posePositionsscore3[index] = keyPoints2[j].z;
                }
            }
            if (closestSkeletonIndex2 == 0)
            {
                Vector3[] keyPoints1 = skeletons[0].keypoints;
                Vector3[] keyPoints2 = skeletons[1].keypoints;
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray2[index] = new Vector4(keyPoints1[j].x, keyPoints1[j].y, 0, 1);
                    posePositionsscore2[index] = keyPoints1[j].z;
                }
                for (int j = 0; j < 17; j++)
                {
                    int index = j;
                    posePositionsArray3[index] = new Vector4(keyPoints2[j].x, keyPoints2[j].y, 0, 1);
                    posePositionsscore3[index] = keyPoints2[j].z;
                }
            }
            for (int j = 0; j < 17; j++)
            {
                int index = j;
                posePositionsArray[index] = new Vector4(keyPoints0[j].x, keyPoints0[j].y, 0, 1);
                posePositionsscore[index] = keyPoints0[j].z;
            }
        }

        float[] prValues = new float[3];

        float[] pt = new float[3];
        pt[0] = posePositionsArray[0].x;
        pt[1] = posePositionsArray2[0].x;
        pt[2] = posePositionsArray3[0].x;
        pvt1 = posePositionsArray[0].x;
        pvt2 = posePositionsArray2[0].x;
        pvt3 = posePositionsArray3[0].x;
     

        float pfa = Mathf.Max(posePositionsscore[11], posePositionsscore[0]);
        if (pfa == pa)
        {
            framesConditionTrueA++; 
            if (framesConditionTrueA >= 10)
            {
                mpa = 0; 
            }
        }
        else
        {
            mpa = 1;
            framesConditionTrueA = 0;        
        }
        float pfb = Mathf.Max(posePositionsscore2[11], posePositionsscore2[0]);
        if (pfb == pb)
        {
            framesConditionTrueB++;
            if (framesConditionTrueB >= 10) 
            {
                mpb = 0;         
            }
        }
        else
        {
            mpb = 1;
            framesConditionTrueB = 0; 
        }
        float pfc = Mathf.Max(posePositionsscore3[11], posePositionsscore3[0]);
        if (pfc == pc)
        {
            framesConditionTrueC++; 
            if (framesConditionTrueC >= 10) 
            {
                mpc = 0;
            }
        }
        else
        {
            mpc = 1;
            framesConditionTrueC = 0; 
        }
        person1 -= 0.02f;
        person2 -= 0.02f;
        person3 -= 0.02f;
        float v = 0.3f;
        if (pfa*mpa > v && pfb*mpb > v && pfc*mpc > v)
        {
            person3 += 0.05f;
        }
        if (pfa * mpa > v && pfb *mpb > v || pfb * mpb > v && pfc * mpc > v || pfa * mpa > v && pfc * mpc > v)
        {
            person2 += 0.05f;
        }
        if (pfc * mpc > v || pfb * mpb > v || pfa * mpa > v)
        {
            person1 += 0.05f;
        }
        person1 = Mathf.Clamp(person1, 0f, 2f);
        person2 = Mathf.Clamp(person2, 0f, 2f);
        person3 = Mathf.Clamp(person3, 0f, 2f);
        nbperson = poses.Length;


        previousHipPosition = Vector3.SmoothDamp(previousHipPosition, new Vector3(posePositionsArray[0].x, posePositionsArray[0].y, pr), ref smoothDampVelocity, smoothingTime);

        previousHipPosition2 = Vector3.SmoothDamp(previousHipPosition2, new Vector3(posePositionsArray[0].x, posePositionsArray[0].y, pbr), ref smoothDampVelocity2, smoothingTime);
        // pn0 = Vector2.SmoothDamp(pn0, new Vector2(posePositionsArray[0].x, posePositionsArray[0].y), ref spn0, 0.3f);

        //"nose", "leftShoulder", "rightShoulder", "leftElbow", "rightElbow", "leftWrist", "rightWrist", "leftHip", "rightHip", "leftKnee", "rightKnee", "leftAnkle", "rightAnkle"
        if (pfa * mpa >score)
        {
            material.SetVectorArray("_pos", posePositionsArray);
           
            if (Smooth == false)
            {
                pos1[7] = new Vector2((posePositionsArray[11].x + posePositionsArray[12].x) * 0.5f, (posePositionsArray[11].y + posePositionsArray[12].y) * 0.5f);
                
                pos1[0] = ((new Vector2(posePositionsArray[0].x, posePositionsArray[0].y)) );
                pos1[1] = ((new Vector2(posePositionsArray[5].x, posePositionsArray[5].y) ));
                pos1[2] = ((new Vector2(posePositionsArray[6].x, posePositionsArray[6].y) ) );
                pos1[3] = ((new Vector2(posePositionsArray[7].x, posePositionsArray[7].y) ));
                pos1[4] = ((new Vector2(posePositionsArray[8].x, posePositionsArray[8].y) ) );
                pos1[5] = ((new Vector2(posePositionsArray[9].x, posePositionsArray[9].y) ) );
                pos1[6] = ((new Vector2(posePositionsArray[10].x, posePositionsArray[10].y) ));
                pos1[8] = ((new Vector2(posePositionsArray[13].x, posePositionsArray[13].y)) );
                pos1[9] = ((new Vector2(posePositionsArray[14].x, posePositionsArray[14].y) ));
                pos1[10] = ((new Vector2(posePositionsArray[15].x, posePositionsArray[15].y)) );
                pos1[11] = ((new Vector2(posePositionsArray[16].x, posePositionsArray[16].y) ) );
            }
            else
            {
                pos1[7] = Vector2.SmoothDamp(pos1[7], new Vector2((posePositionsArray[11].x + posePositionsArray[12].x) * 0.5f, (posePositionsArray[11].y + posePositionsArray[12].y) * 0.5f), ref pos1s[7], 0.3f);
                pos1[0] = Vector2.SmoothDamp(pos1[0],new Vector2(posePositionsArray[0].x, posePositionsArray[0].y),ref pos1s[0],0.3f);
                pos1[1] = Vector2.SmoothDamp(pos1[1], new Vector2(posePositionsArray[5].x, posePositionsArray[5].y), ref pos1s[1], 0.3f);
                pos1[2] = Vector2.SmoothDamp(pos1[2], new Vector2(posePositionsArray[6].x, posePositionsArray[6].y), ref pos1s[2], 0.3f);
                pos1[3] = Vector2.SmoothDamp(pos1[3], new Vector2(posePositionsArray[7].x, posePositionsArray[7].y), ref pos1s[3], 0.3f);
                pos1[4] = Vector2.SmoothDamp(pos1[4], new Vector2(posePositionsArray[8].x, posePositionsArray[8].y), ref pos1s[4], 0.3f);
                pos1[5] = Vector2.SmoothDamp(pos1[5], new Vector2(posePositionsArray[9].x, posePositionsArray[9].y), ref pos1s[5], 0.3f);
                pos1[6] = Vector2.SmoothDamp(pos1[6], new Vector2(posePositionsArray[10].x, posePositionsArray[10].y), ref pos1s[6], 0.3f);
                pos1[8] = Vector2.SmoothDamp(pos1[8], new Vector2(posePositionsArray[13].x, posePositionsArray[13].y), ref pos1s[8], 0.3f);
                pos1[9] = Vector2.SmoothDamp(pos1[9], new Vector2(posePositionsArray[14].x, posePositionsArray[14].y), ref pos1s[9], 0.3f);
                pos1[10] = Vector2.SmoothDamp(pos1[10], new Vector2(posePositionsArray[15].x, posePositionsArray[15].y), ref pos1s[10], 0.3f);
                pos1[11] = Vector2.SmoothDamp(pos1[11], new Vector2(posePositionsArray[16].x, posePositionsArray[16].y), ref pos1s[11], 0.3f);
            }

            score1[0] = (posePositionsscore[0]);
            score1[1] = (posePositionsscore[5]);
            score1[2] = (posePositionsscore[6]);
            score1[3] = (posePositionsscore[7]);
            score1[4] = (posePositionsscore[8]);
            score1[5] = (posePositionsscore[9]);
            score1[6] = (posePositionsscore[10]);
            score1[8] = (posePositionsscore[13]);
            score1[7] = ((posePositionsscore[11] + posePositionsscore[12]) * 0.5f);
            score1[9] = (posePositionsscore[14]);
            score1[10] = (posePositionsscore[15]);
            score1[11] = (posePositionsscore[16]);
            pp = Mathf.Min(posePositionsArray[15].y, posePositionsArray[16].y);
        }
        else
        {
            //Debug.Log("true0");
            score1[0] = 0;
            score1[1] = 0;
            score1[2] = 0;
            score1[3] = 0;
            score1[4] = 0;
            score1[5] = 0;
            score1[6] = 0;
            score1[8] = 0;
            score1[7] = 0;
            score1[9] = 0;
            score1[10] = 0;
            score1[11] = 0;
        }
        if (pfb * mpb >score)
        {
            material.SetVectorArray("_pos2", posePositionsArray2);

            if (Smooth == false)
            {
                pos2[7] = new Vector2((posePositionsArray2[11].x + posePositionsArray2[12].x) * 0.5f, (posePositionsArray2[11].y + posePositionsArray2[12].y) * 0.5f);
                Vector2 p72 = new Vector2(pos2[7].x, pos2[7].y);
                pr = 5 * Vector2.Distance(p72, new Vector2((posePositionsArray2[5].x + posePositionsArray2[6].x), (posePositionsArray2[5].y + posePositionsArray2[6].y)) * 0.5f);
                pos2[0] = ((new Vector2(posePositionsArray2[0].x, posePositionsArray2[0].y) - p72) / pr + a);
                pos2[1] = ((new Vector2(posePositionsArray2[5].x, posePositionsArray2[5].y) - p72) / pr + a);
                pos2[2] = ((new Vector2(posePositionsArray2[6].x, posePositionsArray2[6].y) - p72) / pr + a);
                pos2[3] = ((new Vector2(posePositionsArray2[7].x, posePositionsArray2[7].y) - p72) / pr + a);
                pos2[4] = ((new Vector2(posePositionsArray2[8].x, posePositionsArray2[8].y) - p72) / pr + a);
                pos2[5] = ((new Vector2(posePositionsArray2[9].x, posePositionsArray2[9].y) - p72) / pr + a);
                pos2[6] = ((new Vector2(posePositionsArray2[10].x, posePositionsArray2[10].y) - p72) / pr + a);
                pos2[8] = ((new Vector2(posePositionsArray2[13].x, posePositionsArray2[13].y) - p72) / pr + a);
                pos2[9] = ((new Vector2(posePositionsArray2[14].x, posePositionsArray2[14].y) - p72) / pr + a);
                pos2[10] = ((new Vector2(posePositionsArray2[15].x, posePositionsArray2[15].y) - p72) / pr + a);
                pos2[11] = ((new Vector2(posePositionsArray2[16].x, posePositionsArray2[16].y) - p72) / pr + a);
            }
            else
            {
                pos2[7] = Vector2.SmoothDamp(pos2[7], new Vector2((posePositionsArray2[11].x + posePositionsArray2[12].x) * 0.5f, (posePositionsArray2[11].y + posePositionsArray2[12].y) * 0.5f), ref pos2s[7], 0.3f);
                pos2[0] = Vector2.SmoothDamp(pos2[0], new Vector2(posePositionsArray2[0].x, posePositionsArray2[0].y), ref pos2s[0], 0.3f);
                pos2[1] = Vector2.SmoothDamp(pos2[1], new Vector2(posePositionsArray2[5].x, posePositionsArray2[5].y), ref pos2s[1], 0.3f);
                pos2[2] = Vector2.SmoothDamp(pos2[2], new Vector2(posePositionsArray2[6].x, posePositionsArray2[6].y), ref pos2s[2], 0.3f);
                pos2[3] = Vector2.SmoothDamp(pos2[3], new Vector2(posePositionsArray2[7].x, posePositionsArray2[7].y), ref pos2s[3], 0.3f);
                pos2[4] = Vector2.SmoothDamp(pos2[4], new Vector2(posePositionsArray2[8].x, posePositionsArray2[8].y), ref pos2s[4], 0.3f);
                pos2[5] = Vector2.SmoothDamp(pos2[5], new Vector2(posePositionsArray2[9].x, posePositionsArray2[9].y), ref pos2s[5], 0.3f);
                pos2[6] = Vector2.SmoothDamp(pos2[6], new Vector2(posePositionsArray2[10].x, posePositionsArray2[10].y), ref pos2s[6], 0.3f);
                pos2[8] = Vector2.SmoothDamp(pos2[8], new Vector2(posePositionsArray2[13].x, posePositionsArray2[13].y), ref pos2s[8], 0.3f);
                pos2[9] = Vector2.SmoothDamp(pos2[9], new Vector2(posePositionsArray2[14].x, posePositionsArray2[14].y), ref pos2s[9], 0.3f);
                pos2[10] = Vector2.SmoothDamp(pos2[10], new Vector2(posePositionsArray2[15].x, posePositionsArray2[15].y), ref pos2s[10], 0.3f);
                pos2[11] = Vector2.SmoothDamp(pos2[11], new Vector2(posePositionsArray2[16].x, posePositionsArray2[16].y), ref pos2s[11], 0.3f);
            }
            score2[0] = (posePositionsscore2[0]);
            score2[1] = (posePositionsscore2[5]);
            score2[2] = (posePositionsscore2[6]);
            score2[3] = (posePositionsscore2[7]);
            score2[4] = (posePositionsscore2[8]);
            score2[5] = (posePositionsscore2[9]);
            score2[6] = (posePositionsscore2[10]);
            score2[8] = (posePositionsscore2[13]);
            score2[7] = ((posePositionsscore2[11] + posePositionsscore2[12]) * 0.5f);
            score2[9] = (posePositionsscore2[14]);
            score2[10] = (posePositionsscore2[15]);
            score2[11] = (posePositionsscore2[16]);
            ppb = Mathf.Min(posePositionsArray2[15].y, posePositionsArray2[16].y);
        }
        else
        {
            //Debug.Log("true01");
            score2[0] = 0;
            score2[1] = 0;
            score2[2] = 0;
            score2[3] = 0;
            score2[4] = 0;
            score2[5] = 0;
            score2[6] = 0;
            score2[8] = 0;
            score2[7] = 0;
            score2[9] = 0;
            score2[10] = 0;
            score2[11] = 0;
        }
        if (pfc * mpc >score)
        {
            material.SetVectorArray("_pos3", posePositionsArray3);
            if (Smooth == false)
            {
                pos3[7] = new Vector2((posePositionsArray3[11].x + posePositionsArray3[12].x) * 0.5f, (posePositionsArray3[11].y + posePositionsArray3[12].y) * 0.5f);
                Vector2 p72 = new Vector2(pos3[7].x, pos3[7].y);
                pr = 5 * Vector2.Distance(p72, new Vector2((posePositionsArray3[5].x + posePositionsArray3[6].x), (posePositionsArray3[5].y + posePositionsArray3[6].y)) * 0.5f);
                pos3[0] = ((new Vector2(posePositionsArray3[0].x, posePositionsArray3[0].y) - p72) / pr + a);
                pos3[1] = ((new Vector2(posePositionsArray3[5].x, posePositionsArray3[5].y) - p72) / pr + a);
                pos3[2] = ((new Vector2(posePositionsArray3[6].x, posePositionsArray3[6].y) - p72) / pr + a);
                pos3[3] = ((new Vector2(posePositionsArray3[7].x, posePositionsArray3[7].y) - p72) / pr + a);
                pos3[4] = ((new Vector2(posePositionsArray3[8].x, posePositionsArray3[8].y) - p72) / pr + a);
                pos3[5] = ((new Vector2(posePositionsArray3[9].x, posePositionsArray3[9].y) - p72) / pr + a);
                pos3[6] = ((new Vector2(posePositionsArray3[10].x, posePositionsArray3[10].y) - p72) / pr + a);
                pos3[8] = ((new Vector2(posePositionsArray3[13].x, posePositionsArray3[13].y) - p72) / pr + a);
                pos3[9] = ((new Vector2(posePositionsArray3[14].x, posePositionsArray3[14].y) - p72) / pr + a);
                pos3[10] = ((new Vector2(posePositionsArray3[15].x, posePositionsArray3[15].y) - p72) / pr + a);
                pos3[11] = ((new Vector2(posePositionsArray3[16].x, posePositionsArray3[16].y) - p72) / pr + a);
            }
            else
            {
                pos3[7] = Vector2.SmoothDamp(pos3[7], new Vector2((posePositionsArray3[11].x + posePositionsArray3[12].x) * 0.5f, (posePositionsArray3[11].y + posePositionsArray3[12].y) * 0.5f), ref pos3s[7], 0.3f);
                pos3[0] = Vector2.SmoothDamp(pos3[0], new Vector2(posePositionsArray3[0].x, posePositionsArray3[0].y), ref pos3s[0], 0.3f);
                pos3[1] = Vector2.SmoothDamp(pos3[1], new Vector2(posePositionsArray3[5].x, posePositionsArray3[5].y), ref pos3s[1], 0.3f);
                pos3[2] = Vector2.SmoothDamp(pos3[2], new Vector2(posePositionsArray3[6].x, posePositionsArray3[6].y), ref pos3s[2], 0.3f);
                pos3[3] = Vector2.SmoothDamp(pos3[3], new Vector2(posePositionsArray3[7].x, posePositionsArray3[7].y), ref pos3s[3], 0.3f);
                pos3[4] = Vector2.SmoothDamp(pos3[4], new Vector2(posePositionsArray3[8].x, posePositionsArray3[8].y), ref pos3s[4], 0.3f);
                pos3[5] = Vector2.SmoothDamp(pos3[5], new Vector2(posePositionsArray3[9].x, posePositionsArray3[9].y), ref pos3s[5], 0.3f);
                pos3[6] = Vector2.SmoothDamp(pos3[6], new Vector2(posePositionsArray3[10].x, posePositionsArray3[10].y), ref pos3s[6], 0.3f);
                pos3[8] = Vector2.SmoothDamp(pos3[8], new Vector2(posePositionsArray3[13].x, posePositionsArray3[13].y), ref pos3s[8], 0.3f);
                pos3[9] = Vector2.SmoothDamp(pos3[9], new Vector2(posePositionsArray3[14].x, posePositionsArray3[14].y), ref pos3s[9], 0.3f);
                pos3[10] = Vector2.SmoothDamp(pos3[10], new Vector2(posePositionsArray3[15].x, posePositionsArray3[15].y), ref pos3s[10], 0.3f);
                pos3[11] = Vector2.SmoothDamp(pos3[11], new Vector2(posePositionsArray3[16].x, posePositionsArray3[16].y), ref pos3s[11], 0.3f);
            }
            score3[0] = posePositionsscore3[0];
            score3[1] = posePositionsscore3[5];
            score3[2] = posePositionsscore3[6];
            score3[3] = posePositionsscore3[7];
            score3[4] = posePositionsscore3[8];
            score3[5] = posePositionsscore3[9];
            score3[6] = posePositionsscore3[10];
            score3[8] = posePositionsscore3[13];
            score3[7] = (posePositionsscore3[11] + posePositionsscore3[12]) * 0.5f;
            score3[9] = posePositionsscore3[14];
            score3[10] = posePositionsscore3[15];
            score3[11] = posePositionsscore3[16];
            ppc = Mathf.Min(posePositionsArray3[15].y, posePositionsArray3[16].y);
        }
        else
        {
            //Debug.Log("true02");
            score3[0] = 0;
            score3[1] = 0;
            score3[2] = 0;
            score3[3] = 0;
            score3[4] = 0;
            score3[5] = 0;
            score3[6] = 0;
            score3[8] = 0;
            score3[7] = 0;
            score3[9] = 0;
            score3[10] = 0;
            score3[11] = 0;
        }
        material.SetFloat("_pr", pr);
        material.SetFloat("_pp", pp);
        material.SetFloat("_pos1", skeletons[0].keypoints[0].x);
        material.SetFloat("_pos2a", skeletons[1].keypoints[0].x);
        material.SetFloat("_pos3a", skeletons[2].keypoints[0].x);
        material.SetFloatArray("_score", score1);
        material.SetFloatArray("_score1", score2);
        material.SetFloatArray("_score2", score3);
        material.SetFloat("_ttsocre", score);
        pa = Mathf.Max(posePositionsscore[11], posePositionsscore[0]);
        pb = Mathf.Max(posePositionsscore2[11], posePositionsscore2[0]);
        pc = Mathf.Max(posePositionsscore3[11], posePositionsscore3[0]);
    }
    private void OnDisable()
    {
        engine.worker.Dispose();
    }
}
