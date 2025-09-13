using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpSpeed;

    public float groundSpeed;

    float xInput;
    float yInput;

    [Range(0f, 1f)]
    public float groundDecay;

    public bool grounded;

    public Rigidbody2D body;

    public LayerMask groundMask;

    public BoxCollider2D groundCheck;

    void Start()
    {

    }

    void Update()
    {
        GetInput();
        MoveWithInput();
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
    }

    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void MoveWithInput()
    {
        if (Mathf.Abs(xInput) > 0)
        {
            body.velocity = new Vector2(xInput * groundSpeed, body.velocity.y);
        }

        if (Mathf.Abs(yInput) > 0 && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, yInput * jumpSpeed);
        }
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction()
    {
        if (grounded && xInput == 0 && yInput == 0)
        {
            body.velocity *= groundDecay;
        }
    }
    
}
