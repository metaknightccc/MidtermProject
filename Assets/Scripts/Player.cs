using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed = 5;
    public int jumpForce = 800;

    public Rigidbody2D rb;
    float xSpeed = 0;

    // int endLag;
    // int hitStun;
    void FixedUpdate()
    {
        xSpeed = Input.GetAxis("Horizontal") * speed;
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }
}
