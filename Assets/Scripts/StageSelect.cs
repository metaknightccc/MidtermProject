using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public void PokemonStadium()
    {
        SceneManager.MoveGameObjectToScene(GameObject.Find("Music"), SceneManager.GetActiveScene());
        SceneManager.LoadScene("Stage1");
    }
    public void Forest() {
        SceneManager.MoveGameObjectToScene(GameObject.Find("Music"), SceneManager.GetActiveScene());
        SceneManager.LoadScene("Stage3");
    }

    public void Lava() {
        SceneManager.MoveGameObjectToScene(GameObject.Find("Music"), SceneManager.GetActiveScene());
        SceneManager.LoadScene("Stage4");
    }

    public void Ice() {
        SceneManager.MoveGameObjectToScene(GameObject.Find("Music"), SceneManager.GetActiveScene());
        SceneManager.LoadScene("Stage2");
    }
}
