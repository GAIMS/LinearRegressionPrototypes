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
	
	public SpriteRenderer Rank {
		get {
			return this.rank;
		}
	}
	
	public Transform staminaBar;
	
	public Camera previewCamera;
	
	public float minutes = 0f;
	
	public float seconds = 0f;
	
	public float milliseconds = 0f;
	
	public bool timerEnabled = false;
	
	private void Awake() {
		this.physics = this.GetComponent<RacerPhysics>();
		this.skin = this.GetComponent<RacerSkin>();
		this.stats = this.GetComponent<RacerStats>();
	}
	
	private void Update() {
		this.HandleStaminaBar();
		this.HandleTime();
	}
	
	private void HandleTime() {
		if (!this.timerEnabled) {
			return;
		}
		this.milliseconds += Mathf.Round(Time.deltaTime * 1000f);
		if (this.milliseconds >= 1000f) {
			if (this.seconds >= 59f && this.minutes < 99f) {
				this.minutes++;
				this.seconds = 0f;
			} else if (this.seconds < 59f) {
				this.seconds++;
			}
			this.milliseconds = 0f;
		} else if (this.minutes == 99 && this.seconds == 59 && this.milliseconds >= 990f) {
			this.minutes = 99f;
			this.seconds = 59f;
			this.milliseconds = 999f;
			this.timerEnabled = false;
		}
	}
	
	public void SetRank(int rank) {
		if (!this.physics.canAct) {
			return;
		}
		if (this.rank.sprite != GameManager.Instance.ranks[rank]) {
			this.rankAnimator.Play("Switch", -1, 0f);
			
			for (int i = 0; i < GameplayUI.Instance.raceUI.Length; i++) {
				if (GameplayUI.Instance.raceUI[i].racerInfo.image.texture == this.previewCamera.targetTexture) {
					if (FlyCamera.Instance.racerToFollow == this.gameObject.transform) {
						FlyCamera.Instance.SetRacer(rank);
					}
					GameplayUI.Instance.raceUI[i].SetRank(rank);
				}
			}			
		}
		this.rank.sprite = GameManager.Instance.ranks[rank];
	}
	
	public void HandleStaminaBar() {
		this.staminaBar.transform.localScale = new Vector3((this.physics.stamina / 100f) * 2f, 1f, 1f);
	}
	
	public void SetCameraRenderTexture(int index) {
		Debug.Log("Setting render texture");
		this.previewCamera.targetTexture = GameManager.Instance.renderTextures[index];
	}
	
	public void SetStats(int index) {
		float[] stats = new float[5] {
			this.stats.Speed,
			this.stats.Fly,
			this.stats.Swim,
			this.stats.Climb,
			this.stats.Acceleration
		};
		GameplayUI.Instance.raceUI[index].SetStats(stats);
	}
}
