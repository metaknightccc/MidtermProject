using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour
{
    // Movement
    public float speed = 5;
    public Rigidbody2D rb;
    public Transform feet;
    public LayerMask groundLayer;
    public LayerMask iceLayer;
    public float jumpForce = 250f;
    public float doubleJumpForce = 250f;
    public bool grounded;
    public bool iced;
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
    public float[] moveDamages;
    Vector2 kb;
    Vector2 cstick;
    int recovery = 1;
    int direction = 1;
    int airSideSpecial = 1;
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
        //Sets the blastzones
        if (SceneManager.GetActiveScene().name == "Stage3"){
            blastzoneX = 35f;
            blastzoneCeiling = 10f;
            blastzoneFloor = -10f;
        }
        if (SceneManager.GetActiveScene().name == "Test"){
            blastzoneX = 20f;
            blastzoneCeiling = 10f;
            blastzoneFloor = -8f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        move = controls.Player.Move.ReadValue<Vector2>();
        cstick = controls.Player.RightStickNormal.ReadValue<Vector2>();

        if (grounded || iced) {
            extraJumps = 1;
            recovery = 1;
            airSideSpecial = 1;
        }

        if (endLag <= 0) {
            grounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
            iced = Physics2D.OverlapCircle(feet.position, radius, iceLayer);
            //controls.Player.Jump.ReadValue<float>();
            if (controls.Player.Jump.triggered) {
                if (grounded || iced) {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                } else {
                    if (extraJumps > 0) {
                        extraJumps -= 1;
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.AddForce(new Vector2(rb.velocity.x, doubleJumpForce));
                    }
                }
            }

            if(controls.Player.NormalAttack.triggered) {
                if (grounded || iced) { // Grounded attacks
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
                        endLag = 0.95f;
                        rb.velocity = new Vector2(0, -2);
                        // Add hitboxes onto this
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
                if (grounded || iced) { // Grounded attacks
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
                        endLag = 0.95f;
                        rb.velocity = new Vector2(0, -2);
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
            } else if (controls.Player.SpecialAttack.triggered) {
                if (move.y > .5 && recovery > 0) { // up special
                    Debug.Log("Up Special");
                    endLag = 1.1f;
                    rb.velocity = new Vector2(0, 10);
                    recovery -= 1; //so you can't spam up specials without landing
                } else if (move.y < -.5) { // down special
                    Debug.Log("Down Special");
                    endLag = 0.67f;
                    gameObject.tag = "InvulnerablePlayer";
                    Invoke("MakeVulnerable", 0.1f);
                    StartCoroutine(attackHitbox(0.217f, 12));
                } else if (move.x > .2 && airSideSpecial > 0){ // side special facing right
                    gameObject.transform.localScale = new Vector3(1,1,1);
                    direction = 1;
                    Debug.Log("Side Special right");
                    endLag = 0.9f;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    rb.velocity = new Vector2(6.67f * direction, 0);
                    Invoke("unfreezeY", endLag-0.1f);
                    airSideSpecial -= 1;
                    //Add hitboxes
                } else if (move.x < -.2 & airSideSpecial > 0){ // side special facing left
                    gameObject.transform.localScale = new Vector3(-1,1,1);
                    direction = -1;
                    Debug.Log("Side Special left");
                    endLag = 0.9f;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    rb.velocity = new Vector2(6.67f * direction, 0);
                    Invoke("unfreezeY", endLag-0.1f);
                    airSideSpecial -= 1;
                    //Add hitboxes
                } else { // neutral special
                    Debug.Log("Neutral Special");
                    endLag = 0.78f;
                    StartCoroutine(attackHitbox(0.33f, 9));
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
                HealthSystem enemyHealth = nearby.GetComponent<HealthSystem>();
                if (enemyRB == rb) { // To prevent the player knocking themselves back
                    enemyRB = null;
                }
                if (enemyRB != null) {
                    Debug.Log("Hit");
                    kb = new Vector2(knockbacks[hbIndex].x * direction, knockbacks[hbIndex].y);
                    enemyRB.velocity = new Vector2(0, 0);
                    enemyHealth.Damage(moveDamages[hbIndex]);
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


    void unfreezeY() {
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }


    void FixedUpdate() {
        if (iced == false && grounded == false) {
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
        }        
        if (endLag <= 0 && iced == true) {
            float x = rb.velocity.x + (move.x * (speed/60));
            if(x > 6)
            {
                x = 6;
            }
            else if(x < -6)
            {
                x = -6;
            }
            rb.velocity = new Vector2(x, rb.velocity.y);
            
        }
        if (endLag <= 0 && grounded == true) {
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
        }
        print(move.x);
        if (move.x < -0.2 && (grounded || iced)) { //facing left on ground
        
            gameObject.transform.localScale = new Vector3(-1,1,1);
            direction = -1;
        }
        if (move.x > 0.2 && (grounded || iced)) { //facing right on ground
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
        for (int i = 9; i < hitboxes.Length; i++) {
            Gizmos.DrawWireSphere(hitboxes[i].position, hitboxSizes[i]);
        }
    }
}
