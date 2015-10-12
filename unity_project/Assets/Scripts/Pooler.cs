using UnityEngine;
using System.Collections.Generic;

public class Pooler : MonoBehaviour
{
		public List<GameObject> active = new List<GameObject> ();
		public List<GameObject> useable = new List<GameObject> ();
		public GameObject original;
		int max, min;


		public Pooler (int min, int max, GameObject org)
		{
				original = org;
				Poolable p;
			

//				for (int i = 0; i < min; i++) {
//						p = Instantiate (org).GetComponent<Poolable> ();
//						if (p == null) {
//								Debug.LogError (org.name + " is not poolable");
//						} else {
//								useable.Add (p);
//								p.reset (false);
//						}
//					
//				}
		}

		public GameObject getObject ()
		{
				GameObject ret;
				if (active.Count > max) {
						return null;
				}

//				if ((useable.Count - 1) < min) {
//						Poolable p;
//						for (int i = 0; i < min -  (useable.Count - 1); i++) {
//								p = Instantiate (original).GetComponent<Poolable> ();
//								if (p == null) {
//										Debug.LogError (original.name + " is not poolable");
//								} else {
//										useable.Add (p);
//										p.reset (false);
//								}
//						}
//				}
//				Debug.Log (useable.Count);
//
//				ret = useable [0];
//				active.Add (ret);
//				useable.Remove (ret);
//				ret.reset (true);
//				Debug.Log (useable.Count);

				useable.Add (Instantiate (original)); 
				ret = useable [0];
				useable.RemoveAt (0);
				active.Add (ret);

//				ret.name = "ObjectPooled: " + (useable.Count + active.Count).ToString ();
////				useable.Remove (ret);
//				
//				ret.GetComponent<Poolable> ().reset (true);


				return ret;
				
		}

//		public void disposeObject (Poolable p)
//		{
//				useable.Add (p);
//				active.Remove (p);
//				p.reset (false);
//		}
}
