using UnityEngine;
using System.Collections;

public class decals : MonoBehaviour,Poolable
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


}
