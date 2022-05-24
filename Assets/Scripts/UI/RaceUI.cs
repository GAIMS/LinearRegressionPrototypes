using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RankInfo {
	
	public Image image;
	
	public Animator animator;
	
}

[System.Serializable]
public class RacerInfo {
	
	public RawImage image;
	
	public TextMeshProUGUI name;
	
	public RankInfo rank;
	
	public Transform speed, fly, swim, climb, stamina, acceleration;
	
	public int placement = 0;
}

public class RaceUI : MonoBehaviour {
	
	public RacerInfo racerInfo;

	public void SetRenderTexture(RenderTexture texture) {
		this.racerInfo.image.texture = texture;
	}
	
	public void SetName(string name) {
		this.racerInfo.name.SetText(name);
	}
	
	public void SetRank(int rank) {
		this.racerInfo.rank.animator.Play("Switch", -1, 0f);
		this.racerInfo.rank.image.sprite = GameManager.Instance.ranks[rank];
		this.racerInfo.placement = rank;
		GameplayUI.Instance.ReorderList();
	}
	
	public void SetStats(float[] stats) {
		this.racerInfo.speed.localScale = new Vector3(stats[0], 1f, 1f);
		this.racerInfo.fly.localScale = new Vector3(stats[1], 1f, 1f);
		this.racerInfo.swim.localScale = new Vector3(stats[2], 1f, 1f);
		this.racerInfo.climb.localScale = new Vector3(stats[3], 1f, 1f);
		this.racerInfo.acceleration.localScale = new Vector3(stats[4], 1f, 1f);
	}
}


