using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour {
	
	private PlayerController controller;
	
    private Transform transform;
	
	private PlayerMovement movement;
	
	[SerializeField]
	private Transform horiTransform, vertTransform;
	
	private void Awake() {
		this.transform = this.gameObject.transform;
		
		this.controller = this.GetComponentInParent<PlayerController>();
		this.movement = this.GetComponentInParent<PlayerMovement>();
	}
	
	private void Update() {
		
		Vector3 playerPosX = this.movement.transform.position;
		playerPosX.y = 0f;
		playerPosX.z = 0f;
		
		Vector3 playerPosY = this.movement.transform.position;
		playerPosY.x = 0f;
		playerPosY.z = 0f;
		
		Vector3 targetPosX = ObstacleManager.Instance.TargetTransform.position;
		targetPosX.y = 0f;
		targetPosX.z = 0f;
		
		Vector3 targetPosY = ObstacleManager.Instance.TargetTransform.position;
		targetPosY.x = 0f;
		targetPosY.z = 0f;
		
		float xDistance = Vector3.Distance(playerPosX, targetPosX);
		float yDistance = Vector3.Distance(playerPosY, targetPosY);
		
		Vector3 horiRot = Vector3.one * (xDistance * (xDistance * 2f));
		horiRot.x = 0f;
		horiRot.y = 0f;
		
		Vector3 vertRot = Vector3.one * (yDistance * (yDistance * 2f));
		vertRot.x = 0f;
		vertRot.y = 0f;
		
		this.horiTransform.localEulerAngles = horiRot;
		this.vertTransform.localEulerAngles = vertRot;
		
		
	}
}
