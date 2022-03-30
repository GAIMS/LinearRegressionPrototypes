using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
    [NonSerialized]
	public PlayerMovement movement;
	
	[NonSerialized]
	public PlayerGameplay gameplay;
	
	[NonSerialized]
	public PlayerLine line;
	
	[NonSerialized]
	public PlayerAI ai;
	
	[SerializeField]
	private bool isAI = false;
	
	public bool IsAI {
		get {
			return this.isAI;
		} set {
			this.isAI = value;
		}
	}
	
	public Collider collider;
	
	private void Awake() {
		this.movement = this.GetComponent<PlayerMovement>();
		this.gameplay = this.GetComponent<PlayerGameplay>();
		this.ai = this.GetComponent<PlayerAI>();
		
		this.line = this.GetComponentInChildren<PlayerLine>();
	}
}
