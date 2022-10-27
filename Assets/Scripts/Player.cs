using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    public int extraJump = 5; //changes per character
    public float radius = 0.3f;

    // New Input System
    public PlayerControls controls;
    private PlayerInput playerInput;
    private InputAction jumpAction;
    Vector2 move;

    // Combat
    public float endLag;
    public Transform[] hitboxes;
    public float[] hitboxSizes;
    public Vector2[] knockbacks;
    public float[] moveDamages;
    Vector2 kb;
    Vector2 cstick;
    float multiplier;
    //int recovery = 1;
    int direction = 1;
    //int airSideSpecial = 1;
    public float blastzoneX = 20f;
    public float blastzoneCeiling = 20f;
    public float blastzoneFloor = -10f;
    public GameObject specialProjectile;
    public float specialForce;
    public GameObject downAirProjectile;
    public float downAirForce;

    // Animation
    public Animator playerAnimator;
    bool didDoubleJump;
    Vector2 inputVector;

    public int playerIndex;

    /*
    private void Awake() {
        controls = new PlayerControls();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
    */

    /*
    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
    }
    */

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
        grounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
        iced = Physics2D.OverlapCircle(feet.position, radius, iceLayer);

        if (grounded || iced) {
            extraJumps = extraJump; 
            //recovery = 1;
            //airSideSpecial = 1;
        }
        
        if (endLag <= 0) {
            //controls.Player.Jump.ReadValue<float>();
            if (controls.Player.Jump.triggered) {
                if (grounded || iced) {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                } else {
                    if (extraJumps > 0) {
                        extraJumps -= 1;
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.AddForce(new Vector2(rb.velocity.x, doubleJumpForce));
                        didDoubleJump = true;
                        Invoke("setFalse", 0.5f);
                    }
                }
            }

            if(controls.Player.NormalAttack.triggered) {
                if (grounded || iced) { // Grounded attacks
                    if (move.y > .5) {
                        Debug.Log("Up tilt");
                        playerAnimator.SetTrigger("upAttack");
                        endLag = 0.15f;
                        StartCoroutine(attackHitbox(0.066f, 2, 0.5f));
                    } else if (move.y < -.5) {
                        Debug.Log("Down tilt");
                        playerAnimator.SetTrigger("downAttack");
                        endLag = 0.15f;
                        StartCoroutine(attackHitbox(0.066f, 1, 0));
                    } else if (move.x > .2 || move.x < -.2) {
                        Debug.Log("Side tilt");
                        playerAnimator.SetTrigger("sideAttack");
                        endLag = 0.15f;
                        StartCoroutine(attackHitbox(0.083f, 3, 0.75f));
                    } else {
                        endLag = 0.15f;
                        Debug.Log("Jab");
                        playerAnimator.SetTrigger("neutralAttack");
                        // The following program is exclusive to this character's jab only
                        //gameObject.tag = "InvulnerablePlayer";
                        //rb.AddForce(new Vector2(100 * direction, 0));
                        //Invoke("MakeVulnerable", 0.2f);
                        // The above 3 lines is inspired by electric wind god fist
                        StartCoroutine(attackHitbox(0.033f, 0, 0.5f));
                    }
                } else { // Aerial attacks
                    if (move.y > .5) {
                        Debug.Log("Up Air");
                        playerAnimator.SetTrigger("upAttack");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.067f, 6, 1.25f));
                    } else if (move.y < -.5) {
                        Debug.Log("Down Air");
                        endLag = 0.583f;
                        playerAnimator.SetTrigger("downAir");
                        GameObject newMelon = Instantiate(downAirProjectile, hitboxes[7].position, Quaternion.identity);
                        newMelon.GetComponent<Rigidbody2D>().AddForce(transform.up * -1 * downAirForce);
                        // Add hitboxes onto this
                    } else if (move.x * direction > 0.2) {
                        Debug.Log("Forward Air");
                        playerAnimator.SetTrigger("sideAttack");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.083f, 5, 1.25f));
                    } else if (move.x * direction < -0.2) {
                        direction *= -1;
                        Debug.Log("Back Air");
                        playerAnimator.SetTrigger("sideAttack");
                        gameObject.transform.localScale = new Vector3(direction,1,1);
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.083f, 5, 1.25f));
                    } else {
                        Debug.Log("Neutral Air");
                        playerAnimator.SetTrigger("neutralAttack");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.033f, 4, 1.25f));
                    }
                }
            } else if (controls.Player.RightStickNormal.triggered) {
                // Control stick alternatively inputting moves
                // Cannot input neutral attacks (jab, neutral air)
                // since there is no neutral value for sticks
                if (grounded || iced) { // Grounded attacks
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    if (cstick.y > .5) { // Up tilt
                        endLag = 0.15f;
                        Debug.Log("Up tilt");
                        playerAnimator.SetTrigger("upAttack");
                        StartCoroutine(attackHitbox(0.067f, 2, 0.5f));
                    } else if (cstick.y < -.5) { // Down tilt
                        endLag = 0.15f;
                        Debug.Log("Down tilt");
                        playerAnimator.SetTrigger("downAttack");
                        StartCoroutine(attackHitbox(0.067f, 1, 0));
                    } else if (cstick.x > 0.5) { // Side tilt facing right
                        gameObject.transform.localScale = new Vector3(1,1,1);
                        direction = 1;
                        endLag = 0.15f;
                        Debug.Log("Side tilt right");
                        playerAnimator.SetTrigger("sideAttack");
                        StartCoroutine(attackHitbox(0.083f, 3, 0.75f));
                    } else if (cstick.x < -0.5){ // Side stick facing left
                        gameObject.transform.localScale = new Vector3(-1,1,1);
                        direction = -1;
                        endLag = 0.15f;
                        playerAnimator.SetTrigger("sideAttack");
                        Debug.Log("Side tilt left");
                        StartCoroutine(attackHitbox(0.083f, 3, 0.75f));
                    }
                } else { // Aerial attacks
                    if (cstick.y > .5) { // Up Air
                        Debug.Log("Up Air");
                        playerAnimator.SetTrigger("upAttack");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.067f, 6, 1.25f));
                    } else if (cstick.y < -.5) { // Down Air
                        Debug.Log("Down Air");
                        endLag = 0.583f;
                        playerAnimator.SetTrigger("downAir");
                        GameObject newMelon = Instantiate(downAirProjectile, hitboxes[7].position, Quaternion.identity);
                        newMelon.GetComponent<Rigidbody2D>().AddForce(transform.up * -1 * downAirForce);
                    } else if (cstick.x * direction > 0.5) { // Forward Air
                        Debug.Log("Forward Air");
                        playerAnimator.SetTrigger("sideAttack");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.083f, 5, 1.25f));
                    } else if (cstick.x * direction < -0.5) { // Back Air
                        direction *= -1;
                        Debug.Log("Back Air");
                        playerAnimator.SetTrigger("sideAttack");
                        gameObject.transform.localScale = new Vector3(direction,1,1);
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.083f, 5, 1.25f));
                    }
                } 
            } else if (controls.Player.SpecialAttack.triggered) {
                Debug.Log("Neutral Special");
                endLag = 0.3f;
                playerAnimator.SetTrigger("special");
                GameObject newStar = Instantiate(specialProjectile, hitboxes[8].position, Quaternion.identity);
                newStar.GetComponent<Rigidbody2D>().AddForce(transform.right * direction * specialForce);
            }
        }
    }

    IEnumerator attackHitbox(float startup, int hbIndex, float kbMulti) {
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
                    float enemyPercent = nearby.GetComponent<HealthSystem>().rate;
                    Debug.Log("Hit");
                    kb = new Vector2(knockbacks[hbIndex].x * direction, knockbacks[hbIndex].y);
                    enemyRB.velocity = new Vector2(0, 0);
                    multiplier = 1 + (enemyPercent * kbMulti / 50);
                    enemyHealth.Damage(moveDamages[hbIndex]);
                    enemyRB.AddForce(kb * multiplier);
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

    
    void setFalse() {
        didDoubleJump = false;
    }


    void unfreezeY() {
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }


    void FixedUpdate() {
        if (iced == false && grounded == false) {
            rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
            //rb.velocity = new Vector2(inputVector.x * speed * Time.deltaTime, rb.velocity.y);
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
            //rb.velocity = new Vector2(inputVector.x * speed * Time.deltaTime, rb.velocity.y);
        }
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

        playerAnimator.SetFloat("Speed", rb.velocity.x * direction);
        playerAnimator.SetBool("isGrounded", grounded);
        playerAnimator.SetBool("doubleJump", didDoubleJump);
    }


    void LateUpdate()
    {
        if (endLag > 0) {
            endLag -= Time.deltaTime;
        }
    }


    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < 4; i++) {
            Gizmos.DrawWireSphere(hitboxes[i].position, hitboxSizes[i]);
        }
    }

    /*
    public void Move() {

    }

    public void Jump() {

    }

    public void NormalAttack() {

    }

    public void SpecialAttack() {

    }
    */
}
