using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Audience : MonoBehaviour
{
    public Animator spectatorAnim;
    private GameObject trg;
    private Target target;
    private float randomSeconds;

    private void Start()
    {
        spectatorAnim = GetComponent<Animator>();
        trg = GameObject.Find("Target");
        target = trg.GetComponent<Target>();

    
        StartCoroutine(PlayAnimations());
    }

    private void Update()
    {
      
    }

    public void DanceNow()
    {
        spectatorAnim.SetBool("Lost", false);
    }

    public void GetMad()
    {
        spectatorAnim.SetBool("Lost", true);
    }

    private IEnumerator PlayAnimations()
    {
        while (true)
        {
         
            yield return new WaitForSeconds(Random.Range(0.3f, 4f));

            if (target.canDance == true)
            {
                DanceNow();
            }
            else
            {
                GetMad();
            }
        }
    }
}
