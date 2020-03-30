using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    private Animator anim;

    public bool isShooting;
    public bool isDead;
    public bool hasWon;
    public bool isDamaged;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        StartCoroutine(Animate());
    }

    public IEnumerator Animate()
    {
        if (isShooting)
        {
            isShooting = false;
            //anim.Play("attack");
            anim.SetBool("isShooting", true);
            yield return new WaitForSeconds(1f);
            anim.SetBool("isShooting", false);
        }

        else if (isDead)
        {
            isDead = false;
            anim.SetBool("isDead", true);
            yield return new WaitForSeconds(0.5f);
        }

        else if (hasWon)
        {
            hasWon = false;
            anim.SetBool("hasWon", true);
            yield return new WaitForSeconds(0.5f);
        }

        else if (isDamaged)
        {
            isDamaged = false;
            anim.SetBool("isDamaged", true);
            yield return new WaitForSeconds(.5f);
            anim.SetBool("isDamaged", false);
        }
    }
}
