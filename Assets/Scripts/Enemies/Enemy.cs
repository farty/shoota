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
    public GameObject curWeapon;
    public GameObject weaponPosition;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("manager");

        curWeapon = Instantiate(curWeapon, weaponPosition.transform.position, weaponPosition.transform.rotation);
        curWeapon.transform.parent = weaponPosition.transform;

        curWeapon.GetComponent<Weapon>().owner = gameObject;
        curWeapon.GetComponent<Weapon>().isOnLock = false;

        startPosition = transform.position;
        _patrolPosition = patrolPosition.transform.position;
        

    }

    void Update()
    {
        IdentifyPlayerPosition();
        CombatState();
        PatrolingState();
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

                    manager.GetComponent<EventSystem>().approachPlayerPosition = player.transform.position;
                    playerPositioinIdentified = true;

                    curWeapon.GetComponent<Weapon>().Shot();
                    manager.GetComponent<EventSystem>().SetAlarm();

                }
            }
        }
        if (player.GetComponent<Player>().noiseLevel > Vector3.Distance(transform.position, player.transform.position))
        {

            playerPositioinIdentified = true;

            manager.GetComponent<EventSystem>().SetAlarm();
            manager.GetComponent<EventSystem>().approachPlayerPosition = player.transform.position;
        }
    }
    void CombatState()
    {
        if (playerPositioinIdentified == true)
        {
            RotateToPlayer();
            DistanceToPlayer();
        }

    }
    void RotateToPlayer()
    {
            Vector3 direction = (manager.GetComponent<EventSystem>().approachPlayerPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
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
                        HoldPosition();
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
        agent.SetDestination(manager.GetComponent<EventSystem>().approachPlayerPosition);
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

