using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptForBossSnow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform leftBound, rightBound;
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private float detectionRadius = 7.0f;  
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] public float currentHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private float slowDownFactor = 0.3f;
    [SerializeField] private float slowDownDuration = 1.0f;
    [SerializeField] private float attackRadius = 3.75f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private GameObject healthPotionPrefab; 
    [SerializeField] private GameObject banditArmor; 
    [SerializeField] private GameObject leviathenAxe;
    private Animator animator;  
    private float lastAttackTime = 0;  
    private float threshold = 0.1f;
    private bool movingLeft = true;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Color originalColor;
    private bool isDying = false; 
    private Collider2D colliderComponent; 
    private DungeonMasterInfoCollector dungeonMaster;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        colliderComponent = GetComponent<Collider2D>();
        originalColor = sprite.color;
        rb.isKinematic = true;
        currentHealth = maxHealth;
        dungeonMaster = FindObjectOfType<DungeonMasterInfoCollector>();

    }

    void Update()
    {
        if (isDying) return; // Stop any updates when the boss is dying

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            FollowPlayer(distanceToPlayer);
            animator.SetBool("Fly", true);
        }
        else 
        {
            MoveEnemy();
            animator.SetBool("Fly", false);
        }
    }

    void FollowPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRadius)
        {
            sprite.flipX = transform.position.x > playerTransform.position.x;
            float targetX = Mathf.Clamp(playerTransform.position.x, leftBound.position.x + threshold, rightBound.position.x - threshold);
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
        if (isDying) return; // Do not take damage if dying

        StartCoroutine(ShowDamageEffect());
        currentHealth -= damageAmount;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0 && !isDying)
        {
            Die();
        }
    }

    void Attack()
    {
        if (isDying) return; // Do not attack if dying

        if (animator != null)
        {
            animator.SetBool("Attack", true);
            lastAttackTime = Time.time;
            StartCoroutine(DealDamageAfterAnimation(1f));
        }
        else
        {
            Debug.LogError("Animator not found when trying to attack.");
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
                script.TakeDamage(10 - ScriptForPlayerRest.defensePoint);
            }
            if (playerTransform.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                NewScriptForPlayerSnow script = playerTransform.GetComponent<NewScriptForPlayerSnow>();
                script.TakeDamage(10 - NewScriptForPlayerSnow.defensePoint);
            }
        }
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
        if (!isDying)
        {
            isDying = true;
            dungeonMaster.killedBoss();
            colliderComponent.enabled = false; 
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            animator.SetTrigger("Death"); 
            healthBar.gameObject.SetActive(false);
            healthPotionPrefab.SetActive(true);
            healthPotionPrefab.transform.position = new Vector3(111f, -2.13f, transform.position.z);
            banditArmor.SetActive(true);
            leviathenAxe.SetActive(true);
            ShowPortal();
            
            
        }
    }

    private void ShowPortal()
    {
        if (portalPrefab)
        {
            portalPrefab.SetActive(true);
            portalPrefab.transform.position = new Vector3(114.0f, -1f, transform.position.z);
            ScriptForPortal portalscript = portalPrefab.GetComponent<ScriptForPortal>();
            if (portalscript != null)
            {
                portalscript.OpenPortal();
            }
            else
            {
                Debug.LogError("Portal script not found on portal object!");
            }
        }
    }
    
}
