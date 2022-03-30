using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplay : MonoBehaviour {
	
	private PlayerController controller;
	
    private int score = 0;
	
	public int Score {
		get {
			return this.score;
		} set {
			this.score = value;
		}
	}
	
	private void Awake() {
		this.controller = this.GetComponent<PlayerController>();
	}
	
	public void AddScore(int num) {
		this.Score += num;
	}
}
