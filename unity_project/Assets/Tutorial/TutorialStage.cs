using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStage : Structure
{
    public SpawnSpot ss;

    public void StartStructure()
    {
        foreach (var item in GetComponentsInChildren<CollideIfSeen>())
        {
            item.reset = true;
        }
    }

}
