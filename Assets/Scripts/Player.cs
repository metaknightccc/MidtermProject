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
    public float jumpForce = 250f;
    public float doubleJumpForce = 250f;
    public bool grounded;
    public int extraJumps = 1;
    public float radius = 0.3f;

    // New Input System
    private PlayerControls controls;
    Vector2 move;

    // Combat
    public float endLag;
    public Transform[] hitboxes;
    public float[] hitboxSizes;
    public Vector2[] knockbacks;
    Vector2 kb;
    Vector2 cstick;
    int direction;
    public float blastzoneX = 20f;
    public float blastzoneCeiling = 20f;
    public float blastzoneFloor = -10f;

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
        move = controls.Player.Move.ReadValue<Vector2>();
        cstick = controls.Player.RightStickNormal.ReadValue<Vector2>();

        if (endLag <= 0) {
            grounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
            //controls.Player.Jump.ReadValue<float>();
            if (controls.Player.Jump.triggered) {
                if (grounded) {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                } else {
                    if (extraJumps > 0) {
                        extraJumps -= 1;
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.AddForce(new Vector2(rb.velocity.x, doubleJumpForce));
                    }
                }
            }
            if (grounded) {
                extraJumps = 1;
            }

            if(controls.Player.NormalAttack.triggered) {
                if (grounded) { // Grounded attacks
                    if (move.y > .5) {
                        Debug.Log("Up tilt");
                        endLag = 0.5f;
                        StartCoroutine(attackHitbox(0.15f, 2));
                    } else if (move.y < -.5) {
                        Debug.Log("Down tilt");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.117f, 1));
                    } else if (move.x > .2 || move.x < -.2) {
                        Debug.Log("Side tilt");
                        endLag = 0.65f;
                        StartCoroutine(attackHitbox(0.217f, 3));
                    } else {
                        endLag = 0.5f;
                        Debug.Log("Jab");
                        // The following program is exclusive to this character's jab only
                        gameObject.tag = "InvulnerablePlayer";
                        rb.AddForce(new Vector2(100 * direction, 0));
                        Invoke("MakeVulnerable", 0.2f);
                        // The above 3 lines is inspired by electric wind god fist

                        StartCoroutine(attackHitbox(0.125f, 0));
                    }
                } else { // Aerial attacks
                    if (move.y > .5) {
                        Debug.Log("Up Air");
                        endLag = 0.55f;
                        StartCoroutine(attackHitbox(0.067f, 7));
                    } else if (move.y < -.5) {
                        Debug.Log("Down Air");
                    } else if (move.x * direction > 0.2) {
                        Debug.Log("Forward Air");
                        endLag = 0.5f;
                        StartCoroutine(attackHitbox(0.133f, 5));
                    } else if (move.x * direction < -0.2) {
                        Debug.Log("Back Air");
                        endLag = 0.75f;
                        StartCoroutine(attackHitbox(0.183f, 6));
                    } else {
                        Debug.Log("Neutral Air");
                        endLag = 0.467f;
                        StartCoroutine(attackHitbox(0.133f, 4));
                    }
                }
            } else if (controls.Player.RightStickNormal.triggered) {
                // Control stick alternatively inputting moves
                // Cannot input neutral attacks (jab, neutral air)
                // since there is no neutral value for sticks
                if (grounded) { // Grounded attacks
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    if (cstick.y > .5) { // Up tilt
                        endLag = 0.5f;
                        Debug.Log("Up tilt");
                        StartCoroutine(attackHitbox(0.15f, 2));
                    } else if (cstick.y < -.5) { // Down tilt
                        endLag = 0.25f;
                        Debug.Log("Down tilt");
                        StartCoroutine(attackHitbox(0.117f, 1));
                    } else if (cstick.x > 0.5) { // Side tilt facing right
                        gameObject.transform.localScale = new Vector3(1,1,1);
                        direction = 1;
                        endLag = 0.65f;
                        Debug.Log("Side tilt right");
                        StartCoroutine(attackHitbox(0.217f, 3));
                    } else if (cstick.x < -0.5){ // Side stick facing left
                        gameObject.transform.localScale = new Vector3(-1,1,1);
                        direction = -1;
                        endLag = 0.65f;
                        Debug.Log("Side tilt left");
                        StartCoroutine(attackHitbox(0.217f, 3));
                    }
                } else { // Aerial attacks
                    if (cstick.y > .5) { // Up Air
                        Debug.Log("Up Air");
                        endLag = 0.55f;
                        StartCoroutine(attackHitbox(0.067f, 7));
                    } else if (cstick.y < -.5) { // Down Air
                        Debug.Log("Down Air");
                    } else if (cstick.x * direction > 0.5) { // Forward Air
                        Debug.Log("Forward Air");
                        endLag = 0.5f;
                        StartCoroutine(attackHitbox(0.133f, 5));
                    } else if (cstick.x * direction < -0.5) { // Back Air
                        Debug.Log("Back Air");
                        endLag = 0.75f;
                        StartCoroutine(attackHitbox(0.183f, 6));
                    }
                }
            }
        }
    }

    IEnumerator attackHitbox(float startup, int hbIndex) {
        yield return new WaitForSeconds(startup);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(hitboxes[hbIndex].position, hitboxSizes[hbIndex]);
        foreach (Collider2D nearby in colliders) {
            // Add check for self hitbox
            if (nearby.tag == "Player") {
                Rigidbody2D enemyRB = nearby.GetComponent<Rigidbody2D>();
                if (enemyRB == rb) { // To prevent the player knocking themselves back
                    enemyRB = null;
                }
                if (enemyRB != null) {
                    Debug.Log("Hit");
                    kb = new Vector2(knockbacks[hbIndex].x * direction, knockbacks[hbIndex].y);
                    enemyRB.velocity = new Vector2(0, 0);
                    
                    enemyRB.AddForce(kb);
                }
            }
        }
    }


    void Respawn() {
        // Lose a stock
        // Percent = 0
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector2(0, 5);
        gameObject.tag = "InvulnerablePlayer";
        Invoke("MakeVulnerable", 3f);
    }


    void MakeVulnerable() {
        gameObject.tag = "Player";
    }


    void FixedUpdate() {
        if (endLag <= 0 || grounded == false) {
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
        }
        if (move.x < -0.2 && grounded) { //facing left on ground
            gameObject.transform.localScale = new Vector3(-1,1,1);
            direction = -1;
        }
        if (move.x > 0.2 && grounded) { //facing right on ground
            gameObject.transform.localScale = new Vector3(1,1,1);
            direction = 1;
        }
        if (gameObject.transform.position.x > blastzoneX || gameObject.transform.position.x < -1 * blastzoneX) {
            gameObject.SetActive(false);
            Invoke("Respawn", 1f);
        }
        if (gameObject.transform.position.y < blastzoneFloor){
            gameObject.SetActive(false);
            Invoke("Respawn", 1f);
        }
        if (gameObject.transform.position.y > blastzoneCeiling){
            gameObject.SetActive(false);
            Invoke("Respawn", 1f);
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
        for (int i = 0; i < hitboxes.Length; i++) {
            Gizmos.DrawWireSphere(hitboxes[i].position, hitboxSizes[i]);
        }
    }
}
