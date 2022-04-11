using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjProjectile : MonoBehaviour {
	
	public Rigidbody rigidbody { get; private set; }
	
	public bool isPlayer { get; private set; }
	
	private float timer = 0f;
	
	private float deathTime = 20f;
	
	private void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
	}
	
	private void Start() {
		this.isPlayer = false;
	}
	
	public void Reflect(bool player) {
		this.isPlayer = player;
		this.timer = 0f;
	}
	
	private void Update() {
		this.timer += Time.deltaTime;
		if (this.timer > this.deathTime) {
			Destroy(this.gameObject);
		}
	}
}
