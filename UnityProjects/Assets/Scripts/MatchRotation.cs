using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotation : MonoBehaviour
{
    public GameObject target;
    public GameObject source;
    public Vector3 offsetRot = new Vector3(90, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion offset = Quaternion.Euler(offsetRot.x, offsetRot.y, offsetRot.z);
        target.transform.rotation = source.transform.rotation * offset;
    }
}
