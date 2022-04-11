using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainHitbox : MonoBehaviour {
	
	private Rigidbody rigidbody;
	
	private Collider collider;
	
	private void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
		this.collider = this.GetComponentInChildren<Collider>();
	}
		
	
    public void ToggleHitbox(bool active) {
		this.gameObject.SetActive(active);
	}
}
