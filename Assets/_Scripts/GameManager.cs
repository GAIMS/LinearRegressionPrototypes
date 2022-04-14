using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool bots;
    [SerializeField] private bool singlePlayer;
    [SerializeField] private bool twoPlayer;

    private PlayerController player;
    private BotManager botManager;
    private MapGenerator mapGenerator;
    private Hole hole;
    private NoiseGenerator background;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        botManager = FindObjectOfType<BotManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        hole = FindObjectOfType<Hole>();
        background = FindObjectOfType<NoiseGenerator>();

        if (bots)
        {
            player.gameObject.SetActive(false);
            botManager.gameObject.SetActive(true);
            mapGenerator.GetComponent<Renderer>().enabled = true;
            hole.GetComponent<Renderer>().enabled = true;
            background.GetComponent<Renderer>().enabled = true;
        }
        else if (singlePlayer)
        {
            player.gameObject.SetActive(true);
            botManager.gameObject.SetActive(false);
            mapGenerator.GetComponent<Renderer>().enabled = false;
            hole.GetComponent<Renderer>().enabled = false;
            background.GetComponent<Renderer>().enabled = false;
        }
        else if(twoPlayer)
        {
            player.gameObject.SetActive(true);
            botManager.gameObject.SetActive(false);
            mapGenerator.GetComponent<Renderer>().enabled = false;
            hole.GetComponent<Renderer>().enabled = false;
            background.GetComponent<Renderer>().enabled = false;
        }
    }

    
    void Update()
    {
        
    }
}
