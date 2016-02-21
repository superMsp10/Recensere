using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour
{

		public float speed;		
		public float fly_speed;		

		public float og_speed;		

		public float speedLimitMultiplier;


		public float jumpPower;

		Rigidbody rigidbod;
		public bool jumped = false;
		public bool grounded = false;

		public	Transform start;
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

				grounded = Physics.Linecast (start.position, feets.position, whatGround);

				if (grounded && Input.GetAxis ("Jump") > 0) {
						jump ();

				}
		}

		void jump ()
		{
				rigidbod.AddForce (transform.up * jumpPower);

		}

		void checkMovement ()
		{
				float moveHorizontal = Input.GetAxis ("Horizontal");
	
				float moveVertical = Input.GetAxis ("Vertical");
				if (grounded) {
						speed = og_speed;
				} else
						speed = og_speed * fly_speed;
				if (rigidbod.velocity.magnitude < speed * speedLimitMultiplier)
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
