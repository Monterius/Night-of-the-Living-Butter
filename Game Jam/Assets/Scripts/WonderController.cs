using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderController : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    public bool canMove;
    public bool invincible = false;

    public Unit Wonder;
    BattleHUDBattle playerHUD;
    public GameHandler Handler;

    private void Start()
    {
        playerHUD = GameObject.FindGameObjectWithTag("Phealth").GetComponent<BattleHUDBattle>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Wonder.currentHP > 0)
        {
            // Input
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        
    }

    private void FixedUpdate()
    {
        if (Wonder.currentHP > 0)
        {
            // Movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                if (!invincible)
                {
                    SoundManager.PlaySound(SoundManager.Sound.Hurt);
                    bool isDead = Wonder.TakeDamage(25);
                    playerHUD.SetHP(Wonder.currentHP);
                    if (isDead)
                    {
                        Handler.Dead = true;
                    }
                    invincible = true;
                    yield return new WaitForSeconds(1);
                    invincible = false;
                }
                break;
                
        }
    }
}
