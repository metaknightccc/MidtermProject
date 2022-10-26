using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    public Rigidbody2D thisObject;
    public float lifeTime = 0.2f;
    public float multiplier = 0.5f;
    public float baseKnockbackX;
    public float baseKnockbackY;
    public float damage;
    float knockbackX;
    float knockbackY;
    float direction;

    void Start()
    {
        Destroy(gameObject,lifeTime);
    }

    void Update()
    {
        if (thisObject.velocity.x > 0) {
            direction = 1f;
        }
        if (thisObject.velocity.x < 0) {
            direction = -1f;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            float percent = other.GetComponent<HealthSystem>().rate;
            knockbackX = baseKnockbackX * (1 + (percent / 50)) * multiplier;
            knockbackY = baseKnockbackY * (1 + (percent / 50)) * multiplier;
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x, 0) * knockbackX * direction);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, transform.localScale.y) * knockbackY);
            other.GetComponent<HealthSystem>().Damage(damage);
        }
        Destroy(gameObject);
    }
}
