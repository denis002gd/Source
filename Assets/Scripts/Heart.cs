using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public void SetHearts(int pieces)
    {
        for (int i=0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //hide all
        if (pieces == 0)
        {
            return; 
        }
        for (int i=0; i < pieces; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        
        }
    }
        
}
