using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumberOfPlayers : MonoBehaviour
{
    public void TwoPlayers(){
        PlayerPrefs.SetInt("PlayerNumber",2);
        SceneManager.LoadScene("StageSelect");
    }

    public void ThreePlayers(){
        PlayerPrefs.SetInt("PlayerNumber",3);
        SceneManager.LoadScene("StageSelect");
    }

    public void FourPlayers(){
        PlayerPrefs.SetInt("PlayerNumber",4);
        SceneManager.LoadScene("StageSelect");
    }
}
