using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool bots;
    [SerializeField] private bool singlePlayer;
    [SerializeField] private bool twoPlayer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Text gameUI;

    private PlayerController player;
    private BotManager botManager;
    private MapGenerator mapGenerator;
    private Hole hole;
    private NoiseGenerator background;
    private PlayerController playerTwo;
    void Awake()
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
            playerTwo = Instantiate(playerPrefab).GetComponent<PlayerController>();
            playerTwo.playerTwo = true;
            playerTwo.twoPlayer = true;
            player.myTurn = true;
            player.twoPlayer = true;
            player.gameObject.SetActive(true);
            botManager.gameObject.SetActive(false);
            mapGenerator.GetComponent<Renderer>().enabled = false;
            hole.GetComponent<Renderer>().enabled = false;
            background.GetComponent<Renderer>().enabled = false;
        }
    }

    public void ChangeTurn(PlayerController pc)
    {
        if (pc.gameObject.name == player.gameObject.name)
        {
            playerTwo.myTurn = true;
            player.myTurn = false;
        }
        else if (pc.gameObject.name == playerTwo.gameObject.name)
        {
            player.myTurn = true;
            playerTwo.myTurn = false;
        }
    }
}
