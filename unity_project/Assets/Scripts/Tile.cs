using UnityEngine;
using System.Collections;

public abstract class Tile:MonoBehaviour, Health
{
		public	float health;
		public	string lastAttacker;

		public int tileSize = 4;


		public Tile (int size)
		{
				tileSize = size;
		}
		public	bool takeDamage (float damage, string attacker)
		{
				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
						return true;
				}
				return false;

		}

		void OnControllerColliderHit (ControllerColliderHit hit)
		{
				Debug.Log ("hello");
		}
		public	void Destroy ()
		{
				Destroy (gameObject);
		}
		public	string lastDamageBy ()
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
				if (collision.relativeVelocity.magnitude > health * sdm) {
						if (takeDamage (sdm * collision.relativeVelocity.magnitude, collision.collider.name)) {
								collision.collider.attachedRigidbody.velocity = collision.relativeVelocity;
						}
				}
		
		}

	
}
