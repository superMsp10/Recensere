using UnityEngine;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
		public static EffectsManager thisM;
		private GameObject crackFX = tileDictionary.thisM.hitDecal;
		public Pooler crackPooler;

		void Awake ()
		{
				if (thisM == null)
						thisM = this;

		}
	

		public void addWallCracks (Vector3 normal, Vector3 point)
		{
				if (crackPooler == null) {
						Debug.Log ("hello");
						Pooler p = new Pooler (3, 5, crackFX);
						Debug.Log (new Test ());

				} 
				if (crackFX == null) {
						Debug.Log ("No crackFX");
						return;
				}

				Quaternion hitRotation = Quaternion.FromToRotation (normal, Vector3.forward);
				GameObject g = (GameObject)Instantiate (crackFX, point, hitRotation);
				g.transform.position = g.transform.position - (normal * 0.001f);

		}
}
