using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerSkin : MonoBehaviour {
	
	private RacerCore core;

    public Transform skin;
	
	public Animator animator;
	
	public int motionLayer;
	
	private void Awake() {
		this.core = this.GetComponent<RacerCore>();
	}
	
	public void PlayAnimation(string animation) {
		int anim = Animator.StringToHash(animation);
		this.animator.Play(anim, this.motionLayer, 0f);
	}
	
	private void Update() {
		this.HandleRotation();
		
		this.animator.SetFloat("Horizontal Velocity", this.core.physics.GetHorizontalVelocity());
	}
	
	private void HandleRotation() {
		Vector3 velocity = this.core.physics.GetVelocity();
		velocity.y = 0f;
		if (velocity.magnitude <= 0.2f) {
			return;
		}
		this.skin.forward = velocity;
	}
}
