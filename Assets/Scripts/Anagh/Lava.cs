using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    private HealthSystem health;

    Coroutine watch;

    private void OnTriggerStay2D(Collider2D other) {
        // TODO: Modify this for enemy / second player tag as well if needed
        if (other.gameObject.tag == "Player") {
            print("Player touched lava!");
            print(other.gameObject);
            health = other.gameObject.GetComponent<HealthSystem>();
            print(health);
            if (watch == null) {
                watch = StartCoroutine(inflictLavaDamage());
            }
        }
    }

    IEnumerator inflictLavaDamage() {
        yield return new WaitForSeconds(2);
        health.Damage(5);
        watch = null;
    }


}
