using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
	[SerializeField] LayerMask contactLayer;
	[SerializeField] Stats stats;
	[SerializeField] float speed = 1f;
	[SerializeField] float damage = 1f;
	[SerializeField] float lifetime = 1f;

	void OnEnable() {
		Vector2 speedVector;
		Invoke("SelfDisable", lifetime);
		if (!stats.facingRight) {
			speedVector = new Vector2(-speed, 0f);
		} else {
			speedVector = new Vector2(speed, 0f);
		}
		GetComponent<Rigidbody2D>().velocity = speedVector;
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

	void SelfDisable() {
		gameObject.SetActive(false);
	}

	virtual public void MakeDamage() { }
}