using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public bool isGrounded = false;
    public bool isRight = false;
    public bool isLeft = false;
    public float moveSpeed;

    float jumpForce = 5f;
    public bool onWallJump = false;
	void Update(){
        //saut
		Jump();

        //ma vitesse en chute
        float velocityFall = 8f;

        //on défini les deplacement du joueur avec les appuis de touche
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        //si on est sur un mur a droite ou a gauche alors on reste accroché
        // if (!onWallJump && (isLeft && (movement.x < 0) || (isRight && (movement.x > 0)))){
        //     gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x,0f);
        // }

        //vitesse de chute max
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody2D>().velocity, velocityFall);
        
        //***** si le personnage arrive dans la limite du terrain il réapparait de l'autre coté *****
        PassVertical();
        PassHorizontal();

        //si la vitesse est négative et qu'on saute alors
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0 && onWallJump){
            onWallJump = false;
        }
        if ( !onWallJump && ( (movement.x > 0) && !isRight) || ((movement.x < 0) && !isLeft))
        {
            //transform.position += movement * Time.deltaTime * moveSpeed;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * movement.x, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }

    }

    void PassHorizontal(){
        //si monte retourne en bas
        /*if (gameObject.GetComponent<Rigidbody2D>().position.y >= wrld.y)
        {
            Vector3 deplacement = new Vector3(transform.position.x, 0, transform.position.z);
            transform.position = deplacement;
        }*/
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
    }

	void Jump(){
		if(Input.GetButtonDown("Jump") && isGrounded){
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}
        else if (Input.GetButtonDown("Jump") && (isRight || isLeft) && !isGrounded)
        {
            if(isRight){
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2f, jumpForce), ForceMode2D.Impulse);
            }
            else if(isLeft){
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2f, jumpForce), ForceMode2D.Impulse);
            }
            onWallJump = true;
        }
    }
}
