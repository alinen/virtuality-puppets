using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IK : MonoBehaviour
{
	public Transform target;
	public Transform endEffector;
	public Vector3 middleAxis;

	// For handling relative offsets for the hands from the head
	public Transform originTarget;
	public Transform head;

	private Transform middleJoint;
	private Transform baseJoint;
	private Quaternion restRot;

	// Start is called before the first frame update
	void Start()
    {
		middleJoint = endEffector.parent;
		baseJoint = middleJoint.parent;
		restRot = baseJoint.rotation;
	}

	// Update is called once per frame
	void Update()
    {
		if (target == null || endEffector == null) return;

		baseJoint.rotation = restRot;
		Vector3 targetPos = target.position - originTarget.position + head.position;

		// Calculate the distances
		float upperArmLength = Vector3.Distance(baseJoint.position, middleJoint.position);  // Shoulder to elbow
		float lowerArmLength = Vector3.Distance(middleJoint.position, endEffector.position);  // Elbow to hand
		
		Vector3 baseToMiddle = middleJoint.position - baseJoint.position;
		Vector3 middleToTarget = targetPos - middleJoint.position;
		float l1 = baseToMiddle.magnitude;
		float l2 = (endEffector.position - middleJoint.position).magnitude;
		float r = (targetPos - baseJoint.position).magnitude;

		float cosTheta = Mathf.Clamp((l1 * l1 + l2 * l2 - r * r) / (2 * l1 * l2), -1f, 1f);
		float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
		middleJoint.localRotation = Quaternion.AngleAxis(180 - theta, middleAxis);

		Vector3 baseToEnd = endEffector.position - baseJoint.position;
		Vector3 targetToEndEffector = targetPos - endEffector.position;
		Vector3 baseRotationAxis = Vector3.Cross(baseToEnd, targetToEndEffector).normalized;
		Vector3 crossProduct = Vector3.Cross(baseToEnd, targetToEndEffector);
		float dotSum = Vector3.Dot(baseToEnd, baseToEnd) + Vector3.Dot(baseToEnd, targetToEndEffector);
		float phi = Mathf.Atan2(crossProduct.magnitude, dotSum) * Mathf.Rad2Deg;
		
		baseJoint.rotation = Quaternion.AngleAxis(phi, baseRotationAxis) * baseJoint.rotation;

		// Debug lines to visualize arm positions
		//Debug.DrawLine(baseJoint.position, middleJoint.position, Color.red);
		//Debug.DrawLine(middleJoint.position, endEffector.position, Color.blue);
		//Debug.DrawLine(endEffector.position, targetPos, Color.green);
		float grandparentToEndEffectorDistance = Vector3.Distance(baseJoint.position, endEffector.position);
		float grandparentToTargetDistance = Vector3.Distance(baseJoint.position, targetPos);
		// Print the calculated distances
		Debug.Log("The first distance should be smaller or the same: " + grandparentToEndEffectorDistance + " and " + grandparentToTargetDistance);
	}

    void OnDrawGizmosSelected()
    {
		Vector3 targetPos = target.position - originTarget.position + head.position;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 0.05f);
    }
}
