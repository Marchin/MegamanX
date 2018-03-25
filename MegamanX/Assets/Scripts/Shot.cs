using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
	public LayerMask contactLayer;
	public Stats stats;
	public float speed = 1f;
	public float damage = 1f;

	void Start() {
		if (!stats.facingRight) {
			speed = -speed;
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
		StartCoroutine(SelfDestroy());
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (IsContactingTarget(other.gameObject)) {
			Debug.Log("YEAH");
			MakeDamage();
		}
	}

	bool IsContactingTarget(GameObject contactedObject) {
		return (((contactLayer >> contactedObject.layer)& 1)== 1);
	}

	IEnumerator SelfDestroy() {
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}

	virtual public void MakeDamage() { }
}