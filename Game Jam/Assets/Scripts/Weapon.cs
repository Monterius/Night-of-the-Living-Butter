using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Weapon : MonoBehaviour
{
    CharacterAnim Wonder;
    public Unit WonderUnit;

    public float offset;
    public float bulletForce = 20f;

    public SpriteRenderer mySpriteRenderer;
    public SpriteRenderer hand1;
    public SpriteRenderer hand2;

    public GameObject projectile;
    public Transform shotPoint;

    public bool canAim;
    public bool canShoot;

    //public Transform effect;

    private void Start()
    {
        Wonder = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAnim>();
    }

    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (WonderUnit.currentHP > 0)
        {
            HandleAiming();
            HandleShooting();
        }
    }

    private void HandleAiming()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0f, 0f, rotZ);

        if (Mathf.Abs(rotZ) > 90)
        {
            mySpriteRenderer.flipY = true;
            hand1.flipY = true;
            hand2.flipY = true;
        }
        else if (Mathf.Abs(rotZ) < 90)
        {
            mySpriteRenderer.flipY = false;
            hand1.flipY = false;
            hand2.flipY = false;
        }
    }

    private void HandleShooting()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            SoundManager.PlaySound(SoundManager.Sound.Shot);
            GameObject bullet = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(shotPoint.right * bulletForce, ForceMode2D.Impulse);
            //Destroy(bullet, 1.5f);
        }
    }
}
