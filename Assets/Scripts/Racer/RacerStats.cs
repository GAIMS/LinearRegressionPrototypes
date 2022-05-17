using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerStats : MonoBehaviour {
	
	public bool RandomizeOnStart = true;
	
	[Range(0.1f, 1f)]
	public float Acceleration = 1f;
	
	[Range(0.1f, 1f)]
	public float Speed = 1f;
	
	[Range(0.1f, 1f)]
	public float Stamina = 1f;
	
	[Range(0.1f, 1f)]
	public float Recharge = 1f;
	
	[Range(0.1f, 1f)]
	public float Fly = 1f;
	
	[Range(0.1f, 1f)]
	public float Swim = 1f;
	
	[Range(0.1f, 1f)]
	public float Climb = 1f;
	
	[Range(0.1f, 1f)]
	public float Laziness = 1f;	
	
	private void Start() {
		if (this.RandomizeOnStart) {
			this.RandomizeStats();
		}
	}
	
	public void RandomizeStats() {
		float min = GameManager.Instance.minStat;
		float max = GameManager.Instance.maxStat;
		
		this.Acceleration = UnityEngine.Random.Range(min, max);
		this.Speed = UnityEngine.Random.Range(min, max);
		this.Stamina = UnityEngine.Random.Range(min, max);
		this.Recharge = UnityEngine.Random.Range(min, max);
		this.Fly = UnityEngine.Random.Range(min, max);
		this.Swim = UnityEngine.Random.Range(min, max);
		this.Climb = UnityEngine.Random.Range(min, max);
		this.Laziness = UnityEngine.Random.Range(min, max);
	}
	
}
