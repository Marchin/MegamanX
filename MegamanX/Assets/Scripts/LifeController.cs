using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {
	[SerializeField] Stats stats;
	int currentHealth;
	void Start() {
		currentHealth = stats.maxHealth;
	}

	void Update() {

	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			Eliminate();
		}
	}

	protected virtual void Eliminate() {
		Destroy(this.gameObject);
	}
}