using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerCore : MonoBehaviour {
	
	[NonSerialized]
	public RacerPhysics physics;
	
	[NonSerialized]
	public RacerSkin skin;
	
	[NonSerialized]
	public RacerStats stats;
	
	private void Awake() {
		this.physics = this.GetComponent<RacerPhysics>();
		this.skin = this.GetComponent<RacerSkin>();
		this.stats = this.GetComponent<RacerStats>();
	}	
}
