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

    Vector3 movement;
    private Vector3 direction;
    public float speed = 2;
    public Joystick joystick;

    public GameObject curWeapon;
    public GameObject weaponPosition;
    void Start()
    {
        gameCamera = Instantiate(gameCamera, transform.position, Quaternion.identity);
        rb = GetComponent<Rigidbody>();
        curWeapon = Instantiate(curWeapon, weaponPosition.transform.position, weaponPosition.transform.rotation);
        curWeapon.GetComponent<Weapon>().owner = gameObject;
        curWeapon.transform.parent = weaponPosition.transform;
    }
    void Update()
    {

        JoystickMovement();
        FindClosestEnemy();
        CameraMotor();

    }
    void JoystickMovement()
    {

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        rb.MovePosition(rb.position + input * speed);
        if (input.x!=0 || input.z !=0)
        {
            float xRot = transform.rotation.x + input.x;
            float zRot = transform.rotation.z + input.z;
            Quaternion moveRotation = Quaternion.LookRotation(new Vector3(xRot, 0, zRot));
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, 5 * Time.deltaTime);
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
                        topPart.transform.rotation = Quaternion.Lerp(topPart.transform.rotation, lookRotation, 50 * Time.deltaTime);
                        curWeapon.GetComponent<Weapon>().Shot();
                    }

                    else
                    {
                        topPart.transform.rotation = Quaternion.Lerp(topPart.transform.rotation, transform.rotation, 10 * Time.deltaTime);
                    }
                }
            }
            else
            {
                topPart.transform.rotation = Quaternion.Lerp(topPart.transform.rotation, transform.rotation, 10 * Time.deltaTime);

            }
        }
        else
        {
            topPart.transform.rotation = Quaternion.Lerp(topPart.transform.rotation, transform.rotation, 10 * Time.deltaTime);

        }
    }    
    void CameraMotor()
    {
        gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, transform.position, 24);
    }   
}



