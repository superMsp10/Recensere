using UnityEngine;
using System.Collections;

public class fuelTile : LootTile
{
    public JetpackFuelDisplay display;

    public PhotonView v;
    private float _fuel;
    public float maxFuel;
    playerMove curr;
    public float fuelRate = 1;
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
    //SFX
    public AudioSource thisSource;
    public AudioClip openingSound;

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
        airIntake.Stop();
        fuelSpray.Stop();
        fuel = maxFuel / 2;
    }

    public override void NetworkInit()
    {
        v.RPC("getFuelInit", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
        //Debug.Log("Sent(Client) Fuel Request");
    }

    [PunRPC]
    public void getFuelInit(int playerId)
    {
        v.RPC("syncFuel", PhotonPlayer.Find(playerId), fuel);
        //Debug.Log("Sent(Master) Fuel");
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
        thisSource.PlayOneShot(openingSound);

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
        //Debug.Log("Synced(Client) Fuel");
    }

    public override void generateLoot()
    {
        if ((fuel + ((DefaultMap)GameManager.thisM.currLevel).fuelRate) <= maxFuel)
            fuel += ((DefaultMap)GameManager.thisM.currLevel).fuelRate;
        v.RPC("syncFuel", PhotonTargets.Others, _fuel);
    }

    void OnTriggerEnter(Collider other)
    {
        playerMove m = other.GetComponent<playerMove>();
        if (m != null && fuel > 0)
        {
            curr = m;
            InvokeRepeating("inputFuel", fuelRate, fuelRate);
            if (v != null)
            {
                v.RPC("startFuelFX", PhotonTargets.All, null);
            }
            else
            {
                startFuelFX();
            }

        }
    }

    [PunRPC]
    public void startFuelFX()
    {
        timeStarted = Time.time;
        startUp = true;
        playerOn = true;
        openLid();
        if (!airIntake.isPlaying)
            airIntake.Play();
        if (!fuelSpray.isPlaying)
            fuelSpray.Play();
        thisSource.Play();

    }

    void OnTriggerExit(Collider other)
    {
        if (curr != null)
        {
            if (other.gameObject == curr.gameObject)
            {
                stopFX();
            }
        }

    }

    void stopFX()
    {
        CancelInvoke();
        if (v != null)
        {
            v.RPC("stopFuelFX", PhotonTargets.All, null);
        }
        else
        {
            stopFuelFX();
        }
    }

    [PunRPC]
    public void stopFuelFX()
    {

        startUp = false;
        tube.material.color = tubeOrg;
        playerOn = false;
        closeLid();
        if (!airIntake.isStopped)
            airIntake.Stop();
        if (!fuelSpray.isStopped)
            fuelSpray.Stop();
        thisSource.Stop();

    }

    public void inputFuel()
    {

        startUp = false;

        if ((fuel - fuelRate) >= 0)
        {
            curr.fuel += fuelRate;
            fuel -= fuelRate;
            if (v != null)
            {
                v.RPC("syncFuel", PhotonTargets.Others, _fuel);
            }
        }
        else
        {
            stopFX();
        }
    }


}

