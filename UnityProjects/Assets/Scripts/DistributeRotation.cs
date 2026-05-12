using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViconDataStreamSDK.CSharp;

public class DistributeRotation : MonoBehaviour
{
    public Transform target;
    public Transform source;

    public List<Transform> ancestors = new List<Transform>();
    Quaternion rootStartRot;
    public Transform root;

    // Start is called before the first frame update
    void Start()
    {
        InitAncestors(target, root);
        rootStartRot = root.rotation;
    }

    void InitAncestors(Transform node, Transform root)
    {
        while (node.parent != null && node != root)
        {
	    ancestors.Add(node);
            node = node.parent;
        }
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion tipRot = source.transform.rotation * Quaternion.Euler(90, 0, 0);
        Quaternion rootRot = rootStartRot;

        for (int i = ancestors.Count-1; i >= 0; i--)
        {
            float t = i/(float)(ancestors.Count-1);
            ancestors[i].rotation = Quaternion.Lerp(tipRot, rootRot, t);
        }
    }
}
