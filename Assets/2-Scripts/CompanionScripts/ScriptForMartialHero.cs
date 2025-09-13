using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForMartialHero : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public LayerMask groundLayer; 
    public Transform groundCheck;
    public float checkRadius = 0.1f; 

    private Transform target;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        CheckGroundStatus();
        HandleMovement();
        UpdateAnimationState();
    }

    void CheckGroundStatus()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        // Additional logging to see the status when changing
        if (wasGrounded != isGrounded)
        {
            Debug.Log($"Grounded State Changed: {isGrounded} at position {groundCheck.position}");
        }
    }

    void UpdateAnimationState()
    {
        animator.SetBool("Jump", !isGrounded);
        if (isGrounded)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            bool isRunning = distance > stoppingDistance;
            animator.SetBool("Run", isRunning);
            animator.SetBool("Idle", !isRunning);
        }
    }

    void HandleMovement()
    {
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            sprite.flipX = target.position.x < transform.position.x;
        }
    }

}
