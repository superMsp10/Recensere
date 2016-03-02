using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour
{
		public float og_speed;		
		public float speed;		
		public float fly_speed;		
		public float speedLimitMultiplier;
	
		public float jumpPower;
		public bool jumped = false;
		public bool grounded = false;
		public double fuel;

		public	Transform start;
		public	Transform feets;
		public LayerMask whatGround;

		public Animator anim;
		Rigidbody rigidbod;
	
	


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

				grounded = (Physics.Linecast (start.position, feets.position, whatGround));

				if (Input.GetAxis ("Jump") > 0) {
						if (grounded && !jumped) {
								jump ();
						} else if (fuel > 0) {
								jetPack ();
						}

				} else {
						rigidbod.useGravity = true;
				}
		}

		void jump ()
		{
				rigidbod.velocity = new Vector3 (rigidbod.velocity.x * fly_speed, jumpPower, rigidbod.velocity.z * fly_speed);

				jumped = true;
		}

		void jetPack ()
		{
				rigidbod.useGravity = false;
		}

		void checkMovement ()
		{
				float moveHorizontal = Input.GetAxis ("Horizontal");
				float moveVertical = Input.GetAxis ("Vertical");

				anim.SetFloat ("XVelo", moveHorizontal);
				anim.SetFloat ("ZVelo", moveVertical);

				if (grounded) {
						speed = og_speed;
						jumped = false;

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
