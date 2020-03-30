using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed;
    int countdownTime = 40;
    GameObject target;
    public GameObject deathEffect;
    Vector3 directionToTarget;

    private Rigidbody2D rb;

    private Unit Butter;

    GameHandler handler;
    public BattleHUDBattle playerHUD;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        handler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();

        rb = GetComponent<Rigidbody2D>();

        Butter = GetComponent<Unit>();

        StartCoroutine(Countdown(countdownTime));
    }

    // Update is called once per frame
    void Update()
    {
        MoveButter();
    }

    void MoveButter()
    {
        directionToTarget = (target.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(directionToTarget.x * moveSpeed, directionToTarget.y * moveSpeed);
    }

    IEnumerator Countdown(int time)
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);

            time--;
        }
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
        handler.EnemiesLeft -= 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bullet":
                bool isDead = Butter.TakeDamage(50);
                Debug.Log("Butter took damage HP: " + Butter.currentHP);
                if (isDead)
                {
                    SoundManager.PlaySound(SoundManager.Sound.Explode);
                    GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 1f);
                    Destroy(gameObject);
                    handler.EnemiesLeft -= 1;
                }
                break;
        }
    }
}
