using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour {
	
	private static PlayerObject _Instance;
	public static PlayerObject Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<PlayerObject>();
			}
			return _Instance;
		}
	}
	
	
	
    public GameObject playerOne, playerTwo;
	
	public PlayerWall wall;
	
	private void Awake() {
		this.wall = this.GetComponentInChildren<PlayerWall>();
	}
}
