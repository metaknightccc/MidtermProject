using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Vector2 spawnPoint;
    public TextMeshProUGUI label;

    void Start(){
        int winner = PlayerPrefs.GetInt("winner");
        label.text = ("Player " + winner + " wins!");
        GameObject prefab = playerPrefabs[winner-1];
        GameObject clone = Instantiate(prefab, spawnPoint, Quaternion.identity);
        Invoke("LoadStageSelect", 5f);
    }

    void LoadStageSelect() {
        SceneManager.LoadScene("StageSelect");
    }
}
