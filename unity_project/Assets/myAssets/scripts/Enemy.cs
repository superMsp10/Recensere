using UnityEngine;
using System.Collections;

public class Enemy : Mob1
{
		public LayerMask whatEnemy;
		public GameObject target;
		public float sight;
		public bool jumpedSound = false;
		public bool dropMadeOF = true;

		public bool despawnWithDistance;
		public bool flipMob = true;
		public bool randomAttributes;


		new void Start ()
		{

		
				thisManage = gameManager.thisM;
				thisLevel.addEntity (this);
				GetComponent<Rigidbody2D>().centerOfMass = centerOfMass;
				thisAnim = GetComponent<Animator> ();
				thisAudio = AudioManager.thisAM.playerFX;
				if (randomAttributes)
						resetAttributes ();
				checkNecesseries ();
		
		}
		public void resetAttributes ()
		{
				thisAttributes.Dmg = Random.Range (1, 20);
				thisAttributes.flyAmount = Random.Range (0.1f, 0.9f);
				thisAttributes.HP = Random.Range (50f, 500f);
				thisAttributes.maxHP = thisAttributes.HP;
				thisAttributes.jump = Random.Range (100, 1000);
				thisAttributes.maxSped = Random.Range (5f, 20f);
				thisAttributes.moveForce = Random.Range (0.2f, 3f);







		}
		public void selectTarget ()
		{
				Collider2D[] enemies = Physics2D.OverlapCircleAll (transform.position, sight, whatEnemy);
				foreach (Collider2D c in enemies) {
						Mob1 b = c.GetComponent<Mob1> ();
						if (b != null && b.gameObject != this.gameObject) {
								target = b.gameObject;
						}

				}
		}

		void FixedUpdate ()
		{


				checkground ();
				TargetSight ();

		}

		public override void jump (int jumpF)
		{
		
				if (grounded) {
						GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, jumpF));
						if (!jumpedSound) {
								playJumpSound ();
								jumpedSound = true;
						} else {
								Invoke ("resetJump", 0.5f);
						}
			
				} 
		
		}

		new protected void checkFacing ()
		{
		
				if (Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x) > 1) {
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
						if (GetComponent<Rigidbody2D>().velocity.y < -20 && !attacking) {
				
								falling = true;
								front = true;
				
								sideBody.gameObject.SetActive (false);
								frontBody.gameObject.SetActive (true);
						}
				}
		
		}
		
		protected virtual void TargetSight ()
		{


				if (target != null) {
						if (Vector2.Distance (target.transform.position, transform.position)
								> thisAttributes.optTargetRange) {
								moveAi ();
						} else {
								checkLooking ();
						}
			
						if (Vector2.Distance (target.transform.position, transform.position) > sight) {
								target = null;
								if (despawnWithDistance) {
										gameObject.SetActive (false);
								}
				
				
						}
				} else {
						selectTarget ();
						attacking = false;
						thisAttributes.moving = false;
				}

		}
		
		public virtual void moveAi ()
		{



				Vector2 targetPos = thisManage.transform.TransformPoint (target.transform.position);
				Vector2 thisPos = thisManage.transform.TransformPoint (transform.position);

				float move = 0;

				if (targetPos.x < thisPos.x) {
						move = (Vector2.right.x * -1);
							
				} else if (targetPos.x > thisPos.x) {
						move = (Vector2.right.x);
								
						
						if (flipMob) {
								if (move < 0 && turnR) {
										flip ();
								} else if (move > 0 && !turnR) {
										flip ();
								}
				
						}
						moveX (move);
				}
				if (targetPos.y > thisPos.y || targetPos.y < thisPos.y) {
						jump (thisAttributes.jump * Random.Range (0, 30));

				} 


		}

		

		public void checkLooking ()
		{
				Vector2 targetPos = thisManage.transform.TransformPoint (target.transform.position);
				Vector2 thisPos = thisManage.transform.TransformPoint (transform.position);

				if (Mathf.Abs (targetPos.x - thisPos.x) > 0) {
						float move = 0;
			
						if (targetPos.x < thisPos.x) {
								move = (Vector2.right.x * -1);
				
						} else if (targetPos.x > thisPos.x) {
								move = (Vector2.right.x);
				
						}
						if (move < 0 && turnR) {
								flip ();
						} else if (move > 0 && !turnR) {
								flip ();
						}

				}

		}

		public void resetJump ()
		{
				jumpedSound = false;
		}

}
