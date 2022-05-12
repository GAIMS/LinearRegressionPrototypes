using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerSkin : MonoBehaviour {
	
	private RacerCore core;
	
	
    public Transform rootObj;
	
	public Animator animator;
	
	public int motionLayer;
	
	public void PlayAnimation(string animation) {
		int anim = Animator.StringToHash(animation);
		this.animator.Play(anim, this.motionLayer, 0f);
	}
	
	private void Update() {
		this.HandleRotation();
	}
	
	private void HandleRotation() {
		Vector3 velocity = this.core.physics.GetVelocity();
		velocity.y = 0f;
		if (velocity.magnitude <= 0f) {
			return;
		}
		this.rootObj.forward = velocity;
	}
}
