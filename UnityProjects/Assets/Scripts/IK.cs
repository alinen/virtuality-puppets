using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IK : MonoBehaviour
{
	public Transform target;
	public Transform endEffector;
	public Vector3 middleAxis;

	public Transform originTarget;
	public Transform head;

	private Transform middleJoint;
	private Transform baseJoint;
	// Start is called before the first frame update
	void Start()
    {
		middleJoint = endEffector.parent;
		baseJoint = middleJoint.parent;
	}

	// Update is called once per frame
	void Update()
    {
		if (target == null || endEffector == null) return;

		// Calculate the distances
		float upperArmLength = Vector3.Distance(baseJoint.position, middleJoint.position);  // Shoulder to elbow
		float lowerArmLength = Vector3.Distance(middleJoint.position, endEffector.position);  // Elbow to hand
		float targetDistance = Vector3.Distance(baseJoint.position, target.position);  // Shoulder to target

		Vector3 targetPos = target.position - originTarget.position + head.position;
		
		// Clamp target distance to ensure it's within the arm's total length
		targetDistance = Mathf.Min(targetDistance, upperArmLength + lowerArmLength);
		Vector3 baseToMiddle = middleJoint.position - baseJoint.position;
		Vector3 middleToTarget = targetPos - middleJoint.position;
		Vector3 baseToEnd = targetPos - baseJoint.position;
		Vector3 endToTargetEffector = targetPos - endEffector.position;
		float l1 = baseToMiddle.magnitude;
		float l2 = (endEffector.position - middleJoint.position).magnitude;
		float r = baseToEnd.magnitude;

		float cosTheta = Mathf.Clamp((l1 * l1 + l2 * l2 - r * r) / (2 * l1 * l2), -1f, 1f);
		float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
		middleJoint.localRotation = Quaternion.AngleAxis(180 - theta, middleAxis);

		//Vector3 middleRotationAxis = Vector3.Cross(baseToMiddle, middleToTarget).normalized;
		Vector3 baseRotationAxis = Vector3.Cross(baseToEnd, endToTargetEffector).normalized;

		Vector3 crossProduct = Vector3.Cross(baseToEnd, endToTargetEffector);
		float dotSum = Vector3.Dot(baseToEnd, baseToEnd) + Vector3.Dot(baseToEnd, endToTargetEffector);
		float phi = Mathf.Atan2(crossProduct.magnitude, dotSum) * Mathf.Rad2Deg;

		baseJoint.rotation = Quaternion.AngleAxis(phi, baseRotationAxis) * baseJoint.rotation;
		// Debug lines to visualize arm positions
		Debug.DrawLine(baseJoint.position, middleJoint.position, Color.red);
		Debug.DrawLine(middleJoint.position, endEffector.position, Color.blue);
		Debug.DrawLine(endEffector.position, targetPos, Color.green);
		float grandparentToEndEffectorDistance = Vector3.Distance(baseJoint.position, endEffector.position);
		float grandparentToTargetDistance = Vector3.Distance(baseJoint.position, targetPos);
		// Print the calculated distances
		Debug.Log("The first distance should be smaller or the same: " + grandparentToEndEffectorDistance + " and " + grandparentToTargetDistance);
	}
}
