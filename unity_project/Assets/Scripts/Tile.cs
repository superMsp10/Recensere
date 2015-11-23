using UnityEngine;
using System.Collections.Generic;

public abstract class Tile: MonoBehaviour, Health, Attachable
{
		public	float health;
		public	string lastAttacker;
		public int xPos;
		public int yPos;
		public	bool takeDmg = true;
		public float Sturdy = 10f;
		public int decalLimit;
		List<Poolable> Attached;


		public int tileSize = 4;


		void Start ()
		{
				Attached = new List<Poolable> ();
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
				for (int i = 0; Attached.Count>0; i++) {
						detach (Attached [0].gameobject);


				}

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

		public int limit {
				get {
						return decalLimit; 
				}
		
		}

		public List<Poolable> attached {
				get {
						return Attached; 
				}
		
		}

		public	 void attach (GameObject g)
		{

				if (g != null) {
						Attached.Add (g.GetComponent<Poolable> ());
				} else
						Debug.Log ("Game object you are trying to attach is null");
		}

		public	 void detach (GameObject g)
		{
				Attached.Remove (g.GetComponent<Poolable> ());
				EffectsManager.thisM.crackPooler.disposeObject (g.GetComponent<Poolable> ());

		}

		void OnCollisionEnter (Collision collision)
		{
				float sdm = GameManeger.speedToDamageMultiplier;
//				Debug.Log ("Collision Enter at Tile");
				float dmg = Mathf.Pow (collision.relativeVelocity.magnitude, sdm);
				if (takeDmg && collision.relativeVelocity.magnitude > Sturdy) {
						EffectsManager.thisM.addWallCracks (collision.contacts [0].normal, collision.contacts [0].point, this, dmg / HP);
						if (takeDamage (dmg, collision.collider.name)) {
								if (collision.collider.attachedRigidbody != null) {
										collision.collider.attachedRigidbody.velocity *= sdm;
								}
						}

		
				}
		
		}

	
}
