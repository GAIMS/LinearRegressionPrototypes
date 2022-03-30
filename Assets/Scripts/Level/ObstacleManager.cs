using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
	
	private static ObstacleManager _Instance;
	public static ObstacleManager Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<ObstacleManager>();
			}
			return _Instance;
		}
	}
	
	[SerializeField]
	private ObjObstacle obstacleObj;
    
	private Transform targetTransform;
	
	public Transform TargetTransform {
		get {
			return this.targetTransform;
		} set {
			this.targetTransform = value;
		}
	}
	
	[SerializeField]
	[Range(0.1f, 10f)]
	private float timeBetweenRounds = 3.5f;
	
	[SerializeField]
	[Range(0.1f, 10f)]
	private float timePerRound = 0.5f;
	
	private float timer = 0;
	
	private bool isRound = false;
	
	public Transform obstacleStartPosition, obstacleEndPosition;
	
	private void Awake() {
	}
	
	private void Start() {
		this.isRound = true;
		
		if (this.obstacleObj != null) {
			this.obstacleObj.SetObstacle();
		}
	}
	
	private void Update() {
		this.timer += Time.deltaTime;
		
		if (this.isRound) {
			if (this.timer >= this.timePerRound) {
				if (this.obstacleObj != null) {
					this.obstacleObj.ActivateObstacles();
				}
				this.timer -= this.timePerRound;
				this.isRound = false;
			}
		} else {
			if (this.timer >= this.timeBetweenRounds) {
				if (this.obstacleObj != null) {
					this.obstacleObj.SetObstacle();
				}
				this.timer -= this.timeBetweenRounds;
				this.isRound = true;
			}
		}
	}
}
