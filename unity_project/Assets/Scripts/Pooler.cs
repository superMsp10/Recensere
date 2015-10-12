using UnityEngine;
using System.Collections.Generic;

public class Pooler : MonoBehaviour
{
		public List<Poolable> active;
		public List<Poolable> useable;
		public GameObject original;
		int max, min;


		public Pooler (int min, int max, GameObject org)
		{
				original = org;
				Poolable p;

				for (int i = 0; i < min; i++) {
						p = Instantiate (org).GetComponent<Poolable> ();
						useable.Add (p);
						p.reset (false);
				}
		}

		public GameObject getObject ()
		{
				Poolable ret;
				if (active.Count < max)
						return null;

				if (useable.Count - 1 < min) {
						Poolable p;
						for (int i = 0; i < min -  (useable.Count - 1); i++) {
								p = Instantiate (original).GetComponent<Poolable> ();
								useable.Add (p);
								p.reset (false);
						}
				}

				ret = useable [0];
				active.Add (ret);
				useable.Remove (ret);
				ret.reset (true);
				return ret.gameobject;
				
		}

		public void disposeObject (Poolable p)
		{
				useable.Add (p);
				active.Remove (p);
				p.reset (false);
		}
}
