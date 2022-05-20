using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotNumController : MonoBehaviour
{
    public Button DownButton;
    public Button UpButton;
    public TMP_Text SpeedText;
    public RaceTrackGenerator track;
    
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
            DownButton.onClick.AddListener(() => { AdjustSpeed(-1); });
        }
        if (UpButton != null) {
            UpButton.onClick.AddListener(() => { AdjustSpeed(1); });
        }
    }

    public void AdjustSpeed(int modifier) 
    {
        track.racers += modifier;
        if (SpeedText != null) 
        {
            SpeedText.text = "Bots: " + track.racers;
        }
    }
}
