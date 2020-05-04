using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public bool isGrounded = false;
    public bool isRight = false;
    public bool isLeft = false;
    public float moveSpeed;

    public bool onWallJump = false;
	void Update(){
        //saut
		Jump();

        if(onWallJump){
            Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }
        //ma vitesse en chute
        float velocityFall = 8f;

        //on défini les deplacement du joueur avec les appuis de touche
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        //si on est sur un mur a droite ou a gauche alors on reste accroché
        if (!onWallJump && (isLeft && (movement.x < 0) || (isRight && (movement.x > 0)))){
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x,0f);
        }
        if(onWallJump){
            Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }

        //max speed while falling
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody2D>().velocity, velocityFall);
        
        //***** si le personnage arrive dans la limite basse du terrain il réapparait au dessus *****
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
        /*if (gameObject.GetComponent<Rigidbody2D>().position.y >= wrld.y)
        {
            Vector3 deplacement = new Vector3(transform.position.x, 0, transform.position.z);
            transform.position = deplacement;
        }*/

        //si la vitesse est négative et qu'on saute alors
        //on reprend la main du personnage
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0 && onWallJump){
            onWallJump = false;
        }
        if ( !onWallJump && ( (movement.x > 0) && !isRight) || ((movement.x < 0) && !isLeft))
        {
            //transform.position += movement * Time.deltaTime * moveSpeed;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * movement.x, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }

    }

	void Jump(){
		if(Input.GetButtonDown("Jump") && isGrounded){
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
		}
        if (Input.GetButtonDown("Jump") && (isRight || isLeft))
        {
            if(isRight){
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2f, 5f), ForceMode2D.Impulse);
            }
            else if(isLeft){
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2f, 5f), ForceMode2D.Impulse);
            }
            onWallJump = true;
        }
    }
}
