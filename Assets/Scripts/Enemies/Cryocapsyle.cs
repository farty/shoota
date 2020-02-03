using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cryocapsyle : MonoBehaviour
{
    private GameObject manager;
    public GameObject unitInside;
    public bool isDeactivated;
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
    }

    void Update()
    {
        Unfreeze();
    }

    void Unfreeze()
    {
        if (manager.GetComponent<AlertManager>().alarm == true)
        {
            if(isDeactivated == false)
            {
                Destroy(gameObject);
                Instantiate(unitInside, transform.position, Quaternion.identity);
            }
        }
        
    }

    public void Deactivate()
    {
        isDeactivated = true;
    }
}
