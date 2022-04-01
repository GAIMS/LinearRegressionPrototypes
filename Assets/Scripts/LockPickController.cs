using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickController : MonoBehaviour {
	
	private static LockPickController _Instance;
	public static LockPickController Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<LockPickController>();
			}
			return _Instance;
		}
	}
	
	[SerializeField]
	private KeyCode left, right, unlock;
	
	public Transform lockTransform;
	
	private float angle = 0f;
	
	public float Angle {
		get {
			return this.angle;
		} set {
			float num = Mathf.Clamp(value, -Lock.ANGLE_RANGE, Lock.ANGLE_RANGE);
			this.angle = num;
			this.UpdateRotation();
		}
	}
	
	[SerializeField]
	private float speed = 5f;
	
	private bool isUnlocking = false;
	
	public bool IsUnlocking {
		get {
			return this.isUnlocking;
		} set {
			this.isUnlocking = value;
		}
	}
	
	private void Awake() {
		
	}
	
	private void Start() {
		this.ResetAngle();
	}
	
	private void Update() {
		
		if (this.isUnlocking) {
			return;
		} else {
			if (Input.GetKeyDown(this.unlock)) {
				Lock.Instance.CheckLock();
			}
		}
		
		if (Input.GetKey(this.left)) {
			this.Angle += this.speed * Time.deltaTime;
			return;
		}
		
		if (Input.GetKey(this.right)) {
			this.Angle -= this.speed * Time.deltaTime;
			return;
		}
	}
	
	public void ResetAngle() {
		this.angle = 0f;
		this.UpdateRotation();
	}
	
	public void UpdateRotation() {
		Vector3 rot = Vector3.one * this.angle;
		rot.x = 0f;
		rot.y = 0f;
		this.lockTransform.localEulerAngles = rot;
	}
}
