using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamLine : MonoBehaviour {	
	
	[Range(1, 2)]
	public int team = 1;
	
	public Vector3 a, b;
	
	private void Awake() {
		
	}
	
	private void Start() {
		
	}
	
	private void Update() {
		this.GetAveragePositions();
		
		
		
		Vector3 pos = (this.a - this.b) / 2f;
		
	//	this.transform.position = pos + ((this.team == 1) ? GameManager.Instance.playerSpace1.position : GameManager.Instance.playerSpace2.position);
		
		Vector3 targetDir = this.a - this.b;
		this.transform.rotation = Quaternion.FromToRotation(Vector3.forward, targetDir);
		
		float dist = Vector3.Distance(this.a, this.b);
		
		dist = Mathf.Clamp(dist - 1.25f, 2f, 999f);
	//	this.transform.localScale = new Vector3(dist, 0.25f, 1f);
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
