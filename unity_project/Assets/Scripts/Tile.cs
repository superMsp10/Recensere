using UnityEngine;
using System.Collections;

public abstract class Tile:MonoBehaviour, Health
{
		public	float health;
		public	string lastAttacker;
		public int xPos;
		public int yPos;
		public	bool takeDmg = true;


		public int tileSize = 4;


		public Tile (int size)
		{
				tileSize = size;
		}

		public	virtual bool takeDamage (float damage, string attacker)
		{
//				Debug.Log ("Take Damage From Tile");

				if (health <= 0)
						return false;
				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
						return true;
				}
				return false;

		}

		public	virtual void syncDamage (float damage, string attacker)
		{
//				Debug.Log ("Sync Damage From Tile");

				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
				}
		
		}


		public virtual	void Destroy ()
		{
				Destroy (gameObject);
			
		}
		public	 string lastDamageBy ()
		{
				return lastAttacker;
		}

		public float HP {
				get {
						return health; 
				}
				set {
						health = value; 
				}
		}

		void OnCollisionEnter (Collision collision)
		{
				//		Rigidbody r = collision.collider.attachedRigidbody;
				float sdm = GameManeger.speedToDamageMultiplier;
//				Debug.Log ("Collision Enter at Tile");
				if (takeDmg && collision.relativeVelocity.magnitude > 3) {
						if (takeDamage (Mathf.Pow (collision.relativeVelocity.magnitude, sdm), collision.collider.name)) {
								if (collision.collider.attachedRigidbody != null)
										collision.collider.attachedRigidbody.velocity *= sdm;

						}
						EffectsManager.thisM.addWallCracks (collision.contacts [0].normal, collision.contacts [0].point);
		
				}
		
		}

	
}
