using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class test : MonoBehaviour
{
    public Button resetButton;

    GameObject[] weapons;
    private void Start()
    {
         resetButton.onClick.AddListener(Restart);

    }
    void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);

    }
}
