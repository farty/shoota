using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string title;
    public string weaponType;
    public float shootDistance;
    public float fireRate = 1;
    public float reloading;
    private float reloadingCur;
    private float curFireRate;
    public int bullets;
    private int bulletsCur;
    public int damage = 10;
    public int clips;
    public GameObject bullet;
    public GameObject bulletSpawnPosition;
    public AudioSource shootSound;
    public bool isOnLock = true;
    public GameObject owner;
    public Sprite uiIcon;
    GameObject manager;



    private void Start()
    {
      shootSound = GetComponent<AudioSource>();
      bulletsCur = bullets;
      reloadingCur = reloading;

        manager = GameObject.FindGameObjectWithTag("manager");



    }

    void Update()
    {
        Drop();
    }
        

    public void Shot()
    {
        if (isOnLock == false)
        {
            if (bulletsCur == 0)
            {
                Reload();
            }
            if (bulletsCur > 0)
            {
                if (curFireRate <= 0)
                {
                    shootSound.Play();
                    curFireRate = fireRate;
                    bulletsCur -= 1;
                    Instantiate(bullet, bulletSpawnPosition.transform.position, transform.rotation);
                    bullet.GetComponent<Bullet>().bulletDamage = damage;
                    bullet.GetComponent<Bullet>().owner = owner;
                    if (manager.GetComponent<AlertManager>().alarm == false)
                    {
                        manager.GetComponent<AlertManager>().SetAlarm();
                    }
                    else
                    {
                        manager.GetComponent<AlertManager>().approachPlayerPosition = transform.position;
                    }
                }
                else
                {
                    curFireRate -= Time.deltaTime;
                }
            }
            else
            {
                Reload();
            }
        }
    }
    void Reload()
    {
        if (reloadingCur <= 0)
        {
            bulletsCur = bullets;
            reloadingCur = reloading;
        }
        else reloadingCur -= Time.deltaTime;
    }           
    void Drop()
    {
        if (owner != null && owner.GetComponent<Stats>().curHealth <= 0)
        {
            owner = null;
            isOnLock = true;
            transform.parent = null;
        }
    }
}
