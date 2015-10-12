using UnityEngine;
using System.Collections;

public class decals : MonoBehaviour,Poolable,Timer
{
		public GameObject gameobject {
				get {
						return gameObject;
				}
		}
		public	void reset (bool on)
		{
				gameObject.SetActive (on);
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
				EffectsManager.thisM.crackPooler.disposeObject (this);
		}


}
