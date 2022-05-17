using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	
	[Range(0.1f, 1f)]
	public float minStat = 0.1f;
	
	[Range(0.1f, 1f)]
	public float maxStat = 1f;
	
	[Range(2, 12)]
	public int numberOfRacers = 2;
	
	[SerializeField]
	public GameObject racerPrefab;
	
	public List<GameObject> racers;
	
	public Sprite[] ranks;
	
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
		}
	}
	
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			this.GenerateRacers();
		}
		
		this.TrackPositions();
	}
	
	private void TrackPositions() {
		this.racers = this.racers.OrderBy(
			x => Vector2.Distance(this.transform.position, new Vector2(x.transform.position.x, 0f))
		).ToList();
		for (int i = 0; i < this.racers.Count; i++) {
			RacerCore core = this.racers[i].GetComponent<RacerCore>();
			core.SetRank(11 - i);
		}
	}
}
