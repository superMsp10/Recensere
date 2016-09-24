using UnityEngine;
using System.Collections;

public class fuelTile : LootTile
{
    public JetpackFuelDisplay display;

    public PhotonView v;
    private float _fuel;
    public float maxFuel;
    playerMove curr;
    float fuelRate;
    //Tube FX
    public MeshRenderer tube;
    public Color tubeOrg;
    public Color tubeFuel;
    float timeStarted;
    bool startUp = false;
    //Vale&Lid FX
    public Motor shaft;
    public Vector3 open, closed;
    bool playerOn = false;
    //Particle FX
    public ParticleSystem airIntake;
    public ParticleSystem fuelSpray;


    public float fuel
    {
        get
        {
            return _fuel;
        }
        set
        {
            _fuel = value;
            display.setFuelY(value / maxFuel);
        }
    }

    void Start()
    {
        tube.material.color = tubeOrg;
        fuelRate = GameManager.thisM.currLevel.fuelRate;

        airIntake.Stop();
        fuelSpray.Stop();
    }

    void closeLid()
    {
        shaft.moving = true;
        shaft.totalTime = 2f;
        shaft.start = shaft.lookAt.localPosition;
        shaft.end = closed;
        shaft.startedTime = Time.time;

        Invoke("stopMoving", 2f);
    }

    void openLid()
    {
        shaft.moving = true;
        shaft.totalTime = 1.5f;
        shaft.start = shaft.lookAt.localPosition;
        shaft.end = open;
        shaft.startedTime = Time.time;

        Invoke("stopMoving", 1.5f);
    }

    void stopMoving()
    {
        shaft.moving = false;
    }



    void Update()
    {
        if (startUp)
        {
            tube.material.color = Color.Lerp(tubeOrg, tubeFuel, (Time.time - timeStarted) / fuelRate);


        }
    }

    [PunRPC]
    public void syncFuel(float f)
    {
        fuel = f;
    }

    public override void generateLoot()
    {
        if ((fuel + fuelRate) <= maxFuel)
            fuel += fuelRate;
        v.RPC("syncFuel", PhotonTargets.Others, _fuel);
    }

    void OnTriggerEnter(Collider other)
    {
        playerMove m = other.GetComponent<playerMove>();
        if (m != null && fuel > 0)
        {
            curr = m;
            InvokeRepeating("inputFuel", fuelRate, fuelRate);
            v.RPC("startFuelFX", PhotonTargets.All, null);

        }
    }

    [PunRPC]
    public void startFuelFX()
    {
        timeStarted = Time.time;
        startUp = true;
        playerOn = true;
        openLid();
        airIntake.Play();
        fuelSpray.Play();
        Debug.Log("start fuel FX");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == curr.gameObject)
        {
            stopFX();
        }
    }

    void stopFX()
    {
        CancelInvoke();
        v.RPC("stopFuelFX", PhotonTargets.All, null);
    }

    [PunRPC]
    public void stopFuelFX()
    {

        startUp = false;
        tube.material.color = tubeOrg;
        playerOn = false;
        closeLid();
        airIntake.Stop();
        fuelSpray.Stop();
        Debug.Log("stop fuel FX");

    }

    public void inputFuel()
    {

        startUp = false;

        if ((fuel + fuelRate) >= 0)
        {
            curr.fuel++;
            fuel--;
            v.RPC("syncFuel", PhotonTargets.Others, _fuel);
        }
        else
        {
            stopFX();
        }
    }


}

