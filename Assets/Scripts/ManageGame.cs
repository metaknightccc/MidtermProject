using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour
{
    //Do this tomorrow
    public HealthSystem[] players;
    //public GameObject[] playerObjects;
    public int deadCount = 0;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject percent1;
    public GameObject percent2;
    public GameObject percent3;
    public GameObject percent4;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;

    /*
    void Start() {
        int playerCount = PlayerPrefs.GetInt("PlayerNumber");
        if (playerCount == 2) {
            playerObjects[2].enabled == false;
            playerObjects[3].enabled == false;
        }
    }
    */
    private void Start()
    {
        if (PublicVars.PlayNum==2)
        {
            deadCount=2;
            player3.SetActive(false);
            player4.SetActive(false);
            percent3.SetActive(false);
            percent4.SetActive(false);
            heart3.SetActive(false);
            heart4.SetActive(false);
            //heart inactive, player inactive, percent inactive
        }
        else if (PublicVars.PlayNum==3)
        {
            deadCount=1;
            player4.SetActive(false);
            percent4.SetActive(false);
            heart4.SetActive(false);
        }
    }

    void Update() {
        if (deadCount == 3) {
            for(int i = 0; i < players.Length; i++) {
                if (players[i].life > 0) {
                    Debug.Log("Player "+(i+1)+" is the winner");
                    PlayerPrefs.SetInt("winner",i+1);
                    break;
                }
            }
            SceneManager.LoadScene("Victory", LoadSceneMode.Single);
        }
    }
}
