using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyCore {
	
	private Transform target;
	
	protected override void Awake() {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start();
		
		int rand = UnityEngine.Random.Range(0, 2);
		this.target = ((rand > 0) ? PlayerObject.Instance.playerOne.transform : PlayerObject.Instance.playerTwo.transform);
	}
	
	protected override void Update() {
		this.HandleFireTimer();
	}
	
	protected override void FixedUpdate() {
		this.rigidbody.velocity = Vector3.zero;
	}
	
	protected override void LateUpdate() {
		this.HandleAim();
	}
	
	protected override void HandleAim() {
		this.transform.up = this.target.position - this.transform.position;
	}
	
	protected override void HandleFireTimer() {
		if (this.fireTimer > 0f) {
			this.fireTimer -= Time.deltaTime;
		} else if (this.fireTimer <= 0f) {
			this.Fire();
			this.fireTimer += this.fireRate;
		}
	}
	
	protected override void Fire() {
		base.Fire();
	}	
}
