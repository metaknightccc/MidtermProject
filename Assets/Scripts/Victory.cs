using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    //public GameObject[] playerPrefabs;
    //public Transform spawnPoint;
    public TextMeshProUGUI label;

    void Start(){
        int winner = PlayerPrefs.GetInt("winner");
        label.text = ("Player " + winner + " wins!");
        Invoke("LoadStageSelect", 5f);
    }

    void LoadStageSelect() {
        SceneManager.LoadScene("StageSelect");
    }
}
