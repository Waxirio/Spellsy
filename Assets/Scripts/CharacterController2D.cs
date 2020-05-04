using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public bool isGrounded = false;
    public bool isRight = false;
    public bool isLeft = false;
    public float moveSpeed = 5f;
	void Update(){
        //saut
		Jump();
        //ma vitesse en chute
        float velocityFall = 8f;

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        if ((isLeft && movement.x < 0) || (isRight && movement.x > 0)) gameObject.GetComponent<Rigidbody2D>().velocity.= 1f;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody2D>().velocity, velocityFall);
        //si tombe retourne en haut
        Vector3 wrld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        Vector3 sizePlayer = new Vector3(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y, 0.0f);
        if (gameObject.GetComponent<Rigidbody2D>().position.y <= -wrld.y- sizePlayer.y/2)
        {
            Vector3 deplacement = new Vector3(transform.position.x, wrld.y+ sizePlayer.y / 2, transform.position.z);
            transform.position = deplacement;
        }
        //si monte retourn en bas
        /*if (gameObject.GetComponent<Rigidbody2D>().position.y >= wrld.y)
        {
            Vector3 deplacement = new Vector3(transform.position.x, 0, transform.position.z);
            transform.position = deplacement;
        }*/
        //deplacement
        
        if ( ((movement.x > 0 && !isRight) || (movement.x < 0 && !isLeft)) )
        {
           // transform.position += movement * Time.deltaTime * moveSpeed;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * movement.x, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }

    }

	void Jump(){
		if(Input.GetButtonDown("Jump") && isGrounded){
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
		}
        if (Input.GetButtonDown("Jump") && isRight)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-8f, 8f), ForceMode2D.Impulse);
        }
        if (Input.GetButtonDown("Jump") && isLeft)
        {
            Vector3 movement = new Vector3(-1f, 0f, 0f);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(8f, 8f), ForceMode2D.Impulse);
        }
    }
}
