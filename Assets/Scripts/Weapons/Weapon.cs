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
    public GameObject bullet;
    public GameObject bulletSpawnPosition;
    public GameObject owner;
    public Sprite uiIcon;
    GameObject manager;



    private void Start()
    {
        bulletsCur = bullets;
        reloadingCur = reloading;
        manager = GameObject.FindGameObjectWithTag("manager");
    }

    

    public void Shot()
    {
        if (weaponType == "melee")
        {
            if (curFireRate <= 0)
            {
                curFireRate = fireRate;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
                foreach(Collider hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Player"))
                    {
                        hitCollider.GetComponent<Stats>().TakeDamage(damage); 
                    }
                }
            }
            else
            {
                curFireRate -= Time.deltaTime;
            }
        }

        if(weaponType == "pistol")
        {
            if (bulletsCur == 0)
            {
                Reload();
            }
            if (bulletsCur > 0)
            {
                if (curFireRate <= 0)
                {
                    curFireRate = fireRate;
                    bulletsCur -= 1;
                    Instantiate(bullet, bulletSpawnPosition.transform.position, transform.rotation);
                    bullet.GetComponent<Bullet>().bulletDamage = damage;
                    bullet.GetComponent<Bullet>().owner = owner;

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
}
