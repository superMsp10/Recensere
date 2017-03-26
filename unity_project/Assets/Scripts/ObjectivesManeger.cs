using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ObjectivesManeger : MonoBehaviour
{
    public int playerLevel;
    public static ObjectivesManeger thisM;
    List<Objective> available = new List<Objective>() { new Objective() };
    Objective active;
    public Text objectiveText;
    public string[] compliments = new string[] { "Well done", "Good going", "Great work", "Wow!", "Keep it up", "Oh yeah!", "Grate jub!", "Gracias" };

    void Initialize()
    {
        if (thisM != null)
        {
            thisM = this;
        }
        playerLevel = Persistent.thisPersist.Level;
    }

    void updateObjectives()
    {
        if (active != null)
        {
            if (active.CheckCompleted())
            {
                objectiveText.text = compliments[Random.Range(0, available.Count)] + ", " + active.done;
            }
        }
        else
        {
            newObjective();
            updateObjectives();
        }

    }

    void newObjective()
    {
       active = available[Random.Range(0, available.Count)];
    }

}
