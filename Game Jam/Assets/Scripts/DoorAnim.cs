using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    private Animator anim;

    public bool isOpen;

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
        if (isOpen)
        {
            isOpen = false;
            //anim.Play("attack");
            anim.SetBool("isOpen", true);
            yield return new WaitForSeconds(1f);
        }
    }
}
