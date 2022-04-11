using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlower : FlowerCore {
	
	protected override float WILT_SPEED {
		get {
			return 5f;
		}
	}
	
	protected override float BASE_WATER {
		get {
			return 50f;
		}
	}
	
	protected override float BASE_SIZE {
		get {
			return 1f;
		}
	}
	
	protected override void Update() {
		base.Update();
	}
}
