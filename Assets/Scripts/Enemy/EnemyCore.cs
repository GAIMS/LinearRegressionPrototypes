using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour {
	
    public GameObject bulletPrefab;
	
	public Rigidbody rigidbody { get; private set; }
	
	internal int baseHP;
	
	internal int hp;
	
	internal float fireRate = 5f;
	
	internal float fireTimer = 0f;
	
	internal float projectileSpeed = 10f;
	
	protected virtual int BASE_HEALTH {
		get {
			return 1;
		}
	}
	
	protected virtual float FIRE_RATE {
		get {
			return 5f;
		}
	}
	
	protected virtual float SHOOT_SPEED {
		get {
			return 10f;
		}
	}
	
	protected virtual void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
		
	}
	
	protected virtual void Start() {
		this.Initialize();
	}
	
	protected virtual void Initialize() {
		this.baseHP = BASE_HEALTH;
		this.hp = this.baseHP;
		
		this.fireRate = FIRE_RATE;
		this.fireTimer = this.fireRate;
		
		this.projectileSpeed = SHOOT_SPEED;
		
		this.gameObject.SetActive(true);
	}
	
	protected virtual void Update() {
	}
	
	protected virtual void FixedUpdate() {
	}
	
	protected virtual void LateUpdate() {
	}
	
	protected virtual void HandleAim() {
	}
	
	protected virtual void HandleFireTimer() {
	}
	
	protected virtual void Fire() {
		GameObject obj = Instantiate(this.bulletPrefab, this.transform.position, Quaternion.identity);
		ObjProjectile proj = obj.GetComponent<ObjProjectile>();
		proj.transform.up = this.transform.up;
		proj.rigidbody.velocity = proj.transform.up * this.projectileSpeed;
	}
	
	protected virtual void Damage() {
		this.hp--;
		if (this.hp <= 0){ 
			this.gameObject.SetActive(false);
		}
	}
	
	protected virtual void OnTriggerEnter(Collider collider) {
		ObjProjectile projectile = collider.gameObject.GetComponentInChildren<ObjProjectile>();
		if (projectile == null) {
			return;
		} else {
			if (!projectile.isPlayer) {
				return;
			}
			projectile.gameObject.SetActive(false);
			this.Damage();
		}
	}
}
