using UnityEngine;
using System.Collections;

public class cube_Projectile : MonoBehaviour,Poolable,Timer
{
		public Rigidbody r;
		public item_Cube thisPooler;
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
