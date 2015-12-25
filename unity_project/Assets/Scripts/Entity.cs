using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class DestroyDetails
{
		
		public bool destroyOnAwake;			// Whether or not this gameobject should destroyed after a delay, on Awake.
		public float awakeDestroyDelay;		// The delay for destroying it on Awake.


}

public  class Entity : MonoBehaviour
{

		public DestroyDetails desD;
//		public  level thisLevel;
		public GameManager thisManage;
		public bool customHierarchy = false;
		public int stage;
		public bool despawnWithStage;

		void Start ()
		{
//				if (thisLevel == null)
//						Debug.LogError ("no Level referenced for this entity: " + gameObject.name + " Parent =  " + gameObject.transform.parent);
				thisManage = GameManager.thisM;
//				thisLevel.addEntity (this);


		}

		void Awake ()
		{
				
				if (desD.destroyOnAwake) {
						DestroyEntity (desD.awakeDestroyDelay);
				}
		
		}

		public void changeS (float  lev)
		{
				if (despawnWithStage) {
						if (lev == stage) {
			
								gameObject.SetActive (true);
						} else {
								gameObject.SetActive (false);
						}
				}
		}
//		public virtual void changeLevel (level lev)
//		{
//				if (lev == null)
//						Debug.LogError ("transfering level is null: " + gameObject.name);
//				thisLevel.removeEntity (this);
//				thisLevel = lev;
//				thisLevel.addEntity (this);
//
//		}
	
		void DisableChildGameObject (string name)
		{
				// Destroy this child gameobject, this can be called from an Animation Event.
				if (transform.Find (name).gameObject.activeSelf == true)
						transform.Find (name).gameObject.SetActive (false);
		}

		public void DestroyEntity (float i)
		{
				
				Destroy (gameObject, i);

		}

		void OnDestroy ()
		{
//				thisLevel.removeEntity (this);

		}


}
