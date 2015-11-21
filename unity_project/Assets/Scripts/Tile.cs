using UnityEngine;
using System.Collections.Generic;

public abstract class Tile:MonoBehaviour, Health, Attachable
{
		public	float health;
		public	string lastAttacker;
		public int xPos;
		public int yPos;
		public	bool takeDmg = true;
		public float Sturdy = 10f;
		public List<Poolable> Attached = new List<Poolable> ();


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

		public float Sturdyness {
				get {
						return Sturdy; 
				}
	
		}

		public List<Poolable> attached {
				get {
						return Attached; 
				}
		
		}

		public	 void attach (GameObject g)
		{
				return lastAttacker;
		}

		public	 void detach (GameObject g)
		{
				return lastAttacker;
		}

		void OnCollisionEnter (Collision collision)
		{
				float sdm = GameManeger.speedToDamageMultiplier;
//				Debug.Log ("Collision Enter at Tile");
				if (takeDmg && collision.relativeVelocity.magnitude > Sturdy) {
						if (takeDamage (Mathf.Pow (collision.relativeVelocity.magnitude, sdm), collision.collider.name)) {
								if (collision.collider.attachedRigidbody != null)
										collision.collider.attachedRigidbody.velocity *= sdm;

						}
						EffectsManager.thisM.addWallCracks (collision.contacts [0].normal, collision.contacts [0].point);
		
				}
		
		}

	
}
