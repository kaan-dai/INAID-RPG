using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptForPurpleEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform leftBound, rightBound;
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private float detectionRadius = 5.0f;  
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private float slowDownFactor = 0.3f;
    [SerializeField] private float slowDownDuration = 1.0f;
    [SerializeField] private float attackRadius = 2.0f;
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

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in the enemy script.");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is not attached to the enemy.");
        }
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
        
        // Calculate the target position but constrain it within the boundaries
        float targetX = playerTransform.position.x;
        targetX = Mathf.Clamp(targetX, leftBound.position.x + threshold, rightBound.position.x - threshold);
        
        // Only move towards the player if the target position is within the boundaries
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
        StartCoroutine(ResetAttackAnimation());

            if (playerTransform.GetComponent<ScriptForPlayerRest>() != null)
            {
                playerTransform.GetComponent<ScriptForPlayerRest>().TakeDamage(10);
            }
            if (playerTransform.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                playerTransform.GetComponent<NewScriptForPlayerSnow>().TakeDamage(10);
            }
        }
        else
        {
        Debug.LogError("Animator not found when trying to attack.");
        }
    }
    IEnumerator DealDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRadius)
        {
            if (playerTransform.GetComponent<ScriptForPlayerRest>() != null)
            {
                playerTransform.GetComponent<ScriptForPlayerRest>().TakeDamage(10);
            }
            if (playerTransform.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                playerTransform.GetComponent<NewScriptForPlayerSnow>().TakeDamage(10);
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
