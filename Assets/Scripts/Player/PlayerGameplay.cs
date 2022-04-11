using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplay : MonoBehaviour {
	
	private PlayerCore core;
	
	public int initialHealth = 3;
	
	[SerializeField]
	public int health { get; private set; }
	
	[SerializeField]
	private float dmgTimer = 0f;
	
	public const float DAMAGE_HALT_TIME = 0.75f;
	
	private void Awake() {
		this.core = this.GetComponent<PlayerCore>();
	}
	
	private void Start() {
		this.health = this.initialHealth;
	}
	
	private void Update() {
		this.HandleDamageTimer();
	}
	
	public bool IsDamaged() {
		return this.dmgTimer > 0f;
	}
	
	private void HandleDamageTimer() {
		if (this.dmgTimer <= 0f) {
			return;
		} else if (this.dmgTimer > 0f) {
			this.dmgTimer -= Time.deltaTime;
		}
	}
	
	public void TakeDamage() {
		this.health--;
		this.dmgTimer = DAMAGE_HALT_TIME;
		
		if (this.health <= 0) {
			SceneController.ReloadCurrentScene();
		}
	}
}
