using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
	public PlayerStats playerStats;
	Animator animator;
	bool wasOnLadder;

	void Start() {
		animator = GetComponent<Animator>();
		wasOnLadder = playerStats.isOnLadder;
	}
	void LateUpdate() {
		CheckMovement();
		CheckJump();
		CheckGrounded();
		CheckLand();
		CheckShooting();
		CheckLadder();
		wasOnLadder = playerStats.isOnLadder;
	}

	void CheckMovement() {
		animator.SetBool("IsMoving", playerStats.isMoving);
	}

	void CheckJump() {
		if (playerStats.isJumping) {
			animator.SetTrigger("Jump");
			playerStats.isJumping = false;
		}
	}

	void CheckGrounded() {
		animator.SetBool("IsGrounded", playerStats.isGrounded);
	}

	void CheckLand() {
		animator.SetBool("IsToLand", playerStats.isToLand);
	}

	void CheckShooting() {
		if (playerStats.isShooting) {
			animator.SetBool("Shoot", playerStats.isShooting);
			playerStats.isShooting = false;
		}
	}

	void CheckLadder() {
		AnimatorClipInfo[] currentClipinfo = animator.GetCurrentAnimatorClipInfo(1);
		if (playerStats.isOnLadder) {
			animator.SetLayerWeight(1, 1f); //ver de usar eventos
			if (currentClipinfo[0].clip.name == "Ladder") {
				animator.speed = (Input.GetAxis("Vertical")!= 0f? 1f : 0);
			} else {
				animator.speed = 1f;
			}
		} else if (wasOnLadder && !playerStats.isOnLadder &&
			playerStats.onTopOfLadder && Input.GetAxis("Vertical")> 0f) {

			StartFinishingLadderClimbing();
		} else if (wasOnLadder && !playerStats.isOnLadder) {
			EndLadderAnimation();
		}
		if (playerStats.ladderFromTop) {
			animator.SetTrigger("LadderFromTop");
			playerStats.ladderFromTop = false;
		}
	}

	void StartFinishingLadderClimbing() {
		animator.SetTrigger("FinishingLadderClimbing");
		playerStats.isControllable = false;
		Invoke("EndFinishingLadderClimbing", 0.4f);
	}

	void EndFinishingLadderClimbing() {
		playerStats.isControllable = true;
		EndLadderAnimation();
	}

	void EndLadderAnimation() {
		animator.SetLayerWeight(1, 0f);
		animator.speed = 1f;
	}
}