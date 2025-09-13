using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;


public class CompanionAI : MonoBehaviour
{
    // Jumping
    public float jumpCooldown = 1f; // Duration of the cooldown in seconds
    private float jumpCooldownTimer = 0f; // Timer to track the cooldown
    public float frontCheckDistance = 0.5f; // Distance to check for jumping
    public LayerMask obstacleLayerMask; // Layer mask to specify which layers should be detected as obstacles


    ///////////////////////////////////////////
    public float rightDownCheckDistance = 2f; // Distance to check to the right and down
    public float rightDownCheckOffsetX = 0.5f; // Horizontal offset from the AI position for the right-down check
    public float rightDownCheckOffsetY = 0.8f; // Vertical offset from the AI position for the right-down check
    ///////////////////////////////////////////

    //Enemy follow and attack
    public bool attackEnemy = false;

    [Header("Pathfinding")]
    public Transform target;
    public float activeDistance = 1000f;
    public float activeDistanceEnemy = 5f;
    public float pathUpdateSeconds = 0.5f;
    public float nextWaypointDistance = 3f;

    public bool isArcher;

    [Header("Physics")]
    public float speed = 300f;
    public float jumpModifier = 0.0173f;
    public float newWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpCheckOffset = 0.1f;
    public float attackDamage = 20f;
    public float attackRange = 1.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float offsetX = 0.2f;
    [SerializeField] private float offsetY;


    [Header("Custom Behavior")]
    public bool followEnabeled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    float bossHealth = 100;
    private Path path;
    private int currentWayPoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    private GameObject enemy;
    [SerializeField] private GameObject healthPotion;
    Animator animator;
    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void UpdatePath()
    {
        GameObject closestEnemy = FindClosestEnemy();

        if (closestEnemy == null)
        {
            if (followEnabeled && TargetInDistancePlayer() && seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }
        else {

            if (closestEnemy.transform.GetComponent<ScriptForBossCrystal>() != null)
            {
                bossHealth = closestEnemy.transform.GetComponent<ScriptForBossCrystal>().currentHealth;
            }
            if (closestEnemy.transform.GetComponent<ScriptForBossDesert>() != null)
            {
                bossHealth = closestEnemy.transform.GetComponent<ScriptForBossDesert>().currentHealth;
            }
            if (closestEnemy.transform.GetComponent<ScriptForBossRocky>() != null)
            {
                bossHealth = closestEnemy.transform.GetComponent<ScriptForBossRocky>().currentHealth;
            }
            if (closestEnemy.transform.GetComponent<ScriptForBossSnow>() != null)
            {
                bossHealth = closestEnemy.transform.GetComponent<ScriptForBossSnow>().currentHealth;
            }

            if (TargetInDistanceEnemy(closestEnemy) && seeker.IsDone() && bossHealth > 0)
            {
                seeker.StartPath(rb.position, closestEnemy.transform.position, OnPathComplete);
            }
            else if (followEnabeled && TargetInDistancePlayer() && seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }
    }

    private void FixedUpdate()
    {
        if (FindClosestEnemy() != enemy)
        {
            StopAttackAnimation();
        }

        if (FindClosestEnemy() != null)
        {
            if (FindClosestEnemy().transform.GetComponent<ScriptForBossCrystal>() != null)
            {
                if (FindClosestEnemy().transform.GetComponent<ScriptForBossCrystal>().currentHealth <= 0)
                {
                    StopAttackAnimation();
                }
            }
            if (FindClosestEnemy().transform.GetComponent<ScriptForBossDesert>() != null)
            {
                if (FindClosestEnemy().transform.GetComponent<ScriptForBossDesert>().currentHealth <= 0)
                {
                    StopAttackAnimation();
                }
            }
            if (FindClosestEnemy().transform.GetComponent<ScriptForBossRocky>() != null)
            {
                if (FindClosestEnemy().transform.GetComponent<ScriptForBossRocky>().currentHealth <= 0)
                {
                    StopAttackAnimation();
                }
            }
            if (FindClosestEnemy().transform.GetComponent<ScriptForBossSnow>() != null)
            {
                if (FindClosestEnemy().transform.GetComponent<ScriptForBossSnow>().currentHealth <= 0)
                {
                    StopAttackAnimation();
                }
            }
        }

        // Decrement the jump cooldown timer
        if (jumpCooldownTimer > 0)
        {
            jumpCooldownTimer -= Time.deltaTime;
        }
        if (SceneManager.GetActiveScene().name == "CrystalScene" || SceneManager.GetActiveScene().name == "DesertScene" || SceneManager.GetActiveScene().name == "RockyScene" || SceneManager.GetActiveScene().name == "SnowScene")
        {
            enemy = FindClosestEnemy();
        }

        if (TargetInDistancePlayer() && followEnabeled)
        {
            PathFollow();
        }

        else if (FindClosestEnemy() != null && TargetInDistanceEnemy(enemy))
        {
            PathFollow();
        }
    }


    private void SetattackEnemy(bool x)
    {
        attackEnemy = x;
    }

    private GameObject FindClosestEnemy()
    {
        Dictionary<GameObject, float> enemyList = new Dictionary<GameObject, float>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            return null;
        }

        foreach (GameObject enemy in enemies)
        {
            enemyList.Add(enemy, Vector3.Distance(transform.position, enemy.transform.position));
        }

        var sortedDict = enemyList.OrderBy(entry => entry.Value);

        GameObject keyWithMinValue = sortedDict.First().Key;
        return keyWithMinValue;
    }

    private void AttackEnemy()
    {
        animator.SetBool("Attack", true);
        Vector3 attackPosition = transform.position + new Vector3(offsetX, offsetY, 0);
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayer);

        foreach (Collider2D enemy in enemiesToDamage)
        {

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

    private void StopAttackAnimation()
    {

        animator.SetBool("Attack", false);

    }
    private void PathFollow()
    {

        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        // Check if the companion is grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset), Color.red);

        // Calculate direction to the next waypoint
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Check for obstacles in front of the companion using the layer mask
        bool obstacleInFront = Physics2D.Raycast(transform.position, direction, frontCheckDistance, obstacleLayerMask);
        Debug.DrawRay(transform.position, direction * frontCheckDistance, Color.blue);

        // Attack enemy
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, frontCheckDistance, enemyLayer);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            AttackEnemy();

        }

        //Check for enemy closest for the comapinon using tag
        Dictionary<GameObject, float> enemyList = new Dictionary<GameObject, float>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemyList.Add(enemy, Vector3.Distance(transform.position, enemy.transform.position));
        }

        // Check for hole to the right and down from the companion
        Vector2 rightDownCheckPosition = transform.position + new Vector3(rightDownCheckOffsetX, -rightDownCheckOffsetY, 0f);
        bool holeToRightDown = !Physics2D.Raycast(rightDownCheckPosition, Vector2.down, rightDownCheckDistance);
        Debug.DrawRay(rightDownCheckPosition, Vector2.down * rightDownCheckDistance, Color.cyan);

        // Check for jump conditions, cooldown timer, front obstacle, front ground check, and hole to the right and down
        if (jumpEnabled && isGrounded && jumpCooldownTimer <= 0)
        {
            float heightDifference = path.vectorPath[currentWayPoint].y - transform.position.y;
            if (heightDifference > jumpNodeHeightRequirement || obstacleInFront || holeToRightDown)
            {
                Jump();

                // Reset the jump cooldown timer
                jumpCooldownTimer = jumpCooldown;
            }
        }

        // Apply force for movement
        rb.AddForce(force);

        // Check distance to the next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        // Handle direction look
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                if (!isArcher)
                {
                    transform.localScale = new Vector3(2.5795f, 3.031875f, 1f);
                    if (transform.GetChild(0).localScale.x < 0.0) {
                        Vector3 newScale = transform.GetChild(0).localScale;
                        newScale.x = -newScale.x;
                        transform.GetChild(0).localScale = newScale;
                    }
                }
                else
                {
                    transform.localScale = new Vector3(0.4289635f, 0.4566509f, 1f);
                    if (transform.GetChild(0).localScale.x < 0.0)
                    {
                        Vector3 newScale = transform.GetChild(0).localScale;
                        newScale.x = -newScale.x;
                        transform.GetChild(0).localScale = newScale;
                    }
                }
            }
            else if (rb.velocity.x < -0.05f)
            {
                if (!isArcher)
                {
                    transform.localScale = new Vector3(-2.5795f, 3.031875f, 1f);
                    if (transform.GetChild(0).localScale.x > 0.0)
                    {
                        Vector3 newScale = transform.GetChild(0).localScale;
                        newScale.x = -newScale.x;
                        transform.GetChild(0).localScale = newScale;
                    }
                }
                else
                {
                    transform.localScale = new Vector3(-0.4289635f, 0.4566509f, 1f);
                    if (transform.GetChild(0).localScale.x > 0.0)
                    {
                        Vector3 newScale = transform.GetChild(0).localScale;
                        newScale.x = -newScale.x;
                        transform.GetChild(0).localScale = newScale;
                    }
                }
            }
        }
    }

    public void Jump()
    {
        float jumpForce = speed * jumpModifier;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Follow() {
        followEnabeled = true;
        activeDistance = 200f;
    }

    public void Unfollow() {
        followEnabeled = false;
    }

    public void AttackNow() {
        activeDistanceEnemy = 100f;
    }

    public void StopAttacking() {
        activeDistanceEnemy = 5f;
    }

    public void DropPotion() {
        Instantiate(healthPotion, transform.position, transform.rotation);
        healthPotion.SetActive(true);
    }

    private bool TargetInDistancePlayer()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activeDistance;
    }

    private bool TargetInDistanceEnemy(GameObject enemy)
    {
        float distance = Vector2.Distance(transform.position, enemy.transform.position);
        return distance < activeDistanceEnemy;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
}
