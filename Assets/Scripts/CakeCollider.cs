using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CakeCollider : MonoBehaviour
{

    public Target target;
    public Target.HitType hitType;

    public void Update()
    {
        
        if (target.CurrentTarget() == hitType)
        {
            HighlightOn();
        } else
        {
            HighlightOff();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pie"))
        {
            target.OnHit(hitType, collision.contacts[0]);
            collision.gameObject.tag = "InactivePie";
        }
    }

    private void HighlightOn() {

        GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
    }
    private void HighlightOff() {
        GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }
}
