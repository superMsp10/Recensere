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

		public	virtual void takeDamage (float damage, string attacker)
		{
				if (health <= 0)
						return;
				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
				}

		}

		public	virtual void syncDamage (float damage, string attacker)
		{
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
				if (takeDmg && collision.relativeVelocity.magnitude > 1) {
						takeDamage (Mathf.Pow (collision.relativeVelocity.magnitude, sdm), collision.collider.name);
						Debug.Log (Mathf.Pow (collision.relativeVelocity.magnitude, sdm) + "Mag: " + collision.relativeVelocity.magnitude);
						EffectsManager.thisM.addWallCracks (collision.contacts [0].normal, collision.contacts [0].point);
		
				}
		
		}

	
}
