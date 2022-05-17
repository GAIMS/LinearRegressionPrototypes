using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacerCore : MonoBehaviour {
	
	[NonSerialized]
	public RacerPhysics physics;
	
	[NonSerialized]
	public RacerSkin skin;
	
	[NonSerialized]
	public RacerStats stats;
	
	[SerializeField]
	private Animator rankAnimator;
	
	[SerializeField]
	private SpriteRenderer rank;
	
	private void Awake() {
		this.physics = this.GetComponent<RacerPhysics>();
		this.skin = this.GetComponent<RacerSkin>();
		this.stats = this.GetComponent<RacerStats>();
	}
	
	public void SetRank(int rank) {
		if (!this.physics.canAct) {
			return;
		}
		if (this.rank.sprite != GameManager.Instance.ranks[rank]) {
			this.rankAnimator.Play("Switch", -1, 0f);
		}
		this.rank.sprite = GameManager.Instance.ranks[rank];
	}
}
