using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCore : MonoBehaviour {
	
	private Rigidbody rigidbody;
	
	internal float water = 0f;
	
	internal float wiltSpeed = 1f;
	
	internal bool dead = false;
	
	internal bool watering = false;
	
	internal float baseSize = 1f;
	
	protected virtual float WILT_SPEED {
		get {
			return 1f;
		}
	}
	
	protected virtual float BASE_WATER {
		get {
			return 50f;
		}
	}
	
	protected virtual float BASE_SIZE {
		get {
			return 1f;
		}
	}
	
	private const float MIN_WATER = 0f, MAX_WATER = 100f;
	
	protected virtual void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
	}
	
	protected virtual void Start() {
		this.Initialize();
	}
	
	protected virtual void Initialize() {
		this.water = BASE_WATER;
		this.wiltSpeed = WILT_SPEED;
		
		this.baseSize = BASE_SIZE;
		this.transform.localScale = Vector3.one * this.baseSize;
		
		this.dead = false;
		this.watering = false;
		
	}
	
	protected virtual void Update() {
		this.HandleWater();
		this.HandleSize();
	}
	
	protected virtual void FixedUpdate() {
		
	}
	
	protected virtual void HandleWater() {
		if (this.dead) {
			return;
		}
		
		if (this.watering) {
			this.Watering();
			return;
		}
		
		this.water = Mathf.Clamp(this.water - (Time.deltaTime * this.wiltSpeed), MIN_WATER, MAX_WATER);
		if (this.water <= 0f) {
			this.dead = true;
		}
	}
	
	protected virtual void HandleSize() {
		float num = this.water / MAX_WATER;
		this.transform.localScale = Vector3.one * (this.baseSize * num);
	}
	
	protected virtual void Watering() {
		this.water = Mathf.Clamp(this.water + (Time.deltaTime * CloudController.Instance.WateringSpeed), MIN_WATER, MAX_WATER);
	}
	
	protected virtual void OnTriggerEnter(Collider collider) {
		if (this.dead) {
			return;
		}
		RainHitbox rain = collider.gameObject.GetComponent<RainHitbox>();
		if (rain != null) {
			this.watering = true;
		}
	}
	
	protected virtual void OnTriggerExit(Collider collider) {
		if (this.dead) {
			return;
		}
		RainHitbox rain = collider.gameObject.GetComponent<RainHitbox>();
		if (rain != null) {
			this.watering = false;
		}
	}    
}
