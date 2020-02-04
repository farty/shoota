using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSwitcher : MonoBehaviour
{
    public GameObject curWeapon;
    public GameObject weaponPosition;

    public GameObject weapon_first_slot;
    public GameObject weapon_second_slot;
    public GameObject weapon_third_slot;

    public Button uiAutoFireSwitcher;
    public Button uiWeaponSwitcher;
    void Start()
    {
        curWeapon = Instantiate(weapon_first_slot, weaponPosition.transform.position, weaponPosition.transform.rotation);
        curWeapon.GetComponent<Weapon>().owner = gameObject;
        curWeapon.transform.parent = weaponPosition.transform;
        curWeapon.GetComponent<Weapon>().isOnLock = true;

        uiAutoFireSwitcher.onClick.AddListener(SwitchFire);
        uiWeaponSwitcher.onClick.AddListener(SwitchWeapon);       
    }

    void Update()
    {
        PickupWeapon();
    }

    void SwitchFire()
    {

        if (curWeapon.GetComponent<Weapon>().isOnLock == true)
        {
            curWeapon.GetComponent<Weapon>().isOnLock = false;
        }
        else
        {
            curWeapon.GetComponent<Weapon>().isOnLock = true;
        }

    }
    void SwitchWeapon()
    {
        GameObject _weaponZero = weapon_first_slot;
        Destroy(curWeapon);
        if (weapon_second_slot != null)
        {
            weapon_first_slot = weapon_second_slot;
            weapon_second_slot = _weaponZero;
            curWeapon = Instantiate(weapon_first_slot, weaponPosition.transform.position, weaponPosition.transform.rotation);
            curWeapon.transform.parent = weaponPosition.transform;
        }
    }

    void PickupWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("weapon"))
                {
                    Debug.Log("weapon");
                    if(hit.collider.gameObject.GetComponent<Weapon>().owner == null)
                    {
                        Debug.Log("owner null");

                        if (weapon_first_slot == null)
                        {
                            weapon_first_slot = hit.collider.gameObject;
                            hit.collider.gameObject.SetActive(false);
                        }
                        else if (weapon_second_slot == null)
                        {
                            weapon_second_slot = hit.collider.gameObject;
                            hit.collider.gameObject.SetActive(false);
                        }
                        else if (weapon_third_slot == null)
                        {
                            weapon_third_slot = hit.collider.gameObject;
                            hit.collider.gameObject.SetActive(false);
                        }
                    }                    
                }
            }
        }
    }
}
