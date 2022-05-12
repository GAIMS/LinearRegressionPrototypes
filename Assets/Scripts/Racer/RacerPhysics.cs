using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerPhysics : MonoBehaviour {
	
	private RacerCore core;
	
	private Rigidbody rigidbody;
	
	public Rigidbody Rigidbody {
		get {
			return this.rigidbody;
		}
	}
	
	private bool grounded;
	
	public bool Grounded {
		get {
			return this.grounded;
		} set {
			this.grounded = value;
		}
	}
	
	private const float BASE_SPEED = 5f;
	
	private void Awake() {
		this.core = this.GetComponent<RacerCore>();
		
		this.rigidbody = this.GetComponent<Rigidbody>();
	}
	
	private void Update() {
		
	}
	
	private void FixedUpdate() {
		
	}
	
	public float GetVerticalVelocity() {
		Vector3 velocity = this.rigidbody.velocity;
		return velocity.y;
	}
	
	public float GetHorizontalVelocity() {
		Vector3 velocity = this.rigidbody.velocity;
		velocity.y = 0f;
		return velocity.magnitude;
	}
	
	public Vector3 GetVelocity() {
		return this.rigidbody.velocity;
	}
}
