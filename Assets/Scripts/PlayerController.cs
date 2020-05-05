using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool isRight = true;
    private bool isLeft = true;
    private bool isOnJump = true;
    private bool isOnIce = true;
    private bool isOnBouncy = true;
    private bool isOnMud = true;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsJump;
    public LayerMask whatIsIce;
    public LayerMask whatIsBouncy;
    public LayerMask whatIsMud;
    public float speed;
    private float jumpForce;
    public float normalJumpForce;
    private float maxJumpForce;
    public Transform groundCheck;
    public Transform rightCheck;
    public Transform leftCheck;
    public int extraJumpsBase;
    private int extraJumps;

    public float iceSlideSpeed;
    private float iceSpeed;
    private float mudSpeed;

    //init
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // calcul des sauts
        extraJumps = extraJumpsBase;
        jumpForce = normalJumpForce;
        maxJumpForce = normalJumpForce*2;
        iceSpeed = speed + (speed * iceSlideSpeed);
        mudSpeed = speed/3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //regarde la matiere sur laquelle on est
        getMatterOn();
        //Si on va vers la gauche et que l'on appuie vers la droite
        if(facingRight == false && moveInput > 0){
            Flip(); //alors le personnage flip
        } else if(facingRight == true && moveInput < 0){
            Flip(); //alors le personnage flip
        }
        PassHorizontal();
        PassVertical();
    }

    //retourne notre joueur
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void Update()
    {
        //regarder si on appuie a droite ou a gauche
        moveInput = Input.GetAxis("Horizontal");
        ApplyModificator();
        if(isGrounded == true){
            extraJumps = extraJumpsBase;
        }

        if(Input.GetKeyDown("space") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps -= 1;
        }else if(Input.GetKeyDown("space") && extraJumps == 0 && isGrounded == true){ //jump de base
            rb.velocity = new Vector2(0,1) * jumpForce;
        }else if(Input.GetKeyDown("space") && extraJumps == 0 && isLeft == true){ //jump vers la droite
            rb.velocity = new Vector2(1,1) * jumpForce;
        }else if(Input.GetKeyDown("space") && extraJumps == 0 && isRight == true){//jump vers la gauche
            rb.velocity = new Vector2(-1,1)* jumpForce;
        }
    }

    void getMatterOn(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround); //personnage au sol
        isRight = Physics2D.OverlapCircle(rightCheck.position, checkRadius, whatIsGround); //personnage collé a droite
        isLeft = Physics2D.OverlapCircle(leftCheck.position, checkRadius, whatIsGround); //personnage collé a gauche
        isOnJump = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsJump); //personnage sur un saut
        isOnIce = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsIce); //personnage sur de la glace
        isOnBouncy = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsBouncy); //personnage sur une surface rebondissante
        isOnMud = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsMud);
    }

    void ApplyModificator(){
        //si le personnage est sur un saut alors il saute 2* plus haut
        if(isOnJump == true){ jumpForce = maxJumpForce;}
        else{jumpForce = normalJumpForce;}
        //Si le personnage
        if(isOnBouncy == true){
            rb.velocity = new Vector2(0,1) * jumpForce;
        }
        //si le personnage est sur de la glace alors il glisse
        if(isOnIce){
            rb.velocity  = new Vector2(moveInput * iceSpeed, rb.velocity.y);
        }else if(isOnMud){ // si le personnage est dans la boue
            rb.velocity = new Vector2(moveInput * mudSpeed, rb.velocity.y);
        }
        else{
            //on applique la vitesse à notre joueur
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        Debug.Log(rb.velocity);
    }

    void PassHorizontal(){
        //taille du monde
        Vector3 wrld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        //taille du joueur
        Vector3 sizePlayer = new Vector3(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y, 0.0f);
        //si va a droite revient a gauche
        if (gameObject.GetComponent<Rigidbody2D>().position.x >= wrld.x + sizePlayer.x / 2)
        {
            Vector3 deplacement = new Vector3(-wrld.x - sizePlayer.x / 2, transform.position.y, transform.position.z);
            transform.position = deplacement;
        }
        //si va a gauche revient a droite
        if (gameObject.GetComponent<Rigidbody2D>().position.x <= -wrld.x - sizePlayer.x / 2)
        {
            Vector3 deplacement = new Vector3(wrld.x + sizePlayer.x / 2, transform.position.y, transform.position.z);
            transform.position = deplacement;
        }
    }

    void PassVertical(){
        //taille du monde
        Vector3 wrld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f)); 
        //taille du joueur
        Vector3 sizePlayer = new Vector3(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y, 0.0f);
        //Si le personnage disparait en bas il réapparait en haut
        if (gameObject.GetComponent<Rigidbody2D>().position.y <= -wrld.y- sizePlayer.y/2)
        {
            Vector3 deplacement = new Vector3(transform.position.x, wrld.y+ sizePlayer.y / 2, transform.position.z);
            transform.position = deplacement;
        }
        //si monte retourne en bas
        if (gameObject.GetComponent<Rigidbody2D>().position.y >= wrld.y+ sizePlayer.y / 2)
        {
            Vector3 deplacement = new Vector3(transform.position.x, -wrld.y - sizePlayer.y / 2, transform.position.z);
            transform.position = deplacement;
        }
    }
}
