using UnityEngine;
using System.Collections;

public class pickups : Entity
{

		invManager thisInv;
		public Holdable thisHolding;
		public bool pickable = true;
		public int amount = 1;
		public float resetPickup = 5;
		public AudioClip pickup;
		public bool dropRandom;


		void Start ()
		{
				if (thisLevel == null)
						Debug.LogError ("no Level referenced for this entity: " + gameObject.name);
				thisManage = gameManager.thisM;
				thisInv = invManager.thisInv;
				thisLevel.addEntity (this);
				if (dropRandom)
						amount = Random.Range (1, amount);
				pickable = false;
				Invoke ("resetPick", 0.5f);
	
		}
		void OnTriggerEnter2D (Collider2D other)
		{
				thisManage = gameManager.thisM;
				if (other.gameObject.tag == "teleport") {
						Teleport teleSpot = other.GetComponent<Teleport> ();
						teleSpot.teleport (gameObject);
			
				}
				if (other.gameObject.tag == "boost") {
						collisionBoost thisBoost = other.gameObject.GetComponent<collisionBoost> ();
						if (thisBoost == null)
								Debug.LogError ("no collision boost script attached");
						
						thisBoost.boost (GetComponent<Rigidbody2D>());
				}
		
		
			

				if (other.gameObject.tag == "Player" && pickable) {
						if (thisHolding.weapon) {

								thisHolding.gameObject.GetComponent<Weapon> ().controller = thisManage.myPlayer.GetComponent<Mob1> ();
						}
		
						int returnA = thisInv.addHoldable (thisHolding, amount);
		
						if (returnA == 0) {
								AudioManager.thisAM.playerFX.PlayOneShot (pickup);
								Destroy (gameObject);
						} else if (amount == 0) {
								Destroy (gameObject);
						} else {
								pickable = false;
								Invoke ("resetPick", 5f);
								amount -= returnA;
						}
			
				}


		}

		void resetPick ()
		{
				pickable = true;
		}

		
}
