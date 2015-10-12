using UnityEngine;
using System.Collections.Generic;

public class Pooler : MonoBehaviour
{
		public List<GameObject> active = new List<GameObject> ();
		public List<GameObject> useable = new List<GameObject> ();
		public GameObject original;
		int max;


		public Pooler (int max, GameObject org)
		{
				original = org;
				this.max = max;
		}

		public GameObject getObject ()
		{
				GameObject ret = null;
				if (active.Count > max) {
//						Debug.Log ("Active: " + active.Count);
//						Debug.Log ("Max: " + max);

						ret = active [0];
						active.Remove (ret);
						useable.Add (ret);

				} else if ((useable.Count - 1) < 1) {
//						Debug.Log ("Useable" + useable.Count);
						Poolable p;
						useable.Add (Instantiate (original)); 
						ret = useable [useable.Count - 1];
						ret.name = "ObjectPooled: " + (useable.Count + active.Count).ToString ();		
						p = ret.GetComponent<Poolable> ();
						if (p == null) {
								Debug.LogError (original.name + " is not poolable");
						} else {
								p.reset (false);
						}
						
						ret = useable [useable.Count - 1];

				} else {
						ret = useable [useable.Count - 1];

				}


				
				useable.Remove (ret);
				active.Add (ret);

				ret.GetComponent<Poolable> ().reset (true);
				return ret;
				
		}

		public void disposeObject (Poolable p)
		{
				active.Remove (p.gameobject);
				useable.Add (p.gameobject);
				p.reset (false);
		}
}
