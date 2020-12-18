using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float fieldOfViewAngle = 110f;
    public float retreatDistance = 5f;
    public bool playerPositioinIdentified = false;
    public GameObject patrolPosition;
    Vector3 startPosition;
    Vector3 _patrolPosition;
    public GameObject curWeapon;
    public GameObject weaponPosition;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        curWeapon = Instantiate(curWeapon, weaponPosition.transform.position, weaponPosition.transform.rotation);
        curWeapon.transform.parent = weaponPosition.transform;

        curWeapon.GetComponent<Weapon>().owner = gameObject;

        startPosition = transform.position;
        _patrolPosition = patrolPosition.transform.position;
        

    }

    void Update()
    {
        IdentifyPlayerPosition();
        DistanceToPlayer();

    }
    void IdentifyPlayerPosition()
    {
        Vector3 direction = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction.normalized, out hit, 1000000f))
        {
            if (hit.transform.gameObject == player)
            {
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < fieldOfViewAngle / 2)
                {
                    curWeapon.GetComponent<Weapon>().Shot();
                }
            }
        }
    }   
    void DistanceToPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < retreatDistance)
        {
            Retreat();
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, 1000000f))
            {

                if (hit.transform.gameObject == player)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) >= retreatDistance)
                    {

                        if (Vector3.Distance(transform.position, player.transform.position) > curWeapon.GetComponent<Weapon>().shootDistance)
                        {
                            Chase();
                        }

                        if (Vector3.Distance(transform.position, player.transform.position) <= curWeapon.GetComponent<Weapon>().shootDistance)
                        {
                            HoldPosition();
                        }
                    }

                    else
                    {
                        Chase();
                    }
                }
                else
                {
                    Chase();
                }
            }
        }
        
        
    }
    void Retreat()
    {
        if (Vector3.Distance(transform.position, player.transform.position)< retreatDistance)
        {
            agent.SetDestination(transform.position + (transform.position - player.transform.position));

        }
    }

    void HoldPosition()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= curWeapon.GetComponent<Weapon>().shootDistance)
        {
            agent.SetDestination(transform.position);

        }
    }
    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }
}

