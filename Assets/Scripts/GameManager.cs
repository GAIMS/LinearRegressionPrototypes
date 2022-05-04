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
	
	public IList<PlayerCore> players;
	
	public List<PlayerCore> team1, team2;
	
	[SerializeField]
	[Range(2, 100)]
	public int numberOfPlayers = 2;
	
	public GameObject playerPrefab;
	
	public Transform playerSpace1, playerSpace2;
	
	public float spawnRange = 3f;
	
	public Material[] playerMats;
	
	private void Awake() {
		
	}
	
	private void Start() {
		this.Initialize();
	}
	
	private void Initialize() {
		this.GeneratePlayers();
	}
	
	private void GeneratePlayers() {
		for (int i = 0; i < this.numberOfPlayers * 2f; i++ ) {
			GameObject obj = Instantiate(this.playerPrefab);
			PlayerCore player = obj.GetComponent<PlayerCore>();
			Transform transform = player.transform;
			
			if (i == 0) {
				player.isAI = false;
			} else {
				player.isAI = true;
			}			
			
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
}