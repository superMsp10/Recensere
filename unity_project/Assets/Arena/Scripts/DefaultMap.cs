using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultMap : Level
{
    public float slope;

    public LootTile[] lootTiles;
    public float lootTime;
    public float fuelRate = 2f;

    //public override void generateArena()
    //{
    //    //Debug.Log("Generating Arena");
    //    HeightMapGenerator gen = new HeightMapGenerator(Map.firstMap, slope);
    //    generateArena(gen);
    //}

    List<string> deathMessages = new List<string>() { "Went too <color=red>deep</color>", "You got lost in the <size=24><color=black>abyss</color></size>", "You have reached the point of no <b>return</b>",
        "<color=purple><i>Hypnic Jerk</i>, but this time its real</color>", "<color=grey>Free fall: No air resistance just gravity</color>", "Searching for new <color=green>grounds</color> to land on",
        "<size=36>Cant stop the falling by \n Justin Timberlake</size>", "<color=orange>You are falling in your physics class</color>" };

    public new void Start()
    {
        base.Start();
        lootTiles = GameObject.FindObjectsOfType<LootTile>();

    }

    void generateLoot()
    {
        Debug.Log("Generated loot");
        foreach (LootTile t in lootTiles)
        {
            t.generateLoot();
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        if (PhotonNetwork.isMasterClient)
        {
            InvokeRepeating("generateLoot", 0, lootTime);
            OnLoaded();
        }
        else
        {
            thisM.StartStrucutresSync();

            foreach (SpawnSpot s in spawns)
            {
                s.NetworkInit();
            }

            Debug.Log("Updating Loot Tiles");
            foreach (LootTile t in lootTiles)
            {
                t.NetworkInit();
            }
        }
    }

    public override void playerDeath()
    {
        tileDictionary.thisM.pauseUI.GetComponent<pauseUI>().deathMessage = deathMessages[UnityEngine.Random.Range(0, deathMessages.Count)];
        StartCoroutine(tileDictionary.thisM.pauseUI.GetComponent<pauseUI>().Respawn(5f));
    }

}

