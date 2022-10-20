using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement
    public int speed = 5;
    public Rigidbody2D rb;
    public Transform feet;
    public LayerMask groundLayer;
    public float jumpForce = 500f;
    public float doubleJumpForce = 250f;
    public bool grounded;
    public int extraJumps = 1;
    public float radius = 0.3f;

    // New Input System
    private PlayerControls controls;
    Vector2 move;

    // Combat
    public double endLag;
    public Transform[] hitboxes;
    public float[] hitboxSizes;
    public Vector2[] knockbacks;

    private void Awake() {
        controls = new PlayerControls();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endLag <= 0) {
            move = controls.Player.Move.ReadValue<Vector2>();
            grounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
            //controls.Player.Jump.ReadValue<float>();
            if (controls.Player.Jump.triggered) {
                if (grounded) {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                } else {
                    if (extraJumps > 0) {
                        extraJumps -= 1;
                        rb.AddForce(new Vector2(rb.velocity.x, doubleJumpForce));
                    }
                }
            }
            if (grounded) {
                extraJumps = 1;
            }

            if(controls.Player.NormalAttack.triggered && grounded) {
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

    void FixedUpdate() {
        if (endLag <= 0) {
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
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
