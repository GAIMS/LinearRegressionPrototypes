using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	private static GameManager _Instance;
	public static GameManager Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<GameManager>();
			}
			return _Instance;
		}
	}
	
	 public List<Race> races;
	
	[Range(0.1f, 3f)]
	public float minStat = 0.1f;
	
	[Range(0.1f, 3f)]
	public float maxStat = 1f;
	
	[Range(2, 12)]
	public int numberOfRacers = 2;
	
	private int currentRacers;
	
	public int CurrentRacers {
		get {
			return this.currentRacers;
		}
	}
	
	[SerializeField]
	public GameObject racerPrefab;
	
	public List<GameObject> racers;
	
	public Sprite[] ranks;
	
	public GameObject[] courses = new GameObject[5];
	
	public RenderTexture[] renderTextures;
	
	public string[] names;
	
	public float time = 0f;
	
	private void Start() {
		this.GenerateRacers();
	}
	
	private void GenerateRacers() {
		if (this.racers.Count > 0) {
			for (int i = 0; i < this.racers.Count; i++) {
				Destroy(this.racers[i]);
			}
			this.racers.Clear();
		}
		
		float posRange = this.numberOfRacers;
		Vector3 position = Vector3.zero;
		position.y = 0.55f;
		position.z = this.numberOfRacers / 2f;
		for (int i = 0; i < this.numberOfRacers; i++) {
			GameObject racer = Instantiate(this.racerPrefab, position, Quaternion.identity);
			position.z -= 1f;
			this.racers.Add(racer);
			RacerCore core = racer.GetComponent<RacerCore>();
			core.SetCameraRenderTexture((this.numberOfRacers - 1) - i);
			core.stats.RandomizeStats();
			core.SetStats((this.numberOfRacers - 1) - i);
		}
		
		this.currentRacers = this.numberOfRacers;
		
		int rand = UnityEngine.Random.Range(0, this.racers.Count);
		FlyCamera.Instance.racerToFollow = this.racers[rand].transform;
	}
	
	private void Update() {		
		this.TrackPositions();
	}
	
	private void TrackPositions() {
		
		this.racers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
		
		for (int i = 0; i < this.racers.Count; i++) {
			RacerCore core = this.racers[i].GetComponent<RacerCore>();
			core.SetRank((this.currentRacers - 1) - i);
		}
	}
	
	public void RestartRace() {
		GameplayUI.Instance.SetUI();
		this.GenerateRacers();
	}
	
	public void SwitchCourse(int index) {
		for (int i = 0; i < this.courses.Length; i++) {
			bool flag = i == index;
			this.courses[i].SetActive(flag);
		}
		this.RestartRace();
	}
	
	public void ChangePlayerCount() {
		int num = (int)GameplayUI.Instance.playerNumSlider.value;
		this.numberOfRacers = num;
		GameplayUI.Instance.playerCountSlider.SetText(num.ToString());
	}
	
	public void UpdateMaxStat() {
		float num = GameplayUI.Instance.maxStatSlider.value;
		this.maxStat = num;
		GameplayUI.Instance.maxStat.SetText(num.ToString("F2"));
		
	}
	
	public void UpdateMinStat() {
		float num = GameplayUI.Instance.minStatSlider.value;
		this.minStat = num;
		GameplayUI.Instance.minStat.SetText(num.ToString("F2"));
	}
}

[Serializable]
public class Race
{
    public float runPercent;
    public float flyPercent;
    public float swimPercent;
    public float climbPercent;

    public void RaceUpdate(float segments, RaceChunk[] chunks)
    {
        List<RaceChunk> runChunks = new List<RaceChunk>();
        List<RaceChunk> flyChunks = new List<RaceChunk>();
        List<RaceChunk> swimChunks = new List<RaceChunk>();
        List<RaceChunk> climbChunks = new List<RaceChunk>();

        foreach (var chunk in chunks)
        {
            switch (chunk.GetComponent<RaceChunk>().chunkType)
            {
                case ChunkType.Climb:
                    climbChunks.Add(chunk);
                    break;
                case ChunkType.Ground:
                    runChunks.Add(chunk);
                    break;
                case ChunkType.Fly:
                    flyChunks.Add(chunk);
                    break;
                case ChunkType.Swim:
                    swimChunks.Add(chunk);
                    break;
            }
        }
        
        runPercent = (runChunks.Count / segments) * 100;
        flyPercent = (flyChunks.Count / segments) * 100;
        swimPercent = (swimChunks.Count / segments) * 100;
        climbPercent = (climbChunks.Count / segments) * 100;
    }
}
