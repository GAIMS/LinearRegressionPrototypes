using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GameplayUI : MonoBehaviour {
	
	private static GameplayUI _Instance;
	public static GameplayUI Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<GameplayUI>();
			}
			return _Instance;
		}
	}
	
    public RaceUI[] raceUI;
	
	public Slider playerNumSlider;
	
	public TextMeshProUGUI playerCountSlider;
	
	public Slider raceSegmentSlider;
	
	public TextMeshProUGUI raceCourseSegments;
	
	public Slider maxStatSlider;
	
	public TextMeshProUGUI maxStat;
	
	public Slider minStatSlider;
	
	public TextMeshProUGUI minStat;
	
	private void Awake() {
	}	
	
	private void Start() {
		this.SetUI();
		
		this.playerNumSlider.value = GameManager.Instance.numberOfRacers;
		this.playerCountSlider.SetText(this.playerNumSlider.value.ToString());
		
		this.raceSegmentSlider.value = RaceTrackGenerator.Instance.RaceSegments;
		this.raceCourseSegments.SetText(this.raceSegmentSlider.value.ToString());
		
		this.maxStatSlider.value = GameManager.Instance.maxStat;
		this.maxStat.SetText(this.maxStatSlider.value.ToString());
		
		this.minStatSlider.value = GameManager.Instance.minStat;
		this.minStat.SetText(this.minStatSlider.value.ToString());
	}
	
	public void SetUI() {
		if (this.raceUI != null) {
			for (int i = 0; i < this.raceUI.Length; i++) {
				this.raceUI[i] = null;
			}
		}
		
		this.raceUI = this.GetComponentsInChildren<RaceUI>(true);
		
		for (int i = 0; i < this.raceUI.Length; i++) {
			this.raceUI[i].SetRenderTexture(GameManager.Instance.renderTextures[i]);
			
			int nameRand = UnityEngine.Random.Range(0, GameManager.Instance.names.Length);
			this.raceUI[i].SetName(GameManager.Instance.names[nameRand]);
			
			bool flag = i < GameManager.Instance.numberOfRacers;
			this.raceUI[i].gameObject.SetActive(flag);
		}
	}
	
	public void ReorderList() {
		List<RaceUI> raceUI = new List<RaceUI>{};
		for (int i = 0; i < this.raceUI.Length; i++) {
			raceUI.Add(this.raceUI[i]);
		}
		
		raceUI = raceUI.OrderBy(
			x => x.racerInfo.placement
		).ToList();
		for (int i = 0; i < raceUI.Count; i++) {
			raceUI[i].transform.SetAsLastSibling();
		}
	}
}
