using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    GameObject player;
    public GameObject uiSwitchAutoFire;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        MouseClick();
    }
    void MouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {


                if (hit.collider.gameObject == uiSwitchAutoFire)
                {
                    Debug.Log("button pressed");

                   // SwitchAutoFire();
                }
            }
        }
    }
    /* void SwitchAutoFire()
     {
         if (player.GetComponent<WeaponCarrier>().weapon.GetComponent<Weapon>() != null)
         {
             Debug.Log("!=null");
             if (player.GetComponent<WeaponCarrier>().weapon.GetComponent<Weapon>().isOnLock == false)
             {
                 player.GetComponent<WeaponCarrier>().weapon.GetComponent<Weapon>().isOnLock = true;
                 Debug.Log("true");

             }
             else
             {
                 player.GetComponent<WeaponCarrier>().weapon.GetComponent<Weapon>().isOnLock = false;
                 Debug.Log("false");

             }
         }
     }*/


}
