using UnityEngine;

public class PlayerLadderController : MonoBehaviour {
	public PlayerStats playerStats;
	public LayerMask ladderLayer;
	Rigidbody2D playerRigidbody;
	public float climbSpeed = 2f;
	public float ladderOffset = 0.6f;
	const float fromTopAnimation = 0.2f;

	void Start() {
		playerRigidbody = GetComponent<Rigidbody2D>();
		playerStats.isOnLadder = false;
		playerStats.onTopOfLadder = false;
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
		if (Input.GetAxis("Vertical")> 0f && CheckTouchingLadder()&&
			other.tag == "Ladders") {

			AlingWithLadder();
			EnableStairsClimb(true);
		} else if (other.tag == "Level") {
			EnableStairsClimb(false);
		}
	}

	bool CheckTouchingLadder() {
		return (Physics2D.Raycast(transform.position, Vector2.down,
			0f, ladderLayer));
	}

	void AlingWithLadder() {
		Vector3 stairsCenter = new Vector3(
			Mathf.Floor(transform.position.x)+ 0.5f,
			transform.position.y,
			transform.position.z);
		transform.position = stairsCenter;
	}

	void EnableStairsClimb(bool newState) {
		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = (newState? 0f : 1f);
		Physics2D.IgnoreLayerCollision(9, 8, newState);
		playerStats.isOnLadder = newState;
	}

	void CheckJumpingOut() {
		if (Input.GetButtonDown("Jump")) {
			EnableStairsClimb(false);
		}
	}

	void Climbing() {
		if (Input.GetAxis("Vertical")!= 0f && playerStats.isControllable) {
			playerRigidbody.velocity = (new Vector2(0f, climbSpeed))*
				(Input.GetAxis("Vertical")>= 0f? 1f: -1f);
		} else {
			playerRigidbody.velocity = Vector2.zero;
		}
	}

	void OnTopOfLadder() {
		Vector3 topOfPlayer = transform.position + new Vector3(
			0f, ladderOffset, 0);
		RaycastHit2D checkLadder = Physics2D.Raycast(topOfPlayer,
			Vector2.down, ladderOffset * 2, ladderLayer);
		playerStats.onTopOfLadder = checkLadder;
	}

	void CheckLeavingLadder() {
		if (!CheckTouchingLadder()) {
			EnableStairsClimb(false);
		}
	}

	void CheckDescending() {
		if (playerStats.onTopOfLadder && !playerStats.isOnLadder &&
			(Input.GetAxis("Vertical")< 0f)) {

			playerStats.ladderFromTop = true;
			playerStats.isControllable = false;
			playerRigidbody.gravityScale = 0f;
			AlingWithLadder();
			Invoke("DescendingRepositioning", 0.2f);
		}
	}

	void DescendingRepositioning() {
		transform.position = new Vector3(
			transform.position.x,
			transform.position.y - 0.5f,
			transform.position.z);
		playerStats.isControllable = true;
		EnableStairsClimb(true);
	}
}