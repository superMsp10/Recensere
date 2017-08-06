using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageFinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        (GameManager.thisM.currLevel as TutorialLevel).nextStage();
    }
}
