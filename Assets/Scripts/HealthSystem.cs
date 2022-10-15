using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;
    private int life;
    private bool isDead;

    private void Start()
    {
        life = hearts.Length;
    }

    void Update()
    {
        if (isDead == true)
        {
            Debug.Log("Dead");
            SceneManager.LoadScene("Test");
        }
    }

    public void TakeDamage(int damage)
    {
        if (life >= 1)
        {
            life -= damage;
            hearts[life].gameObject.SetActive(false);
            if (life < 1)
            {
                isDead = true;
            }
            
        }
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
