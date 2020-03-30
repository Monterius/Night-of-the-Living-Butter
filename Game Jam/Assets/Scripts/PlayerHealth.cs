using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator anim;

    public bool isLowPlayer;
    public bool notLowPlayer;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Animate());
    }

    public IEnumerator Animate()
    {
        if (isLowPlayer)
        {
            anim.SetBool("isLowHealth", true);
            yield return new WaitForSeconds(0.5f);
        }
        if (notLowPlayer)
        {
            anim.SetBool("isLowHealth", false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
