using UnityEngine;
using UnityEngine.Input;

public class Player : MonoBehaviour
{
    // Variables related to movement
    public int speed = 5;
    public int jumpForce = 800;

    public Transform feet;
    public LayerMask groundLayer;
    public float radius = 0.3f;
    public bool grounded = false;
    public int extraJumps = 1;
    public float health;
    public InputAction playerControls;

    public Rigidbody2D rb;
    float xSpeed = 0;


    // Variables related to combat
    public double endLag = 0;
    //float hitStun = 0;

    public Transform[] hitboxes;
    public float[] hitboxSizes;
    public Vector2[] knockbacks;


    void Update() {
        if (endLag <= 0) {
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
            
            if(Input.GetMouseButtonDown(0) && grounded) {
                endLag = 0.5;
                Debug.Log(endLag);
                rb.velocity = new Vector2(0, rb.velocity.y);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(hitboxes[0].position, hitboxSizes[0]);
                foreach (Collider2D nearby in colliders) {
                    if (nearby.tag == "Player") {
                        Rigidbody2D enemyRB = nearby.GetComponent<Rigidbody2D>();
                        //Vector2 direction = (nearby.transform.position - transform.position).normalized;
                        if (enemyRB != null) {
                            enemyRB.AddForce(knockbacks[0]);
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (endLag <= 0) {
            xSpeed = Input.GetAxis("Horizontal") * speed;
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        }
    }

    void LateUpdate()
    {
        if (endLag > 0) {
            endLag -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitboxes[0].position, hitboxSizes[0]);
    }
}
