using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAI : MonoBehaviour {
	
	private static LockAI _Instance;
	public static LockAI Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<LockAI>();
			}
			return _Instance;
		}
	}
	
	[SerializeField]
	private float targetAngle;
	
	public List<float> prevDistance = new List<float>();
	
	public List<float> prevAngle = new List<float>();
	
	private Lock _lock;
	
	private LockPickController controller;
	
	[SerializeField]
	private bool isActive = false;
	
	private bool wasMovingAway = false;
	
	public bool IsActive {
		get {
			return this.isActive;
		}
	}
	
	private void Awake() {
		this._lock = this.GetComponentInChildren<Lock>();
		this.controller = this.GetComponentInChildren<LockPickController>();
	}
	
	private void Update() {
		bool flag = LockPickController.Instance.IsUnlocking;
		
		if (flag) {
			return;
		} else {
			float angle = LockPickController.Instance.Angle;
			if (angle < this.targetAngle - Lock.Instance.Offset) {
				LockPickController.Instance.Angle += LockPickController.Instance.Speed * Time.deltaTime;
			} else if (angle > this.targetAngle + Lock.Instance.Offset) {
				LockPickController.Instance.Angle -= LockPickController.Instance.Speed * Time.deltaTime;
			} else {
				Lock.Instance.CheckLock();
			}
			
		}			
	}
	
	public void UpdateInfo(float num) {
		if (this.prevDistance.Count > 0) {
			if (num < this.prevDistance[this.prevDistance.Count - 1]) {
				this.prevDistance.Add(num);
				this.prevAngle.Add(this.targetAngle);
			}
		} else {
			this.prevDistance.Add(num);
			this.prevAngle.Add(this.targetAngle);
		}
		
		this.SetAngle();
	}
	
	public void Failed() {
		if (this.prevDistance.Count > 0) {
			this.SetAngle();
		} else {
			if (this.targetAngle > -45f && this.targetAngle < 45f) {
				float rand = UnityEngine.Random.Range(-Lock.ANGLE_RANGE, Lock.ANGLE_RANGE);
				this.targetAngle = rand;
				return;
			}				
			this.targetAngle = -this.targetAngle;
		}
	}
		
	
	private void SetAngle() {
		int index = 0;
		float dist = 0f;
		float angle = 999f;
		if (this.prevDistance.Count > 0) {
			for (int i = 0; i < this.prevDistance.Count; i++) {
				if (this.prevDistance[i] > dist) {
					dist = this.prevDistance[i];
					angle = this.prevAngle[i];
				}
			}
			float offset = 100 - (dist * 100);
			offset = Mathf.Clamp(offset, 15f, 90f);
			this.targetAngle = Mathf.Clamp(UnityEngine.Random.Range(angle - offset, angle + offset), -100f, 100f);
			return;
		} else {
			float rand = UnityEngine.Random.Range(-Lock.ANGLE_RANGE, Lock.ANGLE_RANGE);
			this.targetAngle = rand;
			return;
		}
	}
	
	public void SetStartAngle() {
		this.prevAngle.Clear();
		this.prevDistance.Clear();
		float rand = UnityEngine.Random.Range(-Lock.ANGLE_RANGE, Lock.ANGLE_RANGE);
		this.targetAngle = rand;
	}
}
