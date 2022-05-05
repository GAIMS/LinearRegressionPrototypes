using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamLine : MonoBehaviour {	
	
	[Range(1, 2)]
	public int team = 1;
	
	public Vector3 a, b;
	
	private float timer = 0f;
	
	private void Awake() {
		
	}
	
	private void Start() {
		
	}
	
	private void Update() {
		if (GameManager.Instance.state == GameManager.State.Results) {
			return;
		}
		
		this.GetAveragePositions();		
		Vector3 pos = (this.a - this.b) / 2f;
		
		Vector3 targetDir = this.a - this.b;
		this.transform.forward = targetDir;
		
		float dist = Vector3.Distance(this.a, this.b);
		
		dist = Mathf.Clamp(dist - 1.25f, 2f, 999f);
		
		if (GameManager.Instance.state != GameManager.State.Game) {
			return;
		}
		if (GameManager.Instance.IsLineMatching(this.transform.forward)) {
			if (GameManager.Instance.holdPositionToWin) {
				this.timer += Time.deltaTime;
				if (this.timer < 1f) {
					return;
				}
			}			
			GameManager.Instance.StartCoroutine(GameManager.Instance.Results(this.team));
		} else {
			this.timer = 0f;
		}
	}
	
	public void GetAveragePositions() {
		List<PlayerCore> players = ((this.team == 1) ? GameManager.Instance.team1 : GameManager.Instance.team2);
		Vector3 avgA = Vector3.zero;
		Vector3 avgB = Vector3.zero;		
		
		for (int i = 0; i < players.Count; i++) {
			if (i < players.Count / 2f) {
				avgA += players[i].transform.position;
			} else {
				avgB += players[i].transform.position;
			}
		}
		
		this.a = avgA / (players.Count / 2f);
		this.b = avgB / (players.Count / 2f);		
	}
}
