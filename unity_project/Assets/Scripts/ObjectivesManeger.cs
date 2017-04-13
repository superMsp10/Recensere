using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectivesManeger : MonoBehaviour
{
    public int playerLevel;
    public static ObjectivesManeger thisM;
    List<Objective> available = new List<Objective> { new PlaceMore() };
    List<Objective> completed = new List<Objective>();

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
                playerRoom.closeLid();
                playerRoom.objectiveText.text = compliments[UnityEngine.Random.Range(0, compliments.Length)] + ", " + active.done;
                available.Remove(active);
                completed.Add(active);
                if (active.reuseable)
                {
                    available.Add((Objective)Activator.CreateInstance(active.GetType()));
                }
                active = randomObjective();
                Invoke("showText", 5f);
                playerLevel++;
            }
        }
        else
        {
            active = randomObjective();
            showText();
        }

    }

    public void setCompleted()
    {
        Persistent.thisPersist.completed = new List<Objective>(completed);
        DatabaseConnect.thisM.setLevel(playerLevel);
    }


    void showText()
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
            return available[UnityEngine.Random.Range(0, available.Count)];
        }
        return finished;
    }

}
