using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParams : MonoBehaviour {
	
	private static LevelParams _Instance;
	public static LevelParams Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<LevelParams>();
			}
			return _Instance;
		}
	}
	
	public Camera camera;
	
	[SerializeField]
	[Range(5f, 50f)]
	private float radius = 10f;
	
	public float Radius {
		get {
			return this.radius;
		}
	}
	
	[SerializeField]
	private Transform arenaTrans;
	
	private void Start() {
		Vector3 scale = Vector3.one * (this.radius * 2f);
		scale.x = scale.z + 1f;
		scale.y = 1f;
		this.arenaTrans.localScale = scale;
		
		this.camera.orthographicSize = this.radius + 5f;
	}
}
