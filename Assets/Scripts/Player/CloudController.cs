using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {
	
	private static CloudController _Instance;
	public static CloudController Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<CloudController>();
			}
			return _Instance;
		}
	}
	
	private Rigidbody rigidbody;
	
	[SerializeField]
	[Range(5f, 100f)]
	private float baseWateringSpeed = 10f;
	
	private float wateringSpeed = 10f;
	
	public float WateringSpeed {
		get {
			return this.wateringSpeed;
		} set {
			this.wateringSpeed = value;
		}
	}
	
	[SerializeField]
	private RainHitbox rainHitbox;
	
	private void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
	}
	
	private void Start() {
		this.rainHitbox.ToggleHitbox(true);
		
		this.wateringSpeed = this.baseWateringSpeed;
	}
	
	private void Update() {
		if (Input.GetKey(KeyCode.D)) {
			this.transform.Rotate(Vector3.up * (50f * Time.deltaTime));
			
		}
		
		if (Input.GetKey(KeyCode.A)) {
			this.transform.Rotate(Vector3.up * (-50f * Time.deltaTime));
		}
	}
	
	private void FixedUpdate() {
		
	}
}
