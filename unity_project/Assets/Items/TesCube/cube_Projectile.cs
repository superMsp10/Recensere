using UnityEngine;
using System.Collections;

public class cube_Projectile : MonoBehaviour,Poolable,Timer
{
		public Rigidbody r;
		public item_Cube thisPooler;
		public int hitDamage;

		public bool armed = false;	
		public GameObject gameobject {
				get {
						return gameObject;
				}
		}

		
		public	void reset (bool on)
		{
				gameObject.SetActive (on);
				r.isKinematic = !on;
				transform.SetParent (tileDictionary.thisM.projectiles, true);


				if (on) {
						r.velocity = Vector3.zero;
				}

		}

		void OnCollisionEnter (Collision collision)
		{
				if (armed) {
						player p = collision.gameObject.GetComponent<player> ();

						if (p != null) {

								if (collision.relativeVelocity.magnitude > p.Sturdy) {
										if (p.takeDamage (hitDamage, collision.collider.name)) {
												Debug.Log ("Player died at cube");

										} 
										armed = false;
								}
						
						
						}
				}
		}

		public void StartTimer (float time)
		{
				Invoke ("TimerComplete", time);
		
		}
	
		public void CancelTimer ()
		{
				CancelInvoke ("TimerComplete");
		
		}
		public void TimerComplete ()
		{
				if (thisPooler != null) {
						thisPooler.detach (gameobject);
				}
		}

	
}
