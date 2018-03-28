using UnityEngine;

public class PlayerLadderController : MonoBehaviour {
	public PlayerStats playerStats;
	public LayerMask ladderLayer;
	public float climbSpeed = 2f;
	public float ladderOffset = 0.7f;
	Rigidbody2D playerRigidbody;

	void Start() {
		playerRigidbody = GetComponent<Rigidbody2D>();
		playerStats.isOnLadder = false;
		playerStats.onTopOfLadder = false;
		playerStats.finishedClimbingLadder = true;
	}

	void Update() {
		if (playerStats.isOnLadder) {
			CheckJumpingOut();
			Climbing();
			CheckLeavingLadder();
			OnTopOfLadder();
		} else {
			CheckDescending();
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (Input.GetAxis("Vertical")> 0f && CheckTouchingLadder() && other.tag == "Stairs") {
			Vector3 stairsCenter = new Vector3(
				other.transform.position.x, transform.position.y, transform.position.z);
			transform.position = stairsCenter;
			EnableStairsClimb(true);
		}
	}

	bool CheckTouchingLadder() {
		return (Physics2D.Raycast(transform.position, Vector2.down,
			0f, ladderLayer));
	}

	void EnableStairsClimb(bool newState) {
		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = (newState? 0 : 1);
		Physics2D.IgnoreLayerCollision(9, 8, newState);
		playerStats.isOnLadder = newState;
		if (!playerStats.onTopOfLadder) {
			playerStats.finishedClimbingLadder = !newState;
		}
	}

	void CheckJumpingOut() {
		if (Input.GetButtonDown("Jump")) {
			EnableStairsClimb(false);
		}
	}

	void Climbing() {
		if (Input.GetAxis("Vertical")!= 0f) {
			playerRigidbody.velocity = (new Vector2(0f, climbSpeed))*
				(Input.GetAxis("Vertical")>= 0f? 1f: -1f);
		} else {
			playerRigidbody.velocity = Vector2.zero;
		}
	}

	void CheckDescending() {
		RaycastHit2D checkedLadder = OnTopOfLadder();
		if (checkedLadder && playerStats.finishedClimbingLadder && (Input.GetAxis("Vertical") < 0f)) {
			transform.position = checkedLadder.transform.position;
			EnableStairsClimb(true);
		}
	}

	RaycastHit2D OnTopOfLadder() {
		RaycastHit2D checkLadder = Physics2D.Raycast(transform.position, Vector2.down,
			ladderOffset, ladderLayer);
		playerStats.onTopOfLadder = checkLadder;
		return checkLadder;
	}

	void CheckLeavingLadder() {
		if (!CheckTouchingLadder()) {
			EnableStairsClimb(false);
		}
	}
}