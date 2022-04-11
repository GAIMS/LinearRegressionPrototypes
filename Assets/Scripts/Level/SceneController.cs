using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController {
	
	public static void ReloadCurrentScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
