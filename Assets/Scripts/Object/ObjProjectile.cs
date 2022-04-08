using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjProjectile : MonoBehaviour {
	
	private Rigidbody rigidbody;
	
	public Rigidbody Rigidbody {
		get {
			return this.rigidbody;
		}
	}
	
	private void Awake() {
		this.rigidbody = this.GetComponent<Rigidbody>();
	}
}
