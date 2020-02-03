
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCarrier : MonoBehaviour
{
    public GameObject weapon;
    public GameObject weaponPosition;
    void Start()
    {
        weapon = Instantiate(weapon, weaponPosition.transform.position, weaponPosition.transform.rotation);
        weapon.GetComponent<Weapon>().owner = gameObject;
        weapon.transform.parent = weaponPosition.transform;
        if (weapon.GetComponent<Weapon>().owner.tag == "Player")
        {
            weapon.GetComponent<Weapon>().isOnLock = true;
        }
    }
}
