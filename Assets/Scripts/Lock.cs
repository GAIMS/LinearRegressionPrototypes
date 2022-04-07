using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {
	
	private static Lock _Instance;
	public static Lock Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<Lock>();
			}
			return _Instance;
		}
	}
	
	[SerializeField]
	private float targetAngle;
	
	public float TargetAngle {
		get {
			return this.targetAngle;
		} set {
			this.targetAngle = value;
		}
	}
	
	[SerializeField]
	[Range(1f, 10f)]
	private float offset = 0f;
	
	public float Offset {
		get {
			return this.offset;
		} set {
			this.offset = value;
		}
	}
	
	private Transform transform;
	
	private Animator animator;
	
	public Animator Animator {
		get {
			return this.animator;
		}
	}
	
	public const float ANGLE_RANGE = 100f;
	
	private void Awake() {
		this.animator = this.GetComponent<Animator>();
		this.transform = this.gameObject.transform;
	}
	
	private void Start() {
		this.SetLock();
	}
	
	private void Update() {
		
	}
	
	public void SetLock() {
		float rand = UnityEngine.Random.Range(-ANGLE_RANGE, ANGLE_RANGE);
		this.TargetAngle = rand;
	}
	
	public void CheckLock() {
		bool flag0 = this.IsWithinRange(this.offset);
		
		bool flag1 = this.IsWithinRange(90f);
		
		LockPickController.Instance.IsUnlocking = true;
		
		if (this.lockRoutine != null) {
			this.StopCoroutine(this.lockRoutine);
		}
		
		if (!flag1) {
			this.lockRoutine = this.StartCoroutine(this.FailedRoutine());
			return;
		} else {
			this.lockRoutine = this.StartCoroutine(this.UnlockRoutine(flag0));
			return;
		}			
	}
	
	private Coroutine lockRoutine;
	
	private IEnumerator FailedRoutine() {
		LockAI.Instance.Failed();
		this.animator.Play("Failed");
		yield return new WaitForSeconds(0.75f);
		this.animator.Play("Locked");
		LockPickController.Instance.IsUnlocking = false;
	}
	
	private IEnumerator UnlockRoutine(bool unlocked) {
		float lockPickRot = LockPickController.Instance.lockTransform.localEulerAngles.z;
		if (lockPickRot > 180f) {
			lockPickRot = (360f - lockPickRot) * -1f;
		}
		float abs = Mathf.Abs(lockPickRot - this.targetAngle);
	//	Debug.Log("Abs: " + abs);
		float speed = abs * 0.1f;
	//	Debug.Log("Speed: " + speed);
		
		this.animator.Play("Unlocking");
		speed = Mathf.Clamp(1f / speed, 0f, 1f);
	//	Debug.Log("Speed 2: " + speed);
		
		this.animator.speed = speed;
		LockAI.Instance.UpdateInfo(this.animator.speed);
		yield return new WaitForSeconds(1f);
		if (!unlocked) {
			this.animator.speed = 0f;
			yield return new WaitForSeconds(1f);
			this.animator.speed = 1f;
			this.animator.Play("Locked");
			LockPickController.Instance.IsUnlocking = false;
			
		} else {
			yield return new WaitForSeconds(1f);
			this.ResetLock();
			Debug.Log("Unlocked Lock!");
		}
	}
	
	public bool IsWithinRange(float offset) {
		float lockPickRot = LockPickController.Instance.lockTransform.localEulerAngles.z;
		
		if (lockPickRot > 180f) {
			lockPickRot = (360f - lockPickRot) * -1f;
		}
		
		float min = this.targetAngle - offset;
		float max = this.targetAngle + offset;
		return lockPickRot >= Mathf.Min(min, max) && lockPickRot <= Mathf.Max(min, max);
	}
	
	public void ResetLock() {
		this.SetLock();
		LockPickController.Instance.ResetAngle();
		LockPickController.Instance.IsUnlocking = false;
		this.animator.Play("Locked");
	}		
}
