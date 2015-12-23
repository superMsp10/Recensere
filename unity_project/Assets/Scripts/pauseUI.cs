using UnityEngine;
using System.Collections;

public class pauseUI : MonoBehaviour,UIState
{
		public GameObject inGame;
		public void StartUI ()
		{
				gameObject.SetActive (true);
		
		}
	
		public void EndUI ()
		{
				gameObject.SetActive (false);
		
		}
	
		public	void UpdateUI ()
		{
				Debug.Log ("hello");
				UIManager.thisM.changeUI (inGame);
		}

		public	void Respawn ()
		{
				GameManager.thisM.NetworkDisable ();
				Invoke ("Spawn", 5.0f);

		}

		void Spawn ()
		{
				GameManager.thisM.NetworkEnable ();

		}
}
