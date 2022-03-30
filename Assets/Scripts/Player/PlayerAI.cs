using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour {
	
	private PlayerController controller;
	
	[SerializeField]
	private bool up, down, left, right;
	
	public bool Up {
		get {
			return this.up;
		} set {
			this.up = value;
		}
	}
	
	public bool Down {
		get {
			return this.down;
		} set {
			this.down = value;
		}
	}
	
	public bool Left {
		get {
			return this.left;
		} set {
			this.left = value;
		}
	}
	
	public bool Right {
		get {
			return this.right;
		} set {
			this.right = value;
		}
	}
	
	private Vector3 currentTargetPos, targetPos;
	
	private void Awake() {
		this.controller = this.GetComponent<PlayerController>();
	}
	
	private void FixedUpdate() {
		if (!this.controller.IsAI) {
			return;
		}
		
		if (ObstacleManager.Instance.TargetTransform != null) {
			Vector3 aiPos = this.controller.movement.transform.position;
			aiPos.z = 0f;
			
			bool flag = this.targetPos == ObstacleManager.Instance.TargetTransform.position;
			if (!flag) {
				this.targetPos = ObstacleManager.Instance.TargetTransform.position;
				
				this.currentTargetPos = ObstacleManager.Instance.TargetTransform.position + (Random.insideUnitSphere * 1.5f);
				this.currentTargetPos.z = 0f;
			}
			
			float dist = Vector3.Distance(aiPos, this.currentTargetPos);
			if (dist < 0.15f) {
				this.Up = false;
				this.Down = false;
				this.Left = false;
				this.Right = false;
				return;
			}
			
			this.Up = aiPos.y < this.currentTargetPos.y;
			this.Down = aiPos.y > this.currentTargetPos.y;
			
			this.Left = aiPos.x > this.currentTargetPos.x;
			this.Right = aiPos.x < this.currentTargetPos.x;
		}		
	}
}
