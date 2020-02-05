using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    private Rigidbody rb;
    public GameObject topPart;
    public GameObject gameCamera;
    public GameObject SwitchButton;
    GameObject manager;

    Vector3 movement;
    private Vector3 direction;
    public float speed = 2;
    public float noiseLevel;
    public Joystick joystick;

    void Start()
    {
        gameCamera = Instantiate(gameCamera, transform.position, Quaternion.identity);
        rb = GetComponent<Rigidbody>();
        noiseLevel = 0;
        manager = GameObject.FindGameObjectWithTag("manager");
    }
    void Update()
    {

        JoystickMovement();
        FindClosestEnemy();
        CameraMotor();
        Noize();

    }
    void JoystickMovement()
    {
        movement.x = joystick.Horizontal;
        movement.z = joystick.Vertical;
        rb.MovePosition(rb.position + movement * speed);

        direction = new Vector3(movement.x, 0, movement.z);
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1);
        }

    }
    void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        
        foreach (Enemy currentEnemy in allEnemies)
        {
            if (currentEnemy != null)
            {
                float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }
            }
            
        }

        if (closestEnemy != null)
        {
            Vector3 targetDir = closestEnemy.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);
            if (angle < 90.0f)
            {
                Vector3 direction = closestEnemy.transform.position - transform.position;
                RaycastHit hit;
                if (Physics.Raycast(transform.position+transform.up, direction.normalized, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("enemy") == true)
                    {
                        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 50 * Time.deltaTime);
                        GetComponent<WeaponSwitcher>().curWeapon.GetComponent<Weapon>().Shot();
                        manager.GetComponent<EventSystem>().approachPlayerPosition = transform.position;
                    }
                }
            }
        }          
     }    
    void CameraMotor()
    {
        gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, transform.position, 24);
    }
    void Noize()
    {
        noiseLevel = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.z))*10;
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noiseLevel);

    }
       
}



