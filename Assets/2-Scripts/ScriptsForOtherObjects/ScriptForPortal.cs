using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForPortal : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    private bool isOpeningComplete = false;

    void Start()
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
    }
    
    public void OpenPortal()
    {
        gameObject.SetActive(true);
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Open");
            StartCoroutine(TransitionToIdle());
        }
    }
    IEnumerator TransitionToIdle()
    {
        yield return new WaitForSeconds(0.35f); 
        animator.SetBool("Idle", true);
        isOpeningComplete = true;
    }

    void Update()
    {
        if (isOpeningComplete)
        {

        }
    }
}