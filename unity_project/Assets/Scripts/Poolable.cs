using UnityEngine;
using System.Collections;

public interface Poolable
{
		GameObject gameobject {
				get;
		}
		void reset (bool on);

}
