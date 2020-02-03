using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    GameObject player;
    public float playersNoiseLevel = 0;
    private AudioSource alarmSound;
    public bool alarm = false;
    public Vector3 approachPlayerPosition;
    public bool PowerSwitchOn = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        alarmSound = GetComponent<AudioSource>();
    }
    public void SetAlarm()
    {
        if (alarm == false)
        {
            alarmSound.Play();
            alarm = true;
            approachPlayerPosition = player.transform.position;
            GameObject[] alarmTarget = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject target in alarmTarget)
            {
                if (target.GetComponent<Enemy>() != null)
                {
                    target.GetComponent<Enemy>().Alarm();
                }                
            }
        }
    }
}
