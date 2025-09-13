using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScriptForPlayerSnow : MonoBehaviour
{
    
    [SerializeField] private float speed = 5.0f; 
    [SerializeField] private float smallMoveAmount = 0.5f; 
    [SerializeField] private float jumpForce = 6.5f; 
    [SerializeField] private CameraFollowForSnow cameraFollow;
    [SerializeField] private Image healthBar; 
    [SerializeField] private float maxHealth = 100;
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
        if (transform.position.x >= 16.2f && transform.position.x < 17)
        {
            transform.position = new Vector2(19.75f, -0.28f);
            cameraFollow.TeleportToMap(2);
        }
        if (transform.position.x >= 52.3f && transform.position.x < 53)
        {
            transform.position = new Vector2(59.7f, transform.position.y);
            cameraFollow.TeleportToMap(3);
        }
    }

    
    private void UpdateAnimationState(){
        
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("Run", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Jump", false);
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack"); 
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            
            rb.MovePosition(rb.position + Vector2.right * smallMoveAmount);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            
            rb.MovePosition(rb.position + Vector2.left * smallMoveAmount);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            sprite.flipX = false;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            sprite.flipX = true;

        }
        
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
             animator.SetBool("Run", false);
             animator.SetBool("Idle", true);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);
            

        }
        else if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);

        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Run", false);
            animator.SetBool("Jump", true);
            
            
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false; 
 
        }
        if (!isGrounded)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Jump", true);
  
        }  

        if (!isGrounded && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            animator.SetBool("Run", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Jump", true);
  
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
            //Debug.Log("Grounded");
        }
    }
}
