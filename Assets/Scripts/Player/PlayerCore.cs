using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {
	
	public Animator animator { get; private set; }
	
	public Rigidbody rigidbody { get; private set; }
	
	public PlayerController controller { get; private set; }
	
	public PlayerGameplay gameplay { get; private set; }
	
	private void Awake() {
		// this.animator = this.GetComponentInChildren<Animator>();
		this.rigidbody = this.GetComponent<Rigidbody>();
		
		this.controller = this.GetComponent<PlayerController>();
		this.gameplay = this.GetComponent<PlayerGameplay>();
	}
}
