using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScriptForPlantEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1.11f;
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private float detectionRadius = 5.0f;  
    [SerializeField] private float attackRadius = 2.0f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] public Image healthBar;
    private Animator animator;  
    private float lastAttackTime = 0;  
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        ManageAnimations(distanceToPlayer);
    }

    void ManageAnimations(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackRadius && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack(distanceToPlayer);
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("AttackLeft"))
                {
                    animator.SetBool("Idle", true);
                }
            }
        }
        else
        {
            animator.SetBool("Idle", true);
        }
    }

    void Attack(float distanceToPlayer)
    {
        lastAttackTime = Time.time;
        bool playerIsLeft = playerTransform.position.x < transform.position.x;
        animator.SetTrigger(playerIsLeft ? "AttackLeft" : "Attack");
        StartCoroutine(DealDamageAfterDelay(0.6f, distanceToPlayer));  
    }

    IEnumerator DealDamageAfterDelay(float delay, float distanceToPlayer)
    {
        yield return new WaitForSeconds(delay);
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRadius)
        {
            if (playerTransform.GetComponent<ScriptForPlayerRest>() != null)
            {
                ScriptForPlayerRest script = playerTransform.GetComponent<ScriptForPlayerRest>();
                script.TakeDamage(2.5f - ScriptForPlayerRest.defensePoint);
            }
            if (playerTransform.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                NewScriptForPlayerSnow script = playerTransform.GetComponent<NewScriptForPlayerSnow>();
                script.TakeDamage(2.5f - NewScriptForPlayerSnow.defensePoint);
            }
        }
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("AttackLeft");
        if (Vector2.Distance(transform.position, playerTransform.position) > attackRadius)
        {
            animator.SetBool("Idle", true);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        StartCoroutine(ShowDamageEffect());
        currentHealth -= damageAmount;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(ShowDamageEffect());
    }

    IEnumerator ShowDamageEffect()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
