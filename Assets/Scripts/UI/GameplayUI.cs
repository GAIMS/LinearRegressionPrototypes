using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	
	[NonSerialized]
    public RaceUI[] raceUI;
	
	private void Awake() {
		this.raceUI = this.GetComponentsInChildren<RaceUI>();
	}	
	
	private void Start() {
		this.SetUI();
	}
	
	public void SetUI() {
		for (int i = 0; i < this.raceUI.Length; i++) {
			this.raceUI[i].SetRenderTexture(GameManager.Instance.renderTextures[i]);
			
			int nameRand = UnityEngine.Random.Range(0, GameManager.Instance.names.Length);
			this.raceUI[i].SetName(GameManager.Instance.names[nameRand]);
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
