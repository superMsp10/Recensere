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
		public	void takeDamage (int damage, string attacker)
		{
				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
				}

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



	
}
