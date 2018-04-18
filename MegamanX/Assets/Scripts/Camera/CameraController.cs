using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform target;
	Camera cameraComponent;
	List<float> boundsX = null;
	List<float> boundsY = null;
	Vector3 bottomLeftBound;
	Vector3 topRightBound;
	float cameraOffsetX;
	float cameraOffsetY;
	float cameraSmoothness;

	void Awake() {
		cameraComponent = GetComponent<Camera>();
		cameraOffsetY = cameraComponent.orthographicSize;
		cameraOffsetX = cameraOffsetY * cameraComponent.aspect;//*(Screen.width / Screen.height)* 2;
		bottomLeftBound = cameraComponent.transform.position;
		topRightBound = cameraComponent.transform.position;
		boundsX = new List<float>();
		boundsY = new List<float>();
	}

	void LateUpdate() {
		if (boundsX.Count > 0) {
			CameraMovement();
		}
	}

	void CameraMovement() {
		print(boundsX.Count);
		transform.position = new Vector3(
			Mathf.Clamp(target.position.x, bottomLeftBound.x, topRightBound.x),
			Mathf.Clamp(target.position.y, bottomLeftBound.y, topRightBound.y),
			transform.position.z
		);
	}
	void CameraMovement2() {
		transform.position = new Vector3(
			Mathf.Clamp(target.position.x, bottomLeftBound.x, topRightBound.x),
			Mathf.Clamp(target.position.y, bottomLeftBound.y, topRightBound.y),
			transform.position.z
		);
	}

	public void AddBounds(Vector3 bottomLeftCorner, Vector3 topRightCorner) {
		boundsX.Add(bottomLeftCorner.x);
		boundsY.Add(bottomLeftCorner.y);
		boundsX.Add(topRightCorner.x);
		boundsY.Add(topRightCorner.y);
		boundsX.Sort();
		boundsY.Sort();
		SetBounds(boundsX[0], boundsY[0], boundsX[boundsX.Count - 1], boundsY[boundsY.Count - 1]);
	}

	public void RemoveBounds(Vector3 bottomLeftCorner, Vector3 topRightCorner) {
		boundsX.Remove(bottomLeftCorner.x);
		boundsY.Remove(bottomLeftCorner.y);
		boundsX.Remove(topRightCorner.x);
		boundsY.Remove(topRightCorner.y);
		if (boundsX.Count > 0) {
			SetBounds(boundsX[0], boundsY[0], boundsX[boundsX.Count - 1], boundsY[boundsY.Count - 1]);
		}
	}

	void SetBounds(float minX, float minY, float maxX, float maxY) {
		bottomLeftBound = new Vector3(
			minX + cameraOffsetX,
			minY + cameraOffsetY,
			transform.position.z
		);
		topRightBound = new Vector3(
			maxX - cameraOffsetX,
			maxY - cameraOffsetY,
			transform.position.z
		);
	}
}