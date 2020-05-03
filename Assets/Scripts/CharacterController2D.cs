using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public bool isGrounded = false;
	public float moveSpeed = 5f;
	void Update(){
		Jump();
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
		transform.position += movement * Time.deltaTime * moveSpeed;
	}

	void Jump(){
		if(Input.GetButtonDown("Jump") && isGrounded){
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
		}
	}
}
