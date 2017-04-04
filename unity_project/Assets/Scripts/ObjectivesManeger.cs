using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ObjectivesManeger : MonoBehaviour
{
    public int playerLevel;
    public static ObjectivesManeger thisM;
    List<Objective> available = new List<Objective> { new Objective() };
    List<Objective> completed;

    Objective active;
    public string[] compliments = new string[] { "Well done", "Good going", "Great work", "Wow!", "Keep it up", "Oh yeah!", "Grate jab!", "Gracias" };

    PlayerStructure playerRoom;
    FinishedObjective finished;

    void Awake()
    {
        if (thisM == null)
        {
            thisM = this;
        }

    }

    public void Initialize()
    {
        playerLevel = Persistent.thisPersist.Level;
        playerRoom = GameManager.thisM.myPlayer.spwanRoom;
        finished = new FinishedObjective();
    }

    public void updateObjectives()
    {
        if (active != null)
        {
            if (active.CheckCompleted())
            {
                playerRoom.objectiveText.text = compliments[Random.Range(0, compliments.Length)] + ", " + active.done;
                available.Remove(active);
                completed.Add(active);
            }
        }
        else
        {
            active = randomObjective();
            showtext();
        }

    }


    void showtext()
    {
        playerRoom.closeLid();
        playerRoom.objectiveText.text = active.description;
        Invoke("endText", 5f);
    }

    void endText()
    {
        playerRoom.openLid();
    }

    Objective randomObjective()
    {
        if (available.Count > 0)
        {
            return available[Random.Range(0, available.Count)];
        }
        return finished;
    }

}
