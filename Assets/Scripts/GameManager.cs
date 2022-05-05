using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
	
	public IList<PlayerCore> players;
	
	public List<PlayerCore> team1, team2;
	
	[SerializeField]
	[Range(2, 1000)]
	public int numberOfPlayers = 2;
	
	public GameObject playerPrefab;
	
	public Transform playerSpace1, playerSpace2;
	
	public float spawnRange = 3f;
	
	public Material[] playerMats;
	
	public State state;
	
	public GameObject team1Highlight, team2Highlight;
	
	public TextMeshProUGUI results;
	
	public TextMeshProUGUI team1ScoreText, team2ScoreText;
	
	public int team1Score, team2Score;
	
	public int Team1Score {
		get {
			return this.team1Score;
		} set {
			this.team1Score = value;
			this.team1ScoreText.SetText(this.team1Score.ToString());
		}
	}
	
	public int Team2Score {
		get {
			return this.team2Score;
		} set {
			this.team2Score = value;
			this.team2ScoreText.SetText(this.team2Score.ToString());
		}
	}
	
	public enum State {
		Preparing,
		Game,
		Results
	}

	public Transform targetLine;
	
	public bool holdPositionToWin = false;
	
	private void Awake() {
		
	}
	
	private void Start() {
		this.Initialize();
		
		this.StartCoroutine(this.PrepareGame());
	}
	
	private void Initialize() {
		this.GeneratePlayers();
	}
	
	private void GeneratePlayers() {
		for (int i = 0; i < this.numberOfPlayers * 2f; i++ ) {
			GameObject obj = Instantiate(this.playerPrefab);
			PlayerCore player = obj.GetComponent<PlayerCore>();
			Transform transform = player.transform;
			
		//	if (i == 0) {
		//		player.isAI = false;
		//	} else {
				player.isAI = true;
		//	}			
			
			bool flag = i < (this.numberOfPlayers * 2f) / 2;
			
			Vector3 spawnPosition = new Vector3(
				UnityEngine.Random.Range(-this.spawnRange, this.spawnRange) * 4f,
				1f,
				UnityEngine.Random.Range(-this.spawnRange, this.spawnRange) * 4f
				);
			
			transform.position = ((flag) ? this.playerSpace1.position + spawnPosition : playerSpace2.position + spawnPosition);
			player.team = ((flag) ? 1 : 2);
			Renderer renderer = player.GetComponentInChildren<Renderer>();
			
			bool colorA = i < (0.25f * (this.numberOfPlayers * 2f));
			bool colorB = i < (0.5f * (this.numberOfPlayers * 2f));
			bool colorC = i < (0.75f * (this.numberOfPlayers * 2f));
			bool colorD = i < this.numberOfPlayers * 2f;
			
			renderer.material = this.playerMats[((colorA) ? 0 : ((colorB) ? 1 : ((colorC) ? 2 : 3)))];
			if (flag) { 
				this.team1.Add(player);
			} else {
				this.team2.Add(player);
			}
		}
		this.players = UnityEngine.Object.FindObjectsOfType<PlayerCore>();
	}
	
	public void ResetGame() {
		for (int i = 0; i < this.players.Count; i++) {
			Destroy(this.players[i].gameObject);
		}
		this.players.Clear();
		this.team1.Clear();
		this.team2.Clear();
	}

	private IEnumerator PrepareGame() {
		this.state = GameManager.State.Preparing;
		this.team1Highlight.SetActive(false);
		this.team2Highlight.SetActive(false);
		this.results.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		Vector3 posA = this.targetLine.position;
		posA.x = UnityEngine.Random.Range(-360f, 360f);
		posA.z = UnityEngine.Random.Range(-360f, 360f);
		Vector3 posB = this.targetLine.position;
		posB.x = UnityEngine.Random.Range(-360f, 360f);
		posB.z = UnityEngine.Random.Range(-360f, 360f);
		Vector3 targetDir = posA - posB;
		this.targetLine.rotation = Quaternion.FromToRotation(Vector3.forward, targetDir);
		yield return new WaitForSeconds(0.5f);
		this.state = GameManager.State.Game;
		
	}
	
	public IEnumerator Results(int team) {
		this.state = GameManager.State.Results;
		if (team == 1) {
			this.Team1Score++;
			this.team1Highlight.SetActive(true);
		} else {
			this.Team2Score++;
			this.team2Highlight.SetActive(true);
		}
		this.results.gameObject.SetActive(true);
		this.results.SetText("Team " + team + " scored a point!");
		yield return new WaitForSeconds(1.5f);
		this.StartCoroutine(this.PrepareGame());		
	}
	
	public bool IsLineMatching(Vector3 forward) {
		float a = Vector3.Angle(forward, this.targetLine.transform.forward);
		float b = Vector3.Angle(forward, -this.targetLine.transform.forward);
		
		return a < 2.5f || b < 2.5f;
	}
}