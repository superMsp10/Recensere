using UnityEngine;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
		public static EffectsManager thisM;
		private GameObject crackFX;
		public Pooler crackPooler = null;

		void Awake ()
		{
				if (thisM == null)
						thisM = this;

		}

		void Start ()
		{
				crackFX = tileDictionary.thisM.hitDecal;	
				crackPooler = new Pooler (3, 5, crackFX);

		}
	

		public void addWallCracks (Vector3 normal, Vector3 point)
		{
			 
				if (crackFX == null) {
						Debug.Log ("No crackFX");
						return;
				}

				Quaternion hitRotation = Quaternion.FromToRotation (normal, Vector3.forward);
				GameObject g = crackPooler.getObject ();
				Debug.Log ("Crack Pooler get objects2 are: " + g);

				g.transform.position = point - (normal * 0.001f);
				g.transform.rotation = hitRotation;

		}
}
