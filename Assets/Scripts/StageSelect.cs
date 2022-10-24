using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public void PokemonStadium() {
        SceneManager.LoadScene("Test");
    }
    public void Forest() {
        SceneManager.LoadScene("Stage3");
    }
}
