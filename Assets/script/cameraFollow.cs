using UnityEngine;

public class cameraFollow : MonoBehaviour{
	public float interpVelocity;
	public float minDistance = 0.25f;
	public float followSpeed = 5f;
	public Transform target;

	void FixedUpdate(){
		if (target) {
			Vector3 posNoZ = transform.position;
			posNoZ.z = target.transform.position.z;

			Vector3 targetDirection = (target.position - posNoZ);

			interpVelocity = targetDirection.magnitude * followSpeed;

			Vector3 targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

			transform.position = Vector3.Lerp (transform.position, targetPos, minDistance);
		}

		else Debug.LogError ("target is null (cameraFollow)");
	}
}
