using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptForPlayerCrystal : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float smallMoveAmount = 0.5f;
    [SerializeField] private float jumpForce = 6.5f;
    [SerializeField] private CameraFollowForRest cameraFollow;
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float attackRange = 1.0f;  
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float offsetX = 0.2f;
    [SerializeField] private float offsetY;

    
    
    private float currentHealth;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer sprite;

    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        UpdateAnimationState();
        HandleTeleportation();
    }
    private void HandleTeleportation()
            {
                if(transform.position.x >= 10.95f & transform.position.x < 11.5f){
                    transform.position = new Vector2(15.1f, transform.position.y);
                    cameraFollow.TeleportToMap(2); 
                }
                if(transform.position.x >= 37.02f & transform.position.x < 38){
                    transform.position = new Vector2(40.97f, transform.position.y);
                    cameraFollow.TeleportToMap(3); 
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.MovePosition(rb.position + Vector2.right * smallMoveAmount);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            rb.MovePosition(rb.position + Vector2.left * smallMoveAmount);
        }

        bool isMoving = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);
        rb.velocity = new Vector2((Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0) * speed, rb.velocity.y);
        sprite.flipX = Input.GetKey(KeyCode.A);

        animator.SetBool("Run", isMoving && isGrounded);
        animator.SetBool("Idle", !isMoving && isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("Jump", true);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", false);
            isGrounded = true;
        }
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
                Debug.Log("Destroying enemy");
                Destroy(enemy.gameObject);
            }
            else
            {
                Debug.Log("Detected object is not tagged as Enemy");
            }
        }
    }
}

