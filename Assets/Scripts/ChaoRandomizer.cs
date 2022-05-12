using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoRandomizer : MonoBehaviour
{
    private Animator animator;
	
	public bool randomizeChao = false;
	
	private void Awake() {
		this.animator = this.GetComponent<Animator>();
	}
	
	private void Start() {
		this.randomizeChao = true;
	}
	
	private void Update() {
		if (this.randomizeChao) {
			this.RandomizeChao();
			this.randomizeChao = false;
		}
	}
	
	public void RandomizeChao() {
		this.animator.SetFloat("First Tone", UnityEngine.Random.Range(0f, 1f));
		this.animator.SetFloat("Second Tone", UnityEngine.Random.Range(0f, 1f));
		this.animator.SetFloat("Monotone", UnityEngine.Random.Range(0f, 1f));
		this.animator.SetFloat("Ball", UnityEngine.Random.Range(0f, 1f));
		this.animator.SetFloat("Shiny", UnityEngine.Random.Range(0f, 1f));
		this.animator.SetFloat("Mouth", 0f + (((int)UnityEngine.Random.Range(0, 5)) * 0.25f));
		
		float rand = UnityEngine.Random.Range(0, 10);
		float eyes = 0.5f;
		if (rand > 7f) {
			eyes = 1f;
		} else if (rand < 3f) {
			eyes = 0f;
		}
		this.animator.SetFloat("Eyes", eyes);
	}
}
