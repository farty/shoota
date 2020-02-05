using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cryocapsyle : MonoBehaviour
{
    private GameObject manager;
    public GameObject unitInside;
    private GameObject player;
    public bool isDeactivated;
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        Unfreeze();
        Deactivate();
    }

    void Unfreeze()
    {
        if (manager.GetComponent<EventSystem>().alarm == true)
        {
            if(isDeactivated == false)
            {
                Destroy(gameObject);
                Instantiate(unitInside, transform.position, Quaternion.identity);
                unitInside.GetComponent<Enemy>().Alarm();
            }
        }
        
    }

    public void Deactivate()
    {
        if(isDeactivated == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= 3)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            isDeactivated = true;
                            Debug.Log(gameObject.name + "deactivated");
                        }
                    }
                }
            }
        }
    }        
}
