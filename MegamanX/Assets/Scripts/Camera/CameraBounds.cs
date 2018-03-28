using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour {
	public Camera mainCamera;
	Collider2D aCameraBound;
	// Use this for initialization
	void Start() {
		aCameraBound = GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerMovement>()) {
			mainCamera.GetComponent<CameraController>()
				.SetBounds(aCameraBound.bounds.min, aCameraBound.bounds.max);
		}
	}
}