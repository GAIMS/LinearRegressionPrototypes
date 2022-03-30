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
			this.controller.ui.UpdateScoreCounter();
		}
	}
	
	private int timesHit = 0;
	
	public int TimesHit {
		get {
			return this.timesHit;
		} set {
			this.timesHit = value;
			this.controller.ui.UpdateTimesHitCounter();
		}
	}
	
	private void Awake() {
		this.controller = this.GetComponent<PlayerController>();
	}
	
	private void Start() {
		this.TimesHit = 0;
		this.Score = 0;
	}
	
	public void AddScore(int num) {
		this.Score += num;
	}
	
	public void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.layer == 7) {
			this.TimesHit++;
		}
		
		if (collider.gameObject.layer == 8) {
			this.AddScore(1);
		}
		
		ObjObstacle obstacle = collider.GetComponentInParent<ObjObstacle>();
		if (obstacle != null) {
			obstacle.DisableColliders();
		}
	}
}
