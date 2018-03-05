using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    private xbox_gamepad gamepad;

    public GameObject bulletPrefab;
    public GameObject bulletSpawnPosition;

    public float machineGunFireRate = 40.0f;
    public float gunCoolDown = 0.1f;
    private float overHeatValue;
    private GameObject upperBody;

    private float gunCoolDownCpy;

	// Use this for initialization
	void Start () {
        gamepad = GamePadManager.Instance.GetGamePad(2);
        upperBody = GetComponent<PlayerController>().upperBody;
        gunCoolDownCpy = gunCoolDown;

	}
	
	// Update is called once per frame
	void Update () {

        if(gunCoolDown > 0)
        {
            gunCoolDown -= Time.deltaTime;
        }

        shoot();

	}

    void shoot()
    {
        if(gamepad.GetTriggerDown_R() && gunCoolDown <= 0)
        {
            gunCoolDown = gunCoolDownCpy;
            Instantiate(bulletPrefab, bulletSpawnPosition.transform.position, upperBody.transform.rotation);

        }
    }
}
