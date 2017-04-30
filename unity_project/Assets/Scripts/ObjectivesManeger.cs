using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectivesManeger : MonoBehaviour
{
    public int playerLevel;
    public static ObjectivesManeger thisM;
    List<Objective> available = new List<Objective> { new PlaceMore(), new KillMore(), new DontDie(), new DestroyMore() };
    List<Objective> completed = new List<Objective>();

    Objective active;
    public string[] compliments = new string[] { "Well done", "Good going", "Great work", "Wow!", "Keep it up", "Oh yeah!", "Grate jab!", "Gracias" };

    DefaultMapPlayerStructure playerRoom;
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
        playerRoom = (DefaultMapPlayerStructure)GameManager.thisM.myPlayer.spwanRoom;
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
                    Objective nO = (Objective)Activator.CreateInstance(active.GetType());
                    nO.iteration += active.iteration;
                    available.Add(nO);
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
        PhotonNetwork.player.SetScore(completed.Count);
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
        Objective ret;
        if (available.Count > 0)
        {
            ret = available[UnityEngine.Random.Range(0, available.Count)];
            ret.Initialize();
            return ret;
        }
        return finished;
    }

}
