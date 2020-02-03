using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float fieldOfViewAngle = 110f;
    public float retreatDistance = 5f;
    private bool playerPositioinIdentified = false;
    GameObject manager;
    public GameObject patrolPosition;
    Vector3 startPosition;
    Vector3 _patrolPosition;
        

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<WeaponCarrier>().weapon.GetComponent<Weapon>().isOnLock = false;
        manager = GameObject.FindGameObjectWithTag("manager");
        startPosition = transform.position;
        _patrolPosition = patrolPosition.transform.position;
    }

    void Update()
    {
        IdentifyPlayerPosition();
        CombatState();
        PatrolingState();
    }
    void CombatState()
    {
        if (playerPositioinIdentified == true)
        {
            RotateToPlayer();
            DistanceToPlayer();
        }
        
    }

    void IdentifyPlayerPosition()
    {
        Vector3 direction = player.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, 1000000f))
        {
            if (hit.transform.gameObject == player)
            {
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < fieldOfViewAngle / 2)
                {

                    manager.GetComponent<AlertManager>().approachPlayerPosition = player.transform.position;
                    playerPositioinIdentified = true;

                    GetComponent<WeaponCarrier>().weapon.GetComponent<Weapon>().Shot();
                    manager.GetComponent<AlertManager>().SetAlarm();

                }
            }
        }
        if (player.GetComponent<Player>().noiseLevel > Vector3.Distance(transform.position, player.transform.position))
        {

            playerPositioinIdentified = true;

            manager.GetComponent<AlertManager>().SetAlarm();
            manager.GetComponent<AlertManager>().approachPlayerPosition = player.transform.position;
        }
    }

    void RotateToPlayer()
    {
            Vector3 direction = (manager.GetComponent<AlertManager>().approachPlayerPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
    }


    void DistanceToPlayer()
    {        
            RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000000f))
        {
            if (hit.transform.gameObject == player)
            {
                if (Vector3.Distance(transform.position, player.transform.position) >= retreatDistance)
                {
                    HoldPosition();
                }
                else
                {
                    Retreat();
                }
            }
            else
            {
                Chase();
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
        if (Vector3.Distance(transform.position, player.transform.position) <= GetComponent<WeaponCarrier>().weapon.GetComponent<Weapon>().shootDistance)
        {
            agent.SetDestination(transform.position);
        }
    }
    void Chase()
    {
        agent.SetDestination(manager.GetComponent<AlertManager>().approachPlayerPosition);
    }
    public void Alarm()
    {
        playerPositioinIdentified = true;
    }
    void PatrolingState()
    {
        if (playerPositioinIdentified == false)
        {
            bool weCanGoForward = false;
            bool weCanGoBack = false;
            if (Vector3.Distance(transform.position, startPosition) < 0.2)
            {
                weCanGoForward = true;
                weCanGoBack = false;
            }
            if (Vector3.Distance(transform.position, _patrolPosition) < 0.2)
            {
                weCanGoForward = false;
                weCanGoBack = true;
            }
            if (weCanGoForward == true)
            {
                agent.SetDestination(_patrolPosition);
            }
            if (weCanGoBack == true)
            {
                agent.SetDestination(startPosition);
            }

        }
            
    }
    
    
}

