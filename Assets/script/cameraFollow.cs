using UnityEngine;

public class cameraFollow : MonoBehaviour{
	public float minDistance = 0.25f;
	public float followSpeed = 5f;
	public Transform target;

	Vector3 posNoZ, targetDirection, targetPos;
	float interpVelocity;

	void FixedUpdate(){
		if (target) {
			posNoZ 				= transform.position;
			posNoZ.z			= target.position.z;
			targetDirection 	= target.position - posNoZ;
			interpVelocity		= targetDirection.magnitude * followSpeed;
			targetPos 			= transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
			transform.position 	= Vector3.Lerp (transform.position, targetPos, minDistance);
		}
		else Debug.LogError ("target is null (cameraFollow)");

	}
}