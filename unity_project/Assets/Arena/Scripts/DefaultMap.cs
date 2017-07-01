using UnityEngine;
using System.Collections;

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
        }
    }

    public override void InitStrucutres(string JSON)
    {
        base.InitStrucutres(JSON);

        Debug.Log("Updating Loot Tiles");
        foreach (LootTile t in lootTiles)
        {
            t.NetworkInit();
        }
    }

}

