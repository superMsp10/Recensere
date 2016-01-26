using UnityEngine;
using System.Collections;

public class destroyedTile : MonoBehaviour,Poolable,Timer
{
		public GameObject gameobject {
				get {
						return gameObject;
				}
		}
		public	virtual void reset (bool on)
		{
				gameObject.SetActive (on);
				if (!on)
						CancelTimer ();
				
		}
	
	
	
	
		public void StartTimer (float time)
		{
				Invoke ("TimerComplete", time);
		
		}
	
		public void CancelTimer ()
		{
				CancelInvoke ("TimerComplete");
		
		}
		public virtual void TimerComplete ()
		{
				Debug.Log ("Default DesTile timer Complete");
		}

}

