using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	
    public GameObject enemyPrefab;
	
	private int currentActiveAmt = 0;
	
	public int maxEnemies = 10;
	
	public int initEnemies = 3;
	
	public float spawnRate = 5f;
	
	private float spawnTimer = 0f;
	
	private void Start() {
		this.currentActiveAmt = 0;
		
		for (int i = 0; i < this.initEnemies; i++) {
			this.SpawnEnemy();
		}
		this.spawnTimer = this.spawnRate;
	}
	
	private void Update() {
		if (this.currentActiveAmt >= this.maxEnemies) {
			return;
		}
		if (this.spawnTimer > 0f) {
			this.spawnTimer -= Time.deltaTime;
		} else if (this.spawnTimer <= 0f) {
			this.SpawnEnemy();
			this.spawnTimer = this.spawnRate;
		}
	}
	
	
	public void SpawnEnemy() {
		GameObject obj = Instantiate(this.enemyPrefab, this.transform.position, Quaternion.identity);
		Vector2 pos = Random.insideUnitCircle.normalized * (LevelParams.Instance.Radius + 2.5f);
		Vector3 v = new Vector3(pos.x, pos.y, 0f);
		obj.transform.position = v;
		this.currentActiveAmt++;
	}
	
	public void EnemyDied() {
		this.currentActiveAmt--;
	}
}
