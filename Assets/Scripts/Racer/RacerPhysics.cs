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
	
	private const float BASE_SPEED = 10f;
	
	private const float BASE_ACCELERATION = 10f;
	
	private const float BASE_STAMINA = 100f;
	
	private const float BASE_RECHARGE = 10f;
	
	private const float BASE_DECELERATION = 2f;
	
	public bool canAct = true;
	
	public float stamina = 10;
	
	public bool recharging = false;
	
	private void Awake() {
		this.core = this.GetComponent<RacerCore>();
		
		this.rigidbody = this.GetComponent<Rigidbody>();
	}
	
	private void Start() {
		this.stamina = this.GetMaxStamina();
	}
	
	private void Update() {
		this.HandleStamina();
	}
	
	private void HandleStamina() {
		if (this.canAct) {
			if (!this.recharging) {
				if (this.stamina > 0f) {
					this.stamina -= Time.deltaTime;
				} else if (this.stamina <= 0f) {
					this.stamina = 0f;
					this.recharging = true;
				}
			} else {
				this.stamina = Mathf.Clamp(this.stamina + (this.GetRecharge() * Time.deltaTime), 0f, this.GetMaxStamina());
				if (this.stamina >= this.GetMaxStamina()) {
					this.recharging = false;
				}
			}
		}
	}				
	
	private void FixedUpdate() {
		this.HandleGround();
	}
	
	private void HandleGround() {
		
		Vector3 velocity = this.rigidbody.velocity;
		
		if (this.recharging || !this.canAct) {
			if (velocity.magnitude > 0f) {
				velocity = Vector3.Lerp(velocity, Vector3.zero, BASE_DECELERATION * Time.fixedDeltaTime);
			} else {
				velocity = Vector3.zero;
			}
		} else {
			velocity += this.core.skin.skin.forward * (this.GetAcceleration() * Time.fixedDeltaTime);
			velocity = Vector3.ClampMagnitude(velocity, this.GetTopSpeed());
		}
		
		this.rigidbody.velocity = velocity;
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
	
	public float GetTopSpeed() {
		return BASE_SPEED * this.core.stats.Speed;
	}
	
	public float GetAcceleration() {
		return BASE_ACCELERATION * this.core.stats.Acceleration;
	}
	
	public float GetMaxStamina() {
		return BASE_STAMINA * this.core.stats.Stamina;
	}
	
	public float GetRecharge() {
		return BASE_RECHARGE * this.core.stats.Recharge;
	}
	
	public void OnTriggerEnter(Collider collider) {
		ObjGoal goal = collider.GetComponent<ObjGoal>();
		if (goal == null) {
			return;
		}
		this.canAct = false;
	}
}
