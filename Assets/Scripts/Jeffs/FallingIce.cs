using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingIce : MonoBehaviour
{
    // Start is called before the first frame update
    public int power = 100;
    [SerializeField] private Animator falling;
    Vector2 kb;
    Rigidbody2D player;
    public int dmg = 5;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Ice")
        {

            StartCoroutine(wait());
            
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<HealthSystem>().Damage(dmg);
            kb = new Vector2(0, power);
            player = collision.GetComponent<Rigidbody2D>();
            player.AddForce(kb);
        }
    }
    
    void FixedUpdate(){
        StartCoroutine(destruction());
    }
    IEnumerator wait(){
        Destroy(GetComponent<Rigidbody2D>());
        falling.Play("Falling_ice", 0, 0);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    IEnumerator destruction(){
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }
}

