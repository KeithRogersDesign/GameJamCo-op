using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    private xbox_gamepad gamepad;

    public GameObject bulletPrefab;
    public GameObject bulletSpawnPosition;

    public GameObject railgunSpawn;
    public GameObject machineSpawn;
    public GameObject gunBarrel;

    public PlayerOverheat playerOverheat;

    public float machineGunFireRate = 5.0f;
    public float railgunDamageRatio = 5.0f;
    public float gunCoolDown = 0.15f;
    public float railgunRange = 100.0f;
    private float overHeatValue;

    private GameObject upperBody;
    public PlayerController player;

    private float gunCoolDownCpy;

    Ray shootRay;
    RaycastHit shoothit;
    int shootableMask;
    ParticleSystem railgunParticles;
    LineRenderer railgunLine;
    Light railgunLight;


    AudioSource railgunAudio;
    AudioSource machinegunAudio;

    private float railgunTimer = 0f;
    private float railgunCD = 3.0f;
    private float railgunDamage;

    private void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
     //   railgunParticles = GetComponentInChildren<ParticleSystem>();
   //     railgunLine = GetComponentInChildren<LineRenderer>();
  //      railgunLight = GetComponentInChildren<Light>();
    //    railgunParticles.Stop();

    }

    // Use this for initialization
    void Start () {
        gamepad = GamePadManager.Instance.GetGamePad(2);
        upperBody = GetComponent<PlayerController>().upperBody;
        gunCoolDownCpy = gunCoolDown;
        

	}
	
	// Update is called once per frame
	void Update () {

        if(gamepad.GetTriggerUp_L())
        {
            railgunTimer = 0;
        }


        if (!playerOverheat.overheated)
        {
            if (gunCoolDown > 0)
            {
                gunCoolDown -= Time.deltaTime;
            }
            railgunCD -= Time.deltaTime;

            if (railgunCD < 0)
            {
                if (gamepad.GetTriggerDown_L() && !playerOverheat.overheated)
                {
                    ShootRailgun();
                    railgunCD = 2.0f;
                }
            }
         
            if (gamepad.GetTriggerDown_R() && gunCoolDown <= 0 && !playerOverheat.overheated)
            {
                player.isShooting = true;
                gunCoolDown = gunCoolDownCpy;
                GameObject tempBullet;
                tempBullet = Instantiate(bulletPrefab, bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.rotation) as GameObject;
                gunBarrel.transform.Rotate(player.playerMech.transform.forward * 2000 * Time.deltaTime);
                Rigidbody tempBody;
                tempBody = tempBullet.GetComponent<Rigidbody>();

                tempBody.AddForce(tempBullet.transform.forward * machineGunFireRate);


                Destroy(tempBullet, 3);
            }
            else if (!gamepad.GetTriggerDown_R() && !gamepad.GetTriggerDown_L())
            {
                player.isShooting = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
           // Destroy(collision.gameObject);
        }
    }

    private void ShootRailgun()
    {

        if (!playerOverheat.overheated || railgunTimer < 3.0f)
        {
            railgunTimer += Time.deltaTime;


            playerOverheat.overheatValue += 15 * railgunTimer;
        }
  //      railgunLight.enabled = true;
     //   railgunLine.enabled = true;
 //       railgunLine.SetPosition(0, railgunSpawn.transform.position);

        shootRay.origin = railgunSpawn.transform.position;
        shootRay.direction = railgunSpawn.transform.forward;


      
            railgunDamage = railgunTimer * railgunDamageRatio;
        //    railgunTimer = 0.0f;
            if (Physics.Raycast(shootRay, out shoothit, railgunRange, shootableMask))
            {
                if (shoothit.collider.gameObject.tag == "Enemy")
                {
                    Debug.Log("Railgun hit enemy!");
                    Enemy enemy = shoothit.collider.gameObject.GetComponent<Enemy>();

                    enemy.m_nHealth -= (int)railgunDamage;
                }
 //               railgunLine.SetPosition(1, shoothit.point);

            }
            else
            {
 //               railgunLine.SetPosition(1, shootRay.origin + shootRay.direction * railgunRange);
            }
        
    }
}
