using UnityEngine;

public class PlayerLadderController : MonoBehaviour {
    [SerializeField] PlayerStats playerStats;
    [SerializeField] LayerMask ladderLayer;
    Rigidbody2D playerRigidbody;
    public float climbSpeed = 2f;
    public float ladderOffset = 0.5f;
    const float fromTopAnimation = 0.2f;
    int numLevelLayer;
    int numPlayerLayer;


    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStats.isOnLadder = false;
        playerStats.onTopOfLadder = false;
        playerStats.hasLadderAbove = false;
        numLevelLayer = LayerMask.NameToLayer("Level");
        numPlayerLayer = LayerMask.NameToLayer("Player");
    }

    void LateUpdate() {
        OnFrontOfLadder();
        if (playerStats.isOnLadder) {
            CheckJumpingOut();
            Climbing();
            CheckFinishedClimbing();
            CheckFinishedDescending();
        } else {
            CheckDescendingFromTop();
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (Input.GetAxis("Vertical") > 0f && LadderAbove(0f) &&
            other.CompareTag("Ladders")) {

            EnableStairsClimb(true);
        }
    }

    void AlingWithLadder() {
        Vector3 stairsCenter = new Vector3(
            Mathf.Floor(transform.position.x) + 0.5f,
            transform.position.y,
            transform.position.z);
        transform.position = stairsCenter;
    }

    void EnableStairsClimb(bool newState) {
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.gravityScale = (newState ? 0f : 1f);
        Physics2D.IgnoreLayerCollision(numPlayerLayer, numLevelLayer, newState);
        playerStats.isOnLadder = newState;
        if (newState) {
            AlingWithLadder();
        }
    }

    void CheckJumpingOut() {
        if (Input.GetButtonDown("Jump")) {
            EnableStairsClimb(false);
        }
    }

    void Climbing() {
        if (Input.GetAxis("Vertical") != 0f && playerStats.isControllable) {
            playerRigidbody.velocity = (new Vector2(0f, climbSpeed)) *
                (Input.GetAxis("Vertical") >= 0f ? 1f : -1f);
        } else {
            playerRigidbody.velocity = Vector2.zero;
        }
    }

    void OnFrontOfLadder() {
        Vector3 topOfPlayer = transform.position + new Vector3(
            0f, ladderOffset, 0);
        RaycastHit2D checkLadder = Physics2D.Raycast(topOfPlayer,
            Vector2.down, ladderOffset * 2, ladderLayer);
        playerStats.onTopOfLadder = checkLadder;
    }

    void CheckDescendingFromTop() {
        if (LadderBelow(-ladderOffset) && (Input.GetAxis("Vertical") < 0f) &&
            !LadderAbove(0f) && playerStats.isControllable) {

            playerStats.ladderFromTop = true;
            playerStats.isControllable = false;

            EnableStairsClimb(true);
            Invoke("DescendingRepositioning", 0.2f);
        }
    }

    void DescendingRepositioning() {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y - 1f,
            transform.position.z);
        playerStats.isControllable = true;
    }

    void CheckFinishedClimbing() {
        playerStats.hasLadderAbove = LadderAbove(0f);
        if (!playerStats.hasLadderAbove && Input.GetAxis("Vertical") > 0f) {

            EnableStairsClimb(false);
            Vector3 MoveOnTop = new Vector3(0f, 0.3f, 0f);
            transform.position += MoveOnTop;
        }
    }

    void CheckFinishedDescending() {
        if (!LadderBelow(-ladderOffset/2) && LadderAbove(ladderOffset) &&
            (Input.GetAxis("Vertical") < 0f || playerStats.isGrounded)) {

            EnableStairsClimb(false);
        }
    }

    bool LadderAbove(float offsetMiddle) {
        Vector3 vOffset = new Vector3(0f, offsetMiddle, 0f);
        return Physics2D.Raycast((transform.position + vOffset), Vector2.up, 0f, ladderLayer);
    }

    bool LadderBelow(float offsetMiddle) {
        Vector3 vOffset = new Vector3(0f, offsetMiddle, 0f);
        return Physics2D.Raycast((transform.position + vOffset), Vector2.down, 0f, ladderLayer);
    }
}