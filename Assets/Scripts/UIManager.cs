using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button restart;
    public GameObject lostUi;
    public Target target;
    void Start()
    {
        restart.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    void Update()
    {
        if(target.isGameOver == true)
        {
            lostUi.SetActive(true);
        }
        else
        {
            lostUi.SetActive(false);
        }
            
    }
    void Restart()
    {
        target.RestartGame();
    }
}
