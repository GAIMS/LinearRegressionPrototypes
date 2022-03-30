using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour {
	
	public PlayerController controller;
	
	public TextMeshProUGUI score, timesHit;
	
	private void Awake() {
		if (this.controller.IsAI) {
			this.gameObject.SetActive(false);
		}
	}
	
	public void UpdateScoreCounter() {
		this.score.SetText(this.controller.gameplay.Score.ToString());
	}
	
	public void UpdateTimesHitCounter() {
		this.timesHit.SetText(this.controller.gameplay.TimesHit.ToString());
	}
}
