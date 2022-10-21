using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class HealthSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rateText;
    [SerializeField] private GameObject[] hearts;
    private int life;
    private bool isDead;
    public float rate=0f;

    private void Start()
    {
        life = hearts.Length;
    }

    void Update()
    {
        rateText.text = rate.ToString()+"%";
        if (isDead == true)
        {
            Debug.Log("Dead");
            SceneManager.LoadScene("Test"); // change to win page
        }
    }

    public void LoseOneHeart()
    {
        rate = 0f;
        if (life >= 1)
        {
            life -= 1;
            hearts[life].gameObject.SetActive(false);
            if (life < 1)
            {
                isDead = true;
            }
            
        }
    }

    public void Damage(float damage)
    {
        rate += damage;
    }
    
    public bool Health(int healthValue)
    {
        if (life + healthValue <= hearts.Length)
        {
            hearts[life].gameObject.SetActive(true);
            life += healthValue;
            return true;
        }
        else
        {
            return false;
        }
    }
}
