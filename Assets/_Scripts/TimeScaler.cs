using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeScaler : MonoBehaviour
{
    public Button DownButton;
    public Button UpButton;
    public TMP_Text SpeedText;

    void Awake() {
        if (DownButton == null) {
            Debug.LogError("DownButton is missing.");
        }
        if (UpButton == null) {
            Debug.LogError("UpButton is missing.");
        }
        if (SpeedText == null) {
            Debug.LogError("SpeedText is missing.");
        }
    }

    void Start() {
        if (DownButton != null) {
            DownButton.onClick.AddListener(() => { AdjustSpeed(0.5f); });
        }
        if (UpButton != null) {
            UpButton.onClick.AddListener(() => { AdjustSpeed(2f); });
        }
    }

    public void AdjustSpeed(float modifier) {
        if (Time.timeScale * modifier > 100f) { // Time.timeScale cannot go above 100
            return;
        }
        Time.timeScale *= modifier;
        if (SpeedText != null) {
            SpeedText.text = "X" + Time.timeScale;
        }
    }
}
