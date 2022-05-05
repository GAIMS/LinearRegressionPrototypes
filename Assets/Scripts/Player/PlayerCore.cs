using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {
	
	private Rigidbody rigidbody;
	
	private Animator animator;
	
	[SerializeField]
	private float acceleration = 10f;
	
	[SerializeField]
	private float deceleration = 10f;
	
	private const float MAX_MOVEMENT_SPEED = 30f;
	
	[SerializeField]
	public bool isAI = false;
	
	private bool aiMoveLeft, aiMoveRight, aiMoveUp, aiMoveDown;
	
	private float aiMovementTimer = 0f;
	
	private bool movingLeft, movingRight, movingUp, movingDown;
	
	public int team;
	
	private void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
	}
	
	private void Start() {
		
	}
	
	private void Update() {
		this.HandleMovementInput();
	}
	
	private void FixedUpdate() {
		this.HandleMovement();
		this.HandlePosition();
	}
	
	private void LateUpdate() {
		
	} 
	
	private void HandleMovementInput() {
		if (this.isAI) {
			if (this.aiMovementTimer <= 0f) {
				this.movingLeft = this.movingRight = this.movingUp = this.movingDown = this.aiMoveLeft = this.aiMoveRight = this.aiMoveUp = this.aiMoveDown = false;
				this.aiMovementTimer = UnityEngine.Random.Range(0.1f, 1f);
				int randHori = UnityEngine.Random.Range(1, 100);
				int randVert = UnityEngine.Random.Range(1, 100);
				this.aiMoveLeft = randHori >= 70;
				this.aiMoveRight = randHori < 70 && randHori >= 40;
				this.aiMoveUp = randVert >= 70;
				this.aiMoveDown = randVert < 70 && randVert >= 40;
			}	
			this.aiMovementTimer -= Time.deltaTime;
		}
		this.movingLeft = ((!this.isAI) ? Input.GetKey(KeyCode.A) : this.aiMoveLeft);
		this.movingRight = ((!this.isAI) ? Input.GetKey(KeyCode.D) : this.aiMoveRight);
		this.movingUp = ((!this.isAI) ? Input.GetKey(KeyCode.W) : this.aiMoveUp);
		this.movingDown = ((!this.isAI) ? Input.GetKey(KeyCode.S) : this.aiMoveDown);
	}
	
	private void HandleMovement() {
		
		Vector3 targetDir = new Vector3(
			((this.movingLeft) ? -1f : (this.movingRight) ? 1f : 0f),
			0f,
			((this.movingUp) ? 1f : (this.movingDown) ? -1f : 0f)
			);
		bool flag = targetDir.magnitude > 0f;
		
		if (flag) {
			this.rigidbody.velocity = this.rigidbody.velocity + (targetDir * (this.acceleration * Time.fixedDeltaTime));
			this.rigidbody.velocity = Vector3.ClampMagnitude(this.rigidbody.velocity, MAX_MOVEMENT_SPEED);
		} else {
			if (this.GetVelocityMagnitude() < 0.2f) {
				this.rigidbody.velocity = Vector3.zero;
			} else {
				this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity, Vector3.zero, this.deceleration * Time.fixedDeltaTime);
			}
		}
	}
	
	private void HandlePosition() {
		Transform playerSpace = ((this.team == 1) ? GameManager.Instance.playerSpace1 : GameManager.Instance.playerSpace2);
		
		float xRange = playerSpace.transform.localScale.x * 4.75f;
		float zRange = playerSpace.transform.localScale.z * 4.75f;
			
		
		Vector3 pos = this.transform.position;
		pos.x = Mathf.Clamp(pos.x, playerSpace.position.x - xRange, playerSpace.position.x + xRange);
		pos.z = Mathf.Clamp(pos.z, playerSpace.position.z - zRange, playerSpace.position.z + zRange);
		this.transform.position = pos;
	}
	
	public Vector3 GetVelocity() {
		return this.rigidbody.velocity;
	}
	
	public float GetVelocityMagnitude() {
		return this.rigidbody.velocity.magnitude;
	}
}
