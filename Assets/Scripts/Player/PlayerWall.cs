using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWall : MonoBehaviour {
	
	[SerializeField]
	private PlayerController playerOne, playerTwo;
	
	private Transform transform;
	
	[SerializeField]
	[Range(5f, 25f)]
	private float strength = 10f;
	
	private void Awake() {
		this.transform = this.gameObject.transform;
	}
	
	private void LateUpdate() {
		if (this.playerOne == null || this.playerTwo == null) {
			return;
		}
		
		this.HandleWall();
	}
	
	private void HandleWall() {
		Vector3 plyOnePos = this.playerOne.transform.position;
		Vector3 plyTwoPos = this.playerTwo.transform.position;
		Vector3 pos = (plyOnePos + plyTwoPos) / 2f;
		
		this.transform.position = pos;
		
		Vector3 targetDir = plyOnePos - plyTwoPos;
		this.transform.rotation = Quaternion.FromToRotation(Vector3.right, targetDir);
		
		float dist = Vector3.Distance(plyOnePos, plyTwoPos);
		
		dist = Mathf.Clamp(dist - 1.25f, 2f, 999f);
		this.transform.localScale = new Vector3(dist, 0.25f, 1f);
	}
	
	private void OnCollisionEnter(Collision collision) {
		Debug.Log("Colliding with " + collision.gameObject.name);
		ObjProjectile projectile = collision.gameObject.GetComponentInChildren<ObjProjectile>();
		if (projectile == null) {
			return;
		} else {
			Vector3 dir = this.transform.position - projectile.transform.position;
			float dot = Vector3.Dot(dir, this.transform.up);
			if (dot >= 0.1f) {
				projectile.Rigidbody.velocity = -this.transform.up * 10f;
			} else if (dot <= -0.1f) {
				projectile.Rigidbody.velocity = this.transform.up * 10f;
			}
		}
	}
}
