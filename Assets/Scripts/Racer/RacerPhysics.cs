using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerPhysics : MonoBehaviour {
	
	private const float CAST_DISTANCE = 0.5f;
	
	public const float CAST_OFFSET = 0.4f;
	
	public const float GRAVITY = -48f * 0.1f;
	
	private RacerCore core;
	
	private Rigidbody rigidbody;
	
	public Rigidbody Rigidbody {
		get {
			return this.rigidbody;
		}
	}
	
	public LayerMask surfaceMask;
	
	public LayerMask waterMask;
	
	[SerializeField]
	private bool grounded;
	
	[SerializeField]
	private bool prevGrounded;
	
	[SerializeField]
	private bool inWater;
	
	[SerializeField]
	private bool detectWall;
	
	private Vector3 gravity = Vector3.up;
	
	private Vector3 groundNormal = Vector3.up;
	
	private RaycastHit hitInfo;
	
	private RaycastHit wallHitInfo;
	
	private GameObject groundObject;
	
	public bool Grounded {
		get {
			return this.grounded;
		} set {
			this.grounded = value;
		}
	}
	
	public bool PrevGrounded {
		get {
			return this.prevGrounded;
		}
	}
	
	public bool InWater {
		get {
			return this.inWater;
		} set {
			this.inWater = value;
		}
	}
	
	public bool DetectWall {
		get {
			return this.detectWall;
		} set {
			this.detectWall = value;
		}
	}
	
	public Vector3 Gravity {
		get {
			return this.gravity;
		} set {
			this.gravity = value;
		}
	}
	
	public Vector3 GroundNormal {
		get {
			return this.groundNormal;
		} set {
			this.groundNormal = value;
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
		this.HandleGroundDetection();
		this.HandleWallDetection();
		this.HandleGround();
		this.HandleAir();
		this.HandleWater();
		this.HandleWall();
	}
	
	private void HandleGround() {
		if (!this.grounded || this.inWater || this.detectWall) {
			return;
		}
		
		Debug.Log("Doing Ground");
		Vector3 velocity = this.rigidbody.velocity;
		Vector3 normalized = Math3d.ProjectVectorOnPlane(this.groundNormal, velocity).normalized;
		
		if (this.recharging || !this.canAct) {
			if (velocity.magnitude > 0f) {
				velocity = Vector3.Lerp(velocity, Vector3.zero, BASE_DECELERATION * Time.fixedDeltaTime);
			} else {
				velocity = Vector3.zero;
			}
			normalized = velocity;
		} else {
			normalized += this.transform.right * (this.GetAcceleration() * Time.fixedDeltaTime) * 50f;
			normalized = Vector3.ClampMagnitude(normalized, this.GetTopSpeed());
		}
		this.rigidbody.velocity = normalized;
	}
	
	private void HandleGroundDetection() {
		if ((this.grounded || Vector3.Dot(this.rigidbody.velocity, this.gravity) <= 0f) && this.GroundCast(out this.hitInfo)) {
			if (!this.grounded) {
				this.rigidbody.velocity = Math3d.ProjectVectorOnPlane(this.groundNormal, this.rigidbody.velocity);
			} else {
				this.rigidbody.velocity = Quaternion.FromToRotation(this.groundNormal, this.hitInfo.normal) * this.rigidbody.velocity;
			}
			if (!this.detectWall) {
				this.transform.position = this.hitInfo.point;
			}
			this.prevGrounded = this.grounded;
			this.grounded = true;
			this.groundNormal = this.hitInfo.normal;
			this.groundObject = this.hitInfo.transform.gameObject;
		} else {
			this.prevGrounded = this.grounded;
			this.grounded = false;
			this.groundNormal = this.gravity;
		}
	}
	
	private void HandleAir() {
		if (this.grounded || this.inWater || this.detectWall) {
			return;
		}
		
		Debug.Log("Doing Air");
		
		Vector3 velocity = this.rigidbody.velocity;
		Vector3 hori = velocity;
		Vector3 vert = velocity;
		hori.y = 0f;
		vert.x = 0f;
		vert.z = 0f;
		vert += GRAVITY * this.gravity * Time.fixedDeltaTime;
		hori += this.transform.right * (this.GetAcceleration() * Time.fixedDeltaTime);
		hori = Vector3.ClampMagnitude(hori, this.GetFlySpeed());
		velocity = hori + vert;
		this.rigidbody.velocity = velocity;
	}
	
	private void HandleWater() {
		if (this.grounded || this.detectWall || !this.inWater) {
			return;
		}
		
		Debug.Log("Doing Water");
		Vector3 velocity = this.rigidbody.velocity;
		velocity += this.transform.right * (this.GetAcceleration() * Time.fixedDeltaTime);
		velocity.y = 0f;
		velocity = Vector3.ClampMagnitude(velocity, this.GetSwimSpeed());
		this.rigidbody.velocity = velocity;
		
	}
	
	private void HandleWall() {
		if (!this.detectWall ) {
			return;
		}		
		
		Debug.Log("Doing Wall");
		Vector3 velocity = Vector3.zero;
		velocity = Vector3.up * this.GetClimbSpeed();
		this.rigidbody.velocity = velocity;
	}
	
	private void HandleWallDetection() {
		this.detectWall = this.WallCast();
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
	
	public float GetFlySpeed() {
		return BASE_SPEED * 1.5f * this.core.stats.Fly;
	}
	
	public float GetSwimSpeed() {
		return BASE_SPEED * 0.75f * this.core.stats.Swim;
	}
	
	public float GetClimbSpeed() {
		return BASE_SPEED * 0.5f * this.core.stats.Climb;
	}
	
	public bool GroundCast(out RaycastHit hit) {
		return this.GroundCast(this.transform.position + this.gravity * CAST_OFFSET, out hit);
	}
	
	public bool GroundCast(Vector3 origin, out RaycastHit hit) {
		return Physics.Raycast(origin, -this.gravity, out hit, CAST_DISTANCE, this.surfaceMask, QueryTriggerInteraction.Ignore);
	}
	
	public bool WallCast() {
		return Physics.Raycast(this.transform.position + (Vector3.up * 0.25f), this.transform.right, out this.wallHitInfo, 0.3f, this.surfaceMask, QueryTriggerInteraction.Ignore);
	}
	
	public void OnTriggerEnter(Collider collider) {
		ObjGoal goal = collider.GetComponent<ObjGoal>();
		if (goal != null) {
			this.canAct = false;
		}
		
		bool water = collider.gameObject.layer == 4;
		if (water) {
			this.inWater = true;
		}
	}
	
	public void OnTriggerExit(Collider collider) {
		bool water = collider.gameObject.layer == 4;
		if (water) {
			this.inWater = false;
			this.rigidbody.velocity = Vector3.zero;
		}
	}
}
