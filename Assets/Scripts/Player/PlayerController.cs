using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	private Rigidbody rigidbody;
	
	private Transform transform;
	
	public Transform Transform {
		get {
			return this.transform;
		}
	}
	
	[SerializeField]
	private KeyCode up, left, down, right;
	
	private float speed = 10f;
	
	private void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
		
		this.transform = this.gameObject.transform;
	}
	
	private void Update() {
		
	}
	
	private void FixedUpdate() {
		bool up = Input.GetKey(this.up);
		bool down = Input.GetKey(this.down);
		
		bool left = Input.GetKey(this.left);
		bool right = Input.GetKey(this.right);
		
		float vert = 0f;
		float hori = 0f;
		
		vert += ((up) ? 1f : 0f) + ((down) ? -1f : 0f);
		hori += ((right) ? 1f : 0f) + ((left) ? -1f : 0f);
		
		Vector3 velocity = new Vector3(hori, vert, 0f) * this.speed;
		
		this.rigidbody.velocity = velocity;
	}
	
	private void OnCollisionEnter(Collision collision) {
		ObjProjectile projectile = collision.gameObject.GetComponentInChildren<ObjProjectile>();
		if (projectile == null) {
			return;
		} else {
			projectile.gameObject.SetActive(false);
			// Damage player
		}
	}
}
