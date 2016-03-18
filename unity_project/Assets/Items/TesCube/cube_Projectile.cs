﻿using UnityEngine;
using System.Collections;

public class cube_Projectile : MonoBehaviour,Poolable,Timer
{
		public Rigidbody r;
		public item_Cube thisPooler;
		public int hitDamage;
		public LayerMask damage;

		bool armed = true;	
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
						armed = true;
						r.velocity = Vector3.zero;
				}

		}

		void OnCollisionEnter (Collision collision)
		{
				if (armed && damage == (damage | (1 << collision.gameObject.layer))) {
						player p = collision.gameObject.GetComponent<player> ();

						if (p != null) {

								if (collision.relativeVelocity.magnitude > p.Sturdy) {
										Debug.Log ("Collision velocity > player sturdyness at Cube");
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
