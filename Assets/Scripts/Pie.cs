using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pie : MonoBehaviour
{
   
    void Start()
    {

        Invoke(nameof(Suicide), 5f);
    }

    void Suicide()
    { 
        
        Destroy(gameObject);
    }
    
}
