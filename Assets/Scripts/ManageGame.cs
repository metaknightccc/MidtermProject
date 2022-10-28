using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour
{
    //Do this tomorrow
    public HealthSystem[] players;
    //public GameObject[] playerObjects;
    public int deadCount = 0;

    /*
    void Start() {
        int playerCount = PlayerPrefs.GetInt("PlayerNumber");
        if (playerCount == 2) {
            playerObjects[2].enabled == false;
            playerObjects[3].enabled == false;
        }
    }
    */

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
