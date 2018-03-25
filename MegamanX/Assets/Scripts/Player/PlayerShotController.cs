using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : MonoBehaviour {
	public Shot shotPrefab;
	public Transform jumpShotStartPoint;
	public Transform runningShotStartPoint;
	public Transform standShotStartPoint;
	public PlayerStats playerStats;

	void Update() {
		if (playerStats.isControllable) {
			ShootController();
		}
	}

	void ShootController() {
		Transform shotStartPoint;
		if (!playerStats.isGrounded) {
			shotStartPoint = jumpShotStartPoint;
		} else if (playerStats.isMoving) {
			shotStartPoint = runningShotStartPoint;
		} else {
			shotStartPoint = standShotStartPoint;
		}
		if (Input.GetButtonDown("Fire1")) {
			Instantiate(shotPrefab, shotStartPoint.position, Quaternion.identity);
			playerStats.isShooting = true;
		}
	}
}