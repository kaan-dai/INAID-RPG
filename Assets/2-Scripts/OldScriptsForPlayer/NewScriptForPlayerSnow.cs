using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;

public class NewScriptForPlayerSnow : MonoBehaviour
{
    private DungeonMasterInfoCollector dungeonMaster;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 6.5f;
    [SerializeField] public static float attackDamage = 50.0f;
    [SerializeField] public static float defensePoint= 0;
    [SerializeField] private CameraFollowForSnow cameraFollow;
    [SerializeField] private Image healthBar;
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public static float currentHealth;
    [SerializeField] private float attackRange = 1.0f;  
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float offsetX = 0.2f;
    [SerializeField] private float offsetY;
    [SerializeField] private GameObject sceneName;
    private Transform interactableItem; 
    private InventoryManager inventoryManager;
    private float currentRoomFinishTime;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool facingRight = true;



    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        dungeonMaster = FindObjectOfType<DungeonMasterInfoCollector>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    void Update()
    {
        UpdateAnimationState();
        HandleTeleportation();
        HandleInteraction();
        
    }

    private void HandleTeleportation()
    {
        if (transform.position.x >= 16.2f && transform.position.x < 17)
        {
            currentRoomFinishTime = Time.time - cameraFollow.firstRoomStartTime;
            dungeonMaster.ChangeDifficultyForNextRoom(currentRoomFinishTime);

            transform.position = new Vector2(19.75f, -0.28f);
            cameraFollow.TeleportToMap(2);
            dungeonMaster.newRoom();

        }
        if (transform.position.x >= 52.3f && transform.position.x < 53)
        {
            currentRoomFinishTime = Time.time - currentRoomFinishTime;
            dungeonMaster.ChangeDifficultyForNextRoom(currentRoomFinishTime);

            transform.position = new Vector2(59.7f, transform.position.y);
            cameraFollow.TeleportToMap(3);
            dungeonMaster.newRoom();
        }

        if (transform.position.x >= 92.17f && transform.position.x < 93)
        {
            transform.position = new Vector2(94, -1.71f);
            cameraFollow.TeleportToMap(4);
            dungeonMaster.newRoom();
            AudioManager.Instance.StopMusic();  // Stop the current music first
            AudioManager.Instance.PlayBossRoomMusic();
        }
    }
    private void UpdateAnimationState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput != 0)
        {
            facingRight = moveInput > 0;
        }

        sprite.flipX = !facingRight;

        bool isMoving = Mathf.Abs(moveInput) > 0;
        animator.SetBool("Run", isMoving && isGrounded);
        animator.SetBool("Idle", !isMoving && isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("Jump", true);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }
    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableItem != null)
        {
            interactableItem.GetComponent<ScriptForHealthPotion>()?.PickUp();  
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthPotion") || other.CompareTag("Key"))
        {
            interactableItem = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == interactableItem)
        {
            interactableItem = null;
        }
    }
   public void TakeDamage(float damage)
    {
        StartCoroutine(FlashRedWhenDamaged());
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        sprite.color= Color.black;
        Debug.Log("Player Died");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", false);
            isGrounded = true;
        }
    }
    IEnumerator FlashRedWhenDamaged()
    {
    sprite.color = Color.red;
    yield return new WaitForSeconds(0.5f); 
    sprite.color = Color.white;  
    }

    public void RestoreHealth(float increaseAmount){
        if(currentHealth + increaseAmount >= maxHealth){
            currentHealth = maxHealth;
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        else{
            currentHealth += increaseAmount;
            healthBar.fillAmount = currentHealth / maxHealth;
        } 
    }
    public void IncreaseDamage(float increaseAmount){
        Debug.Log(attackDamage);
        attackDamage += increaseAmount;
        Debug.Log(attackDamage);
    }

    public void IncreaseDefense(float increaseAmount){
        Debug.Log(defensePoint);
        defensePoint += increaseAmount;
        Debug.Log(defensePoint);
    }
    public void PerformAttack()
    {
        Debug.Log("PerformAttack called");
        Vector3 attackPosition = transform.position + new Vector3(offsetX, offsetY, 0);
        Debug.Log($"Attack position: {attackPosition}");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayer);
        Debug.Log($"Found {enemiesToDamage.Length} enemies within range.");

        foreach (Collider2D enemy in enemiesToDamage)
        {
            Debug.Log($"Enemy detected with tag {enemy.tag}");
            if (enemy.CompareTag("Enemy"))
            {
                ScriptForEnemy enemyScript = enemy.GetComponent<ScriptForEnemy>();
                ScriptForFlyingEnemy flyingScript = enemy.GetComponent<ScriptForFlyingEnemy>();
                ScriptForGreenEnemy greenScript = enemy.GetComponent<ScriptForGreenEnemy>();
                ScriptForPlantEnemy plantScript = enemy.GetComponent<ScriptForPlantEnemy>();
                ScriptForPurpleEnemy purpleScript = enemy.GetComponent<ScriptForPurpleEnemy>();
                ScriptForRaEnemy raScript = enemy.GetComponent<ScriptForRaEnemy>();
                ScriptForRedHeadEnemy redScript = enemy.GetComponent<ScriptForRedHeadEnemy>();
                ScriptForSnakeManEnemy snakeScript = enemy.GetComponent<ScriptForSnakeManEnemy>();
                ScriptForBossCrystal crystalBoss = enemy.GetComponent<ScriptForBossCrystal>();
                ScriptForBossDesert desertBoss = enemy.GetComponent<ScriptForBossDesert>();
                ScriptForBossRocky rockyBoss = enemy.GetComponent<ScriptForBossRocky>();
                ScriptForBossSnow snowBoss = enemy.GetComponent<ScriptForBossSnow>();



                if (plantScript != null)
                    {
                    plantScript.TakeDamage(attackDamage);
                    }
                
                
                if (flyingScript != null)
                    {
                    flyingScript.TakeDamage(attackDamage);
                    }
                

                if (greenScript != null)
                    {
                    greenScript.TakeDamage(attackDamage);
                    }
                
                if (purpleScript != null)
                    {
                    purpleScript.TakeDamage(attackDamage);                   
                    }
                    
                if (raScript != null)
                    {
                    raScript.TakeDamage(attackDamage);                   
                    }
                
                if (redScript != null)
                    {
                    redScript.TakeDamage(attackDamage);                   
                    }
                
                if (snakeScript != null)
                    {
                    snakeScript.TakeDamage(attackDamage);                   
                    }
                
                if (crystalBoss != null)
                    {
                    crystalBoss.TakeDamage(attackDamage);                
                    }
                
                if (desertBoss != null)
                    {
                    desertBoss.TakeDamage(attackDamage);                  
                    }
                
                if (rockyBoss != null)
                    {
                    rockyBoss.TakeDamage(attackDamage);             
                    }

                if (snowBoss != null)
                    {
                    snowBoss.TakeDamage(attackDamage);                  
                    }     
                                                       
            }
        }
    }
}