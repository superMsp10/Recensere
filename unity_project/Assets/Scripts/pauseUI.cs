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
}
