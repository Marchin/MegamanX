using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour {
	Camera mainCamera;
	Collider2D aCameraBound;

	void Start() {
		mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		aCameraBound = GetComponent<Collider2D>();
	}

	void Update() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerMovement>()) {
			mainCamera.GetComponent<CameraController>()
				.AddBounds(aCameraBound.bounds.min, aCameraBound.bounds.max);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<PlayerMovement>()) {
			mainCamera.GetComponent<CameraController>()
				.RemoveBounds(aCameraBound.bounds.min, aCameraBound.bounds.max);
		}
	}
}