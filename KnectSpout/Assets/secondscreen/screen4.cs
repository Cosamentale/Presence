using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screen4 : MonoBehaviour
{
    public PoseNetCompo script;
    public Material mat;
    private Vector4 smoothDampVelocity;
    private Vector4 smoothDampVelocity2;
    private float smoothingTime = 0.1f;
    public Vector4 pp;
    public Vector4 pp2;
    public static Vector4 Vector4SmoothDamp(Vector4 current, Vector4 target, ref Vector4 currentVelocity, float smoothTime)
    {
        current.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime);
        current.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime);
        current.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime);
        current.w = Mathf.SmoothDamp(current.w, target.w, ref currentVelocity.w, smoothTime);

        return current;
    }

    void Update()
    {

        //"nose", 5"leftShoulder",6 "rightShoulder", "leftElbow", "rightElbow", "leftWrist", "rightWrist", "leftHip", "rightHip", "leftKnee", "rightKnee", "leftAnkle", "rightAnkle"
        Vector4 pa = new Vector4(script.pos1[9].x, script.pos1[9].y, script.pos1[10].x, script.pos1[10].y);
        Vector4 pb = new Vector4(script.pos1[7].x, script.pos1[7].y, script.pos1[8].x, script.pos1[8].y);
        pp = Vector4SmoothDamp(pp, pa, ref smoothDampVelocity, smoothingTime);
        pp2 = Vector4SmoothDamp(pp2, pb, ref smoothDampVelocity, smoothingTime);
        Vector4 pc = (pp - pp2) * 0.25f;
        Vector4 pr = pp  ;
        // Vector2 d = new Vector2 ( Mathf.Sqrt(pc.x * pc.x + pc.y * pc.y), Mathf.Sqrt(pc.z * pc.z + pc.w * pc.w));
        Vector2 d = new Vector2(script.pos1[9].z, script.pos1[10].z);
        mat.SetVector("_pos", pa);
        mat.SetVector("_d", d);
    }
}
