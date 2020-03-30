using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;

    //public GameObject gun;

    //public GameObject projectile;
    //public Transform shotPoint;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        //aimTransform = GameObject.FindGameObjectWithTag("Aim").GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        Debug.Log(angle);
    }

    /*private void HandleAiming()
    {

    }
    private void HandleShooting()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            //Instantiate(projectile, shotPoint.position, transform.rotation);
        }
    }*/
}
