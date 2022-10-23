using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDmg : MonoBehaviour
{
    public int power = 100;
    public int dmg = 5;
    Vector2 kb;
    Rigidbody2D player;
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<HealthSystem>().Damage(dmg);
            var force = transform.position - collision.transform.position;
            
            player = collision.GetComponent<Rigidbody2D>();
            player.AddForce(-force * power);
        }
    }
}
