using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public void PokemonStadium() {
        SceneManager.LoadScene("Stage1");
    }
    public void Forest() {
        SceneManager.LoadScene("Stage3");
    }

    public void Lava() {
        SceneManager.LoadScene("Stage4");
    }

    public void Ice() {
        SceneManager.LoadScene("Stage2");
    }
}
