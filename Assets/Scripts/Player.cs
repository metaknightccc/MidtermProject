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
    float xSpeed;
    float ySpeed;

    // Combat
    [SerializeField]
    private float endLag;
    public float hitStun;
    public Transform[] hitboxes;
    public float[] hitboxSizes;
    public Vector2[] knockbacks;
    public float[] moveDamages;
    Vector2 kb;
    Vector2 cstick;
    float multiplier;
    int direction = 1;
    public float blastzoneX = 20f;
    public float blastzoneCeiling = 20f;
    public float blastzoneFloor = -10f;
    public GameObject specialProjectile;
    public float specialForce;
    public GameObject downAirProjectile;
    public float downAirForce;
    public HealthSystem stocks;
    public ManageGame gameManager;
    public Vector2 respawnPoint = new Vector2(0, 5);

    // Animation
    public Animator playerAnimator;
    bool didDoubleJump;
    Vector2 inputVector;

    public int playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the blastzones
        if (SceneManager.GetActiveScene().name == "Stage1"){
            blastzoneX = 20f;
            blastzoneCeiling = 10f;
            blastzoneFloor = -8f;
        }
        if (SceneManager.GetActiveScene().name == "Stage2"){
            blastzoneX = 20f;
            blastzoneCeiling = 20f;
            blastzoneFloor = -10f;
        }
        if (SceneManager.GetActiveScene().name == "Stage3"){
            blastzoneX = 35f;
            blastzoneCeiling = 40f;
            blastzoneFloor = -10f;
        }
        if (SceneManager.GetActiveScene().name == "Stage4"){
            blastzoneX = 16f;
            blastzoneCeiling = 25f;
            blastzoneFloor = -20f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
        iced = Physics2D.OverlapCircle(feet.position, radius, iceLayer);

        if (grounded || iced) {
            extraJumps = extraJump; 
            //recovery = 1;
            //airSideSpecial = 1;
        }
        
        if (endLag <= 0 && hitStun <= 0) {
            if (Input.GetButtonDown("Jump"+playerIndex)) {
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
            
            if(Input.GetButtonDown("Attack"+playerIndex)) {
                if (grounded || iced) { // Grounded attacks
                    if (Input.GetAxis("Vertical"+playerIndex)>0.5f) {
                        Debug.Log("Up tilt");
                        playerAnimator.SetTrigger("upAttack");
                        endLag = 0.15f;
                        StartCoroutine(attackHitbox(0.066f, 2, 0.5f));
                    } else if (Input.GetAxis("Vertical"+playerIndex)<-0.5f) {
                        Debug.Log("Down tilt");
                        playerAnimator.SetTrigger("downAttack");
                        endLag = 0.15f;
                        StartCoroutine(attackHitbox(0.066f, 1, 0.5f));
                    } else if (Input.GetAxis("Horizontal"+playerIndex)>0.2f || Input.GetAxis("Horizontal"+playerIndex)<-0.2f) {
                        Debug.Log("Side tilt");
                        playerAnimator.SetTrigger("sideAttack");
                        endLag = 0.15f;
                        StartCoroutine(attackHitbox(0.083f, 3, 0.75f));
                    } else {
                        endLag = 0.15f;
                        Debug.Log("Jab");
                        playerAnimator.SetTrigger("neutralAttack");
                        StartCoroutine(attackHitbox(0.033f, 0, 0.5f));
                    }
                } else { // Aerial attacks
                    if (Input.GetAxis("Vertical"+playerIndex)>0.5f) {
                        Debug.Log("Up Air");
                        playerAnimator.SetTrigger("upAttack");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.067f, 6, 1.25f));
                    } else if (Input.GetAxis("Vertical"+playerIndex)<-0.5f) {
                        Debug.Log("Down Air");
                        endLag = 0.583f;
                        playerAnimator.SetTrigger("downAir");
                        GameObject newMelon = Instantiate(downAirProjectile, hitboxes[7].position, Quaternion.identity);
                        newMelon.GetComponent<Rigidbody2D>().AddForce(transform.up * -1 * downAirForce);
                        // Add hitboxes onto this
                    } else if (Input.GetAxis("Horizontal"+playerIndex) * direction > 0.2f) {
                        Debug.Log("Forward Air");
                        playerAnimator.SetTrigger("sideAttack");
                        endLag = 0.25f;
                        StartCoroutine(attackHitbox(0.083f, 5, 1.25f));
                    } else if (Input.GetAxis("Horizontal"+playerIndex) * direction < -0.2f) {
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
            } else if (Input.GetButtonDown("Special"+playerIndex)) {
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
                Player hitPlayers = nearby.GetComponent<Player>();
                if (enemyRB == rb) { // To prevent the player knocking themselves back
                    enemyRB = null;
                }
                if (enemyRB != null) {
                    float enemyPercent = nearby.GetComponent<HealthSystem>().rate;
                    Debug.Log("Hit");
                    kb = new Vector2(knockbacks[hbIndex].x * direction, knockbacks[hbIndex].y);
                    //enemyRB.velocity = new Vector2(0, 0);
                    multiplier = 1f + (enemyPercent * kbMulti / 5f);
                    enemyHealth.Damage(moveDamages[hbIndex]);
                    enemyRB.AddForce(kb * multiplier);
                    hitPlayers.hitStun = 0.4f * kbMulti;
                }
            }
        }
    }

    void Respawn() {
        stocks.LoseOneHeart();
        gameObject.transform.position = respawnPoint;
        if (stocks.life > 0){
            gameObject.SetActive(true);
            gameObject.tag = "InvulnerablePlayer";
            Invoke("MakeVulnerable", 3f);
        } else {
            gameManager.deadCount += 1;
        }
    }

    void MakeVulnerable() {
        gameObject.tag = "Player";
    }
    
    void setFalse() {
        didDoubleJump = false;
    }

    void FixedUpdate() {
        xSpeed = Input.GetAxis("Horizontal"+playerIndex) * speed;
        if (iced == false && grounded == false && hitStun <= 0) {
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        } 
        if (endLag <= 0 && iced == true && hitStun <= 0) {
            float x = rb.velocity.x + (xSpeed/60);
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
        if (endLag <= 0 && grounded == true && hitStun <= 0) {
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        }
        if (Input.GetAxis("Horizontal"+playerIndex)< -0.2 && (grounded || iced)) { //facing left on ground
        
            gameObject.transform.localScale = new Vector3(-1,1,1);
            direction = -1;
        }
        if (Input.GetAxis("Horizontal"+playerIndex) > 0.2 && (grounded || iced)) { //facing right on ground
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
        playerAnimator.SetFloat("hitStun", hitStun);
    }

    void LateUpdate()
    {
        if (endLag > 0) {
            endLag -= Time.deltaTime;
        }
        if (hitStun > 0) {
            hitStun -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < 4; i++) {
            Gizmos.DrawWireSphere(hitboxes[i].position, hitboxSizes[i]);
        }
    }
}
