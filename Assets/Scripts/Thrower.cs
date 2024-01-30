using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Thrower : MonoBehaviour
{
   

    [Header("References")]
    public GameObject projectile;
    public Transform throwStartPoint;
    public Camera cam;
    public Transform aimLine;
    public Target target;
    public Animator animator;
    public Transform pie;
    public Animator pieAnimator;

    [Header("Throwing")]
    public float throwForce;
    public float forwardThrowForce = 10f;

    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 throwDirection;
    Quaternion aimDirection;
    bool canThrow = true;

   
    // Update is called once per frame
    void Update()
    {
        if (target.isGameOver)
        {
            HideAimAssist();
            transform.localScale = Vector3.one;
            return;
        }
        if (transform.localScale == Vector3.one)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
       
        DragThrowPie();

    }

    /// <summary>
    /// Spawns and throws a new cake with a horizontal and vertical force.
    /// </summary>
    public void Throw()
    {
        canThrow = false;
        pieAnimator.SetBool("Hidden", true);

        GetComponent<AudioSource>().Play();

        GameObject cake = Instantiate(projectile, throwStartPoint.position, transform.rotation);    
        Vector3 force = aimDirection * throwStartPoint.forward * throwForce;
        Rigidbody cakeRb = cake.GetComponent<Rigidbody>();
        Transform cakeTransform = cake.GetComponent<Transform>();
        cakeTransform.rotation = aimDirection;
        cakeRb.AddForce(force, ForceMode.Impulse);
        animator.SetTrigger("Throw");

        StartCoroutine(ThrowCooldown());
        
    }

    public IEnumerator ThrowCooldown()
    {
        yield return new WaitForSeconds(1f);
        pieAnimator.SetBool("Hidden", false);
        canThrow = true;
    }
    public void TrowCoroutine()
    {
        StartCoroutine(ThrowCooldown());
    }

    void DragThrowPie()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        if (Input.GetMouseButtonDown(0))
        {

            startPoint = cam.ScreenToWorldPoint(mousePosition);
            startPoint.z = 15;
        }
        if (Input.GetMouseButton(0))
        {
            endPoint = cam.ScreenToWorldPoint(mousePosition);
            endPoint.z = 15;
            throwDirection = startPoint - endPoint;
            throwDirection.z = forwardThrowForce;
            throwDirection = throwDirection.normalized;
            aimDirection = Quaternion.FromToRotation(transform.forward, throwDirection);
            DrawAimAssist();
        }
        if (!canThrow)
        {
            return;
        }
        
           
        
        if (Input.GetMouseButtonUp(0))
        {
            Throw();
            aimDirection = Quaternion.identity;
            HideAimAssist();
        }

    }

    public void DrawAimAssist()
    {
        aimLine.SetPositionAndRotation(throwStartPoint.position, aimDirection);
    }

    public void HideAimAssist()
    {
        aimLine.position = new Vector3(1000, 1000, 1000);
    }

}
