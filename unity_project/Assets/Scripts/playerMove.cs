using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class playerMove : MonoBehaviour
{
    //Movement
    public float og_speed;
    public float speed;
    public float fly_speed;
    public float speedLimit;
    public Transform startChecks;
    public Transform[] endChecks;
    public LayerMask whatGround;

    //Jumping
    public float jumpPower;
    public bool jumped;
    public bool grounded = false;

    //Jetpack
    private float _fuel;
    public float maxFuel;
    public JetpackFuelDisplay jetPackFuelDisplay;
    public ParticleSystem airIntake;
    public ParticleSystem afterburn;
    bool usingJetPack = false;

    bool fuelCountdown = false;
    Slider fuelSlider;
    GameObject jetpackUIFire;

    public Animator anim;
    Rigidbody rigidbod;

    //Network
    public PhotonView photonV;

    //SFX
    public AudioSource fxSource;
    public AudioSource jetpackSource;
    public float soundMaxPitch, soundMinPitch;


    public List<AudioClip> walkingClips;
    public float soundMax, soundMin;

    public float fuel
    {
        get
        {
            return _fuel;
        }
        set
        {
            if (value >= 0 && value <= maxFuel)
            {
                _fuel = value;
                if (photonV.isMine)
                    fuelSlider.value = value;
                jetPackFuelDisplay.setFuel(value / maxFuel);
            }
        }
    }

    public void Start()
    {
        rigidbod = GetComponent<Rigidbody>();
        fuelSlider = tileDictionary.thisM.JetpackFuel;
        jetpackUIFire = tileDictionary.thisM.JetpackFire;
        fuel = maxFuel;
        fuelSlider.maxValue = maxFuel;
        fuelSlider.value = _fuel;
        jetpackUIFire.SetActive(false);
        airIntake.Stop();
        afterburn.Stop();


    }

    void Update()
    {
        checkJump();
    }

    void FixedUpdate()
    {
        checkMovement();
    }

    public void playRandomJumpingSFX()
    {
        randomJumpingSFX();
        photonV.RPC("randomJumpingSFX", PhotonTargets.Others);
    }

    [PunRPC]
    public void randomJumpingSFX()
    {
        fxSource.PlayOneShot(walkingClips[Random.Range(0, walkingClips.Count)], Random.Range(soundMin, soundMax));
    }

    void checkJump()
    {
        grounded = false;
        foreach (Transform item in endChecks)
        {
            if (Physics.Linecast(startChecks.position, item.position, whatGround))
            {
                grounded = true;
                break;
            }
        }

        if (!jumped)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (grounded)
                    jump();
                else
                    jumped = true;
            }
        }
        else if (Input.GetKey(KeyCode.Space) && _fuel > 0)
        {
            if (!usingJetPack)
                jetPack();

        }
        else if (usingJetPack)
            stopJetPack();

        if (grounded && !Input.GetKey(KeyCode.Space) && usingJetPack)
            stopJetPack();

        if (fuel <= 0 && usingJetPack)
            stopJetPack();

    }

    void stopJetPack()
    {
        usingJetPack = false;
        rigidbod.useGravity = true;
        CancelInvoke("updateFuel");
        fuelCountdown = false;
        jetpackUIFire.SetActive(false);
        airIntake.Stop();
        afterburn.Stop();
        jetpackSource.Stop();

        photonV.RPC("syncJetPackStop", PhotonTargets.Others, _fuel);
    }

    [PunRPC]
    void syncJetPackStop(float f)
    {
        airIntake.Stop();
        afterburn.Stop();
        fuel = f;
        jetpackSource.Stop();
    }

    void updateFuel()
    {
        fuel--;
    }

    void jump()
    {
        rigidbod.velocity = new Vector3(rigidbod.velocity.x * fly_speed, jumpPower, rigidbod.velocity.z * fly_speed);
        jumped = true;
        playRandomJumpingSFX();
    }



    void jetPack()
    {
        usingJetPack = true;
        rigidbod.useGravity = false;
        jetpackUIFire.SetActive(true);

        if (!fuelCountdown)
        {
            InvokeRepeating("updateFuel", 0, 1.0f);
            fuelCountdown = true;
        }
        airIntake.Play();
        afterburn.Play();
        jetpackSource.pitch = Random.Range(soundMinPitch, soundMaxPitch);
        jetpackSource.Play();

        photonV.RPC("syncJetPackStart", PhotonTargets.Others, null);

    }

    [PunRPC]
    void syncJetPackStart()
    {
        airIntake.Play();
        afterburn.Play();
        jetpackSource.Play();

    }

    void checkMovement()
    {


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        anim.SetFloat("XVelo", moveHorizontal);
        anim.SetFloat("ZVelo", moveVertical);

        if (grounded)
        {
            speed = og_speed;
            jumped = false;
        }
        else
            speed = og_speed * fly_speed;

        if (moveHorizontal == 0 && moveVertical == 0)
        {
            if (grounded)
                rigidbod.velocity = new Vector3(0, rigidbod.velocity.y, 0);
        }

        if (rigidbod.velocity.magnitude < speedLimit)
        {
            if (moveVertical > 0)
                rigidbod.AddForce(transform.forward * speed);
            else if (moveVertical < 0)
                rigidbod.AddForce(transform.forward * -speed);

            if (moveHorizontal > 0)
                rigidbod.AddForce(transform.right * speed);
            else if (moveHorizontal < 0)
                rigidbod.AddForce(transform.right * -speed);
        }
    }

}
