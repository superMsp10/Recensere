using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
		public static UIManager thisM;
		public ErrorUI connectError;
		public UIState currentUI;
		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}
}
