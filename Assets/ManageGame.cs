using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour
{
    //Do this tomorrow
    public HealthSystem[] players;
    public int deadCount = 0;

    void Update() {
        if (deadCount == 3) {
            for(int i = 0; i < players.Length; i++) {
                if (players[i].life > 0) {
                    Debug.Log("Player "+(i+1)+" is the winner");
                }
            }
            SceneManager.LoadScene("StageSelect"); // change to win page
        }
    }
}
