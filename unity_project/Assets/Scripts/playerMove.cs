using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour
{

		public float speed;
		public float jumpPower;

		Rigidbody rigidbod;
		public bool jumped = false;
		public bool grounded = false;
		public	Transform feets;
		public LayerMask whatGround;

		void Start ()
		{
				rigidbod = GetComponent<Rigidbody> ();
		}

		void FixedUpdate ()
		{
				checkMovement ();
				checkJump ();
		}

		void checkJump ()
		{
				grounded = Physics.Linecast (transform.position, feets.position, whatGround);
				if (grounded && Input.GetAxis ("Jump") > 0) {
						jump ();

				}
		}

		void jump ()
		{
				Debug.Log ("hello");
				rigidbod.AddForce (transform.up * jumpPower);

		}

		void checkMovement ()
		{
				float moveHorizontal = Input.GetAxis ("Horizontal");
	
				float moveVertical = Input.GetAxis ("Vertical");
		
				if (moveVertical > 0)
						rigidbod.AddForce (transform.forward * speed);
				else if (moveVertical < 0)
						rigidbod.AddForce (transform.forward * -speed);
		
				if (moveHorizontal > 0)
						rigidbod.AddForce (transform.right * speed);
				else if (moveHorizontal < 0)
						rigidbod.AddForce (transform.right * -speed);
		}
}
