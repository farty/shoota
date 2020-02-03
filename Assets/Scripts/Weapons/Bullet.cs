using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 5;
    public float speed;
    public GameObject owner;
    public float lifetime = 1;
    Vector3 mPrevPos;
    void Start()
    {
        {
            Destroy(gameObject, lifetime);
            mPrevPos = transform.position;
        }
    }
    void Update()
    {
        mPrevPos = transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos, (transform.position - mPrevPos).normalized), (transform.position - mPrevPos).magnitude);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject != null)
            {
                Destroy(gameObject);
                if (hits[i].transform.gameObject.GetComponent<Stats>() != null)
                {
                    hits[i].transform.gameObject.GetComponent<Stats>().TakeDamage(bulletDamage);
                }

            }
        }
        Debug.DrawLine(transform.position, mPrevPos, Color.red);

    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

}
