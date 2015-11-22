using UnityEngine;
using System.Collections.Generic;

public interface Attachable
{
		List<Poolable> attached {
				get;
		} 

		int limit {
				get;
		} 

		void attach (GameObject g);
		void detach (GameObject g);


}

