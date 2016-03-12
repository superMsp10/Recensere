using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerMove : MonoBehaviour
{
		public float og_speed;		
		public float speed;		
		public float fly_speed;		
		public float speedLimitMultiplier;
	
		public float jumpPower;
		public bool jumped;
		public bool grounded = false;
		public float _fuel;
		public float maxFuel;
		bool fuelCountdown = false;
		public JetpackFuelDisplay jetPackFuelDisplay;
	
		public	Transform start;
		public	Transform feets;
		public LayerMask whatGround;

		public Animator anim;
		Rigidbody rigidbod;
		Slider fuelSlider ;
		GameObject fire ;


		public float fuel {
				get {
						return _fuel; 
				}
				set {
						_fuel = value; 
						fuelSlider.value = value;
						jetPackFuelDisplay.setFuel (value / maxFuel);
				}
		}
		void Start ()
		{
				rigidbod = GetComponent<Rigidbody> ();
				fuelSlider = tileDictionary.thisM.JetpackFuel;
				fire = tileDictionary.thisM.JetpackFire;
				fuelSlider.maxValue = maxFuel;
				fuelSlider.value = _fuel;
				fire.SetActive (false);


		}

		void FixedUpdate ()
		{
				checkMovement ();
				checkJump ();

		}


		void checkJump ()
		{
		
				grounded = (Physics.Linecast (start.position, feets.position, whatGround));

				if (Input.GetKey (KeyCode.Space)) {
						if (!jumped) {
								if (Input.GetKeyDown (KeyCode.Space)) {
										if (grounded)
												jump ();
										else
												jumped = true;
								}
						} else if (_fuel > 0) {
								jetPack ();
						} else {
								stopJetPack ();
						}
				} else {
						stopJetPack ();

				}
				
		}

		void stopJetPack ()
		{
				rigidbod.useGravity = true;
				CancelInvoke ("updateFuel");
				fuelCountdown = false;
				fire.SetActive (false);
		}

		void updateFuel ()
		{
				fuel--;
		}

		void jump ()
		{
				rigidbod.velocity = new Vector3 (rigidbod.velocity.x * fly_speed, jumpPower, rigidbod.velocity.z * fly_speed);
				jumped = true;

		}

		void jetPack ()
		{
				rigidbod.useGravity = false;
				fire.SetActive (true);

				if (!fuelCountdown) {
						InvokeRepeating ("updateFuel", 0, 1.0f);
						fuelCountdown = true;
				}

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
