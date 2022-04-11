using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	private PlayerCore core;
	
	private Rigidbody rigidbody;
	
	public Rigidbody Rigidbody {
		get {
			return this.rigidbody;
		}
	}
	
	private Transform transform;
	
	public Transform Transform {
		get {
			return this.transform;
		}
	}
	
	[SerializeField]
	private KeyCode up, left, down, right;
	
	private float speed = 10f;
	
	public bool isNotHuman = false;
	
	private void Awake() {
		this.core = this.GetComponent<PlayerCore>();
		
		this.rigidbody = this.GetComponent<Rigidbody>();
		
		this.transform = this.gameObject.transform;
	}
	
	private void Update() {
		
	}
	
	private void FixedUpdate() {
		
		if (this.isNotHuman) {
			if (this.core.gameplay.IsDamaged()) {
				this.rigidbody.velocity = Vector3.zero;
			} else {
				if (PlayerObject.Instance.wall.playerOne == this) {
					this.rigidbody.velocity = PlayerObject.Instance.wall.playerTwo.Rigidbody.velocity * -1f;
				} else if (PlayerObject.Instance.wall.playerTwo == this) {
					this.rigidbody.velocity = PlayerObject.Instance.wall.playerOne.Rigidbody.velocity * -1f;
				}
			}
		} else {
			bool up = Input.GetKey(this.up);
			bool down = Input.GetKey(this.down);
			
			bool left = Input.GetKey(this.left);
			bool right = Input.GetKey(this.right);
			
			float vert = 0f;
			float hori = 0f;
			
			vert += ((up) ? 1f : 0f) + ((down) ? -1f : 0f);
			hori += ((right) ? 1f : 0f) + ((left) ? -1f : 0f);
			
			Vector3 velocity = new Vector3(hori, vert, 0f) * (this.speed * ((this.core.gameplay.IsDamaged()) ? 0f : 1f));
			
			this.rigidbody.velocity = velocity;
		}
		
		Vector3 v = this.transform.position - Vector3.zero;
		v = Vector3.ClampMagnitude(v, LevelParams.Instance.Radius);
		v.z = 0f;
		this.transform.position = Vector3.zero + v;
	}
	
	private void OnTriggerEnter(Collider collider) {
		ObjProjectile projectile = collider.gameObject.GetComponentInChildren<ObjProjectile>();
		if (projectile == null) {
			return;
		} else {
			if (projectile.isPlayer) {
				return;
			}
			projectile.gameObject.SetActive(false);
			if (this.core.gameplay.IsDamaged()) {
				return;
			}
			this.core.gameplay.TakeDamage();
		}
	}
}
