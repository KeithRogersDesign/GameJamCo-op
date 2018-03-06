using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float bulletSpeed = 300.0f;
    public int bulletDamage = 3;
    public GameObject bulletPrefab;

    private GameObject bulletspawn;
	// Use this for initialization
	void Start () {
        bulletspawn = GameObject.Find("BulletSpawn");
        this.gameObject.transform.forward = bulletspawn.transform.forward;
       
    }
	
	// Update is called once per frame
	void Update () {

        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed * Time.deltaTime);
        
	}

    void fire()
    {
       
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Enemy")
        {

        }
    }
}
