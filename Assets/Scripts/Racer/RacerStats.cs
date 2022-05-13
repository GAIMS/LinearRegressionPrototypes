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
	
//	public float Fly = 1f;
	
//	public float Swim = 1f;
	
//	public float Climb = 1f;
	
//	public float Laziness = 1f;	
	
	private void Start() {
		if (this.RandomizeOnStart) {
			this.RandomizeStats();
		}
	}
	
	public void RandomizeStats() {
		this.Acceleration = UnityEngine.Random.Range(0.1f, 1f);
		this.Speed = UnityEngine.Random.Range(0.1f, 1f);
		this.Stamina = UnityEngine.Random.Range(0.1f, 1f);
		this.Recharge = UnityEngine.Random.Range(0.1f, 1f);
	}
	
}
