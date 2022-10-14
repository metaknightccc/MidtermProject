using UnityEngine;


public class Player : MonoBehaviour
{
    public int speed = 5;
    public int jumpForce = 800;

    public Transform feet;
    public LayerMask groundLayer;
    public float radius = 0.3f;
    public bool grounded = false;
    public int extraJumps = 1;

    public Rigidbody2D rb;
    float xSpeed = 0;



    // int endLag;
    // int hitStun;

    void Update() {
         
                    grounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
                    if(Input.GetButtonDown("Jump"))
                    {
                        if (grounded) {
                            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                        } else {
                            if (extraJumps > 0) {
                                extraJumps -= 1;
                                rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                            }
                        }
                    }
                    if (grounded) {
                        extraJumps = 1;
                    }
                
    }

    void FixedUpdate()
    {
        xSpeed = Input.GetAxis("Horizontal") * speed;
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }
}
