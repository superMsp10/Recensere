using  UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class mobAtributes
{
		public float HP;
		public float maxHP;

		public int Dmg;
		public float maxSped = 10f;
		public float moveForce = 5;
		public int jump = 100;
		public bool moving = false;
		public bool teleports = true;
		public int optTargetRange = 30;
		public float  flyAmount = 0.5f;

	
	
	
	
	
}

public class Mob1 : Entity
{
		public mobAtributes thisAttributes = new mobAtributes ();

		//---------------------------------------
		protected bool landed = false;
		public bool grounded = false;

		public bool turnR = false;
		protected bool front = true;

		//---------------------------------------
		public Transform[] groundCheck;
		public Vector2 centerOfMass;
		public LayerMask whatGround;
		public Animator thisAnim;
		public Transform frontBody;
		public Transform sideBody;
		public bool animated = false;
		public bool detectedFacing = false;
//		public AudioSource thisAudio;
		public AudioClip  jumpClip;
		public AudioClip  stepClip;
		public AudioClip  landClip;
		public AudioClip  dmgClip;
		public AudioClip  movingClip;
		public Slider healthbar;
		public bool falling;
		public GameObject weaponHand;
		public bool attacking;
		public GameObject attackArea;





		void Start ()
		{
				
				thisManage = GameManager.thisM;
				checkNecesseries ();
//				thisLevel.addEntity (this);
				GetComponent<Rigidbody2D> ().centerOfMass = centerOfMass;
				thisAnim = GetComponent<Animator> ();
//				thisAudio = AudioManager.thisAM.playerFX;

		}

//		public override void changeLevel (level lev)
//		{
//				if (lev == null)
//						Debug.LogError ("transfering level is null: " + gameObject.name);
//				thisLevel.removeEntity (this);
//				thisLevel = lev;
//				thisLevel.addEntity (this);
//				checkNecesseries ();
//
//
//
//		}

		protected virtual void checkNecesseries ()
		{

			
				
				if (thisAnim == null && animated) {
						Debug.LogError ("no animator is provided");
				}
				if (groundCheck == null)
						Debug.Log ("no feets included");
				
				if (healthbar != null) {
						healthbar.maxValue = thisAttributes.maxHP;
						healthbar.value = thisAttributes.HP;
				} else {

						Debug.Log ("no health bar provided");
				}
		}
		
		void Update ()
		{

				if (animated)
						updateAnim ();
				checkDead ();
				checkFacing ();
		}

		protected virtual void checkDead ()
		{
				if (thisAttributes.HP <= 0)
						die ();
//				if (transform.position.y < thisLevel.deathHeight)
//						thisAttributes.HP = 0;
		}

		protected void checkFacing ()
		{

				if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x) > 1) {
						thisAttributes.moving = true;
			
				} else if (attacking) {
						thisAttributes.moving = true;
				} else {
						thisAttributes.moving = false;
			
				}
				if (detectedFacing) {
						if (thisAttributes.moving || attacking) {
								front = false;
								frontBody.gameObject.SetActive (false);
								sideBody.gameObject.SetActive (true);
						} else {
								front = true;

								sideBody.gameObject.SetActive (false);
								frontBody.gameObject.SetActive (true);
						}
						if (GetComponent<Rigidbody2D> ().velocity.y < -20 && !attacking) {
				
								falling = true;
								front = true;
				
								sideBody.gameObject.SetActive (false);
								frontBody.gameObject.SetActive (true);
						}
				}
				
		}
		
		public virtual void stepSound ()
		{
		
//				thisAudio.PlayOneShot (stepClip);

		}

		public virtual void playJumpSound ()
		{
//				if (thisAudio == null) {
//			
////						thisAudio = AudioManager.thisAM.playerFX;
//				}
//				thisAudio.PlayOneShot (jumpClip);
		
		}
		public virtual void playLandSound ()
		{
//				if (thisAudio == null) {
//			
////						thisAudio = AudioManager.thisAM.playerFX;
//				}
//				thisAudio.PlayOneShot (landClip);
//		
		}
		public virtual void playMoveSound ()
		{
		
//				thisAudio.PlayOneShot (movingClip, 0.5f);
		
		}

		public virtual void die ()
		{

				Destroy (gameObject);

		}


		protected virtual void checkground ()
		{
				int yGround = 0;
				int nGround = 0;
				foreach (Transform t in groundCheck) {
						if (Physics2D.Linecast (transform.position, t.position, whatGround))
								yGround++;
						else
								nGround ++;
				}
				bool ground;
				if (yGround > nGround)
						ground = true;
				else
						ground = false;
		
				if (!grounded && ground) {
						landed = true;
						grounded = true;
						falling = false;
			
						playLandSound ();
			
				} else if (!ground) {
						grounded = false;
						landed = false;
			
			
				} else {
						landed = false;
				}
		}
		void FixedUpdate ()
		{
				checkground ();
		}

		public virtual void updateAnim ()
		{
				thisAnim.SetBool ("grounded", grounded);
				thisAnim.SetFloat ("vSpeed", GetComponent<Rigidbody2D> ().velocity.y);
				thisAnim.SetFloat ("hSpeed", Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x));
				thisAnim.SetBool ("Attack", attacking);
				thisAnim.SetBool ("moving", thisAttributes.moving);


		}

		public virtual void takeDmg (float damage)
		{
				thisAttributes.HP -= damage;
//				if (thisAudio == null) {
//			
//						thisAudio = AudioManager.thisAM.playerFX;
//				}
//				thisAudio.PlayOneShot (dmgClip);
				healthbar.value = thisAttributes.HP;
		}
		
		public virtual void flip ()
		{
				turnR = !turnR;
				Vector3 scale = transform.localScale;
				scale.x *= -1;
				transform.localScale = scale;
		}
		
		public virtual void moveX (float moveX)
		{
				float xMove = (moveX * thisAttributes.moveForce);

	
				if (!grounded) {
						xMove /= thisAttributes.flyAmount;
				}

				if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x) < thisAttributes.maxSped)
						GetComponent<Rigidbody2D> ().velocity = new Vector2 (xMove + GetComponent<Rigidbody2D> ().velocity.x, GetComponent<Rigidbody2D> ().velocity.y);
		
		}
			
		public virtual void jump (int jumpF)
		{

				if (grounded) {
						GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpF));
					
						playJumpSound ();

						
				} 

		}

	

}
