using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptForSnakeManEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] public Transform leftBound, rightBound;
    [SerializeField] public Transform playerTransform; 
    [SerializeField] private float detectionRadius = 5.0f;  
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] public Image healthBar;
    [SerializeField] private float slowDownFactor = 0.3f;
    [SerializeField] private float slowDownDuration = 1.0f;
    [SerializeField] private float attackRadius = 1.35f;
    [SerializeField] private float attackCooldown = 1.5f;
    private Animator animator;  
    private float lastAttackTime = 0; 
    

    private float threshold = 0.1f;
    private bool movingLeft = true;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalColor = sprite.color;
        rb.isKinematic = true;
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            FollowPlayer(distanceToPlayer);
            animator.SetBool("Walk", true);
        }
        else {
            MoveEnemy();
            animator.SetBool("Walk", false);

        }
    }
    void FollowPlayer(float distanceToPlayer)
{
    if (distanceToPlayer > attackRadius)
    {
        sprite.flipX = transform.position.x > playerTransform.position.x;
        
        float targetX = playerTransform.position.x;
        targetX = Mathf.Clamp(targetX, leftBound.position.x + threshold, rightBound.position.x - threshold);
        
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), moveSpeed * Time.deltaTime);
    }
    else if (Time.time >= lastAttackTime + attackCooldown)
    {
        Attack();  
    }
}


    void MoveEnemy()
    {
        if (movingLeft && transform.position.x > leftBound.position.x + threshold ||
            !movingLeft && transform.position.x < rightBound.position.x - threshold)
        {
            float currentSpeed = movingLeft ? -moveSpeed : moveSpeed;
            transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
            sprite.flipX = movingLeft;
        }
        else
        {
            movingLeft = !movingLeft;
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
    }
    void Attack()
    {
        if (animator != null)
        {
        animator.SetBool("Attack", true);
        lastAttackTime = Time.time;
        StartCoroutine(DealDamageAfterAnimation(0.35f));

        }
       
    }
    IEnumerator DealDamageAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRadius)
        {
            
           if (playerTransform.GetComponent<ScriptForPlayerRest>() != null)
            {
                ScriptForPlayerRest script = playerTransform.GetComponent<ScriptForPlayerRest>();
                script.TakeDamage(8 - ScriptForPlayerRest.defensePoint);
            }
            if (playerTransform.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                NewScriptForPlayerSnow script = playerTransform.GetComponent<NewScriptForPlayerSnow>();
                script.TakeDamage(8 - NewScriptForPlayerSnow.defensePoint);
            }
        }
        animator.SetBool("Attack", false);
    }
    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(1); 
        animator.SetBool("Attack", false);
    }   
    IEnumerator ShowDamageEffect()
    {
        sprite.color = Color.red;
        moveSpeed *= slowDownFactor;
        yield return new WaitForSeconds(slowDownDuration);
        sprite.color = originalColor;
        moveSpeed /= slowDownFactor;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}