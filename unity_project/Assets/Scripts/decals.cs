using UnityEngine;
using System.Collections;

public class decals : MonoBehaviour,Poolable,Timer
{
		public ParticleSystem dust;
		public Tile t;

		public GameObject gameobject {
				get {
						return gameObject;
				}
		}
		public	void reset (bool on)
		{
				gameObject.SetActive (on);

				if (!on) {
						CancelTimer ();
						dust.Stop ();
						gameobject.transform.localScale = new Vector3 (1, 1, 1);

				} else
						dust.Play ();
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
				if (t != null) {
						t.detach (gameobject);
				}
				EffectsManager.thisM.crackPooler.disposeObject (this);
		}


}
