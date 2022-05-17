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
	
	[Range(0.1f, 1f)]
	public float minStat = 0.1f;
	
	[Range(0.1f, 1f)]
	public float maxStat = 1f;	
    
}
