using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjObstacle : MonoBehaviour {
	
	public GameObject[] obstacles;
	
	private Renderer[] renderers;
	
	private Collider[] colliders;
	
	private void Awake() {
		this.renderers = new Renderer[this.obstacles.Length];
		this.colliders = new Collider[this.obstacles.Length];
		for (int i = 0; i < this.obstacles.Length; i++) {
			this.renderers[i] = this.obstacles[i].GetComponentInChildren<Renderer>();
			this.colliders[i] = this.obstacles[i].GetComponentInChildren<Collider>();
		}
	}
	
	public void SetObstacle() {
		int rand = UnityEngine.Random.Range(0, this.obstacles.Length);
		
		for (int i = 0; i < this.obstacles.Length; i++) {
			bool flag = i == rand;
			this.obstacles[i].SetActive(false);
			this.renderers[i].enabled = !flag;
			this.colliders[i].enabled = !flag;
			if (flag) {
				ObstacleManager.Instance.TargetTransform = this.obstacles[i].transform;
			}
		}
	}	

	public void ActivateObstacles() {
		if (this.activateRoutine != null) {
			this.StopCoroutine(this.activateRoutine);
		}
		this.activateRoutine = this.StartCoroutine(this.ActivateRoutine());	
	}
	
	private Coroutine activateRoutine;
	
	private const float TIME_TO_ACTIVATE_VISUALS = 0.25f;
	
	private const float TIME_TO_ACTIVATE_COLLIDERS = 0.5f;
	
	private IEnumerator ActivateRoutine() {
		yield return new WaitForSeconds(TIME_TO_ACTIVATE_VISUALS);
		for (int i = 0; i < this.obstacles.Length; i++) {
			this.obstacles[i].SetActive(true);
		}
		yield return new WaitForSeconds(TIME_TO_ACTIVATE_COLLIDERS);
		for (int i = 0; i < this.colliders.Length; i++) {
			this.colliders[i].enabled = true;
		}			
	}
}
