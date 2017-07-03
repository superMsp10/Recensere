using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class tileDictionary : MonoBehaviour
{

    public static tileDictionary thisM;
    //----------------Tiles-------------------//
    public Tile floorTile;
    public Tile wallTile;
    public Tile border;
    public GameObject destroyedFloorTile;
    public GameObject destroyedWallTile;

    //----------------Other Stuff-------------------//
    public SpawnSpot spawnSpot;
    public GameObject hitDecal;
    public GameObject inGameUI;
    public GameObject pauseUI;

    public Text HPText;
    public Slider JetpackFuel;
    public GameObject JetpackFire;
    public Transform projectiles;

    public GameObject destroyedGrenadeProjectile;



    void Awake()
    {
        if (thisM == null)
            thisM = this;

    }

}
