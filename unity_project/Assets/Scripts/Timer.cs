using UnityEngine;

using System.Collections;
public interface Timer
{
		void StartTimer (float time);
		void CancelTimer ();

		void TimerComplete ();

	
}
