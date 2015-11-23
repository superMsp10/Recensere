using UnityEngine;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
		public static EffectsManager thisM;
		private GameObject crackFX;

		[HideInInspector]
		public Pooler
				crackPooler = null;

		public float decalReset;
		public int maxDecals;


		void Awake ()
		{
				if (thisM == null)
						thisM = this;

		}

		void Start ()
		{
				crackFX = tileDictionary.thisM.hitDecal;	
				crackPooler = new Pooler (maxDecals, crackFX);

		}
	

		public void addWallCracks (Vector3 normal, Vector3 point, Tile t, float dmgPercent)
		{
				if (t.attached.Count < t.limit) {
						if (crackFX == null) {
								Debug.Log ("No crackFX");
								return;
						}
						float size = Mathf.Lerp (0.1f, t.tileSize, dmgPercent);
						Quaternion hitRotation = Quaternion.FromToRotation (normal, Vector3.forward);
						GameObject g = crackPooler.getObject ();
						g.transform.parent = transform;
						g.transform.localScale = new Vector3 (size, size, size);

						g.transform.position = point - (normal * 0.001f);
						g.transform.rotation = hitRotation;
						g.GetComponent<Timer> ().StartTimer (decalReset);
						t.attach (g);


				}
		}
}
