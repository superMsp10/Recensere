using UnityEngine;
using System.Collections;

public class DefaultMap : Level
{
		public float slope;

		public override	void  generateArena ()
		{
				HeightMapGenerator gen = new HeightMapGenerator (Map.firstMap, slope);
				liveTiles = gen.generateMap (levelStart);
		}
}

