using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	private PlayerController controller;
	
	[SerializeField]
	private KeyCode up, down, left, right;
	
	private Rigidbody rigidbody;
	
	[SerializeField]
	private float speed = 2.5f;
	
	private void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
		
		this.controller = this.GetComponent<PlayerController>();
	}
	
	private void FixedUpdate() {
		bool isUp = false;
		bool isDown = false;
		bool isLeft = false;
		bool isRight = false;
		
		if (this.controller.IsAI) {
			isUp = this.controller.ai.Up;
			isDown = this.controller.ai.Down;
			isLeft = this.controller.ai.Left;
			isRight = this.controller.ai.Right;
			
		} else {
			isUp = Input.GetKey(this.up);
			isDown = Input.GetKey(this.down);
			isLeft = Input.GetKey(this.left);
			isRight = Input.GetKey(this.right);
		}
						
		float h = ((isLeft) ? -1f : (isRight) ? 1f : 0);
		float v = ((isUp) ? 1f : (isDown) ? -1f : 0);
		
		Vector3 dir = new Vector3(h, v, 0f);
		
		dir *= this.speed;
		
		this.rigidbody.velocity = dir;
	}
}
