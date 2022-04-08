using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour {
	
    public GameObject bulletPrefab;
	
	public float fireRate = 5f;
	
	internal float fireTimer = 0f;
	
	protected virtual void Awake() {
		
	}
	
	protected virtual void Start() {
		
	}
	
	protected virtual void Update() {
		
	}
	
	protected virtual void FixedUpdate() {
		
	}
	
	protected virtual void Fire() {
		GameObject proj = Instantiate(this.bulletPrefab, this.transform.position, Quaternion.identity);
	}
}
