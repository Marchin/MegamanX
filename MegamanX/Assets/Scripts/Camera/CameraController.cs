using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform target;
	Camera cameraComponent;
	Vector3 bottomLeftBound;
	Vector3 topRightBound;
	float cameraOffsetX;
	float cameraOffsetY;
	float cameraSmoothness;

	void Start() {
		cameraComponent = GetComponent<Camera>();
		cameraOffsetY = cameraComponent.orthographicSize;
		cameraOffsetX = cameraOffsetY * (Screen.width / Screen.height);
	}

	void LateUpdate() {
		CameraMovement();
	}

	void CameraMovement() {
		transform.position = new Vector3(
			Mathf.Clamp(target.position.x, bottomLeftBound.x, topRightBound.x),
			Mathf.Clamp(target.position.y, bottomLeftBound.y, topRightBound.y),
			transform.position.z
		);
	}

	public void SetBounds(Vector3 bottomLeftCorner, Vector3 topRightCorner) {
		bottomLeftBound = new Vector3(
			bottomLeftCorner.x + cameraOffsetX,
			bottomLeftCorner.y + cameraOffsetY,
			transform.position.z
		);
		topRightBound = new Vector3(
			topRightCorner.x - cameraOffsetX,
			topRightCorner.y - cameraOffsetY,
			transform.position.z
		);
	}
}