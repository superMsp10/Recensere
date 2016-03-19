using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerMove : MonoBehaviour
{
		//Movement
		public float og_speed;
		public float speed;
		public float fly_speed;
		public float speedLimitMultiplier;
		public	Transform start;
		public	Transform feets;
		public LayerMask whatGround;
	
		//Jumping
		public float jumpPower;
		public bool jumped;
		public bool grounded = false;

		//Jetpack
		public float _fuel;
		public float maxFuel;
		public JetpackFuelDisplay jetPackFuelDisplay;
		public ParticleSystem airIntake;
		public ParticleSystem afterburn;
		bool usingJetPack = false;

		bool fuelCountdown = false;
		Slider fuelSlider;
		GameObject jetpackUIFire;

		public Animator anim;
		Rigidbody rigidbod;


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

		public void Start ()
		{
				rigidbod = GetComponent<Rigidbody> ();
				fuelSlider = tileDictionary.thisM.JetpackFuel;
				jetpackUIFire = tileDictionary.thisM.JetpackFire;
				fuel = maxFuel;
				fuelSlider.maxValue = maxFuel;
				fuelSlider.value = _fuel;
				jetpackUIFire.SetActive (false);
				airIntake.Stop ();
				afterburn.Stop ();

		
		}

		void FixedUpdate ()
		{
				checkMovement ();
				checkJump ();

		}


		void checkJump ()
		{
		
				grounded = (Physics.Linecast (start.position, feets.position, whatGround));

				if (!jumped) {
						if (Input.GetKeyDown (KeyCode.Space)) {
								if (grounded)
										jump ();
								else
										jumped = true;
						}
				} else if (Input.GetKey (KeyCode.Space) && _fuel > 0) {
						if (!usingJetPack)
								jetPack ();

				} else if (usingJetPack)
						stopJetPack ();

				if (grounded && !Input.GetKey (KeyCode.Space) && usingJetPack)
						stopJetPack ();

				if (fuel <= 0 && usingJetPack)
						stopJetPack ();

		}

		void stopJetPack ()
		{
				usingJetPack = false;
				rigidbod.useGravity = true;
				CancelInvoke ("updateFuel");
				fuelCountdown = false;
				jetpackUIFire.SetActive (false);
				airIntake.Stop ();
				afterburn.Stop ();
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
				usingJetPack = true;
				rigidbod.useGravity = false;
				jetpackUIFire.SetActive (true);

				if (!fuelCountdown) {
						InvokeRepeating ("updateFuel", 0, 1.0f);
						fuelCountdown = true;
				}
				airIntake.Play ();
				afterburn.Play ();
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
