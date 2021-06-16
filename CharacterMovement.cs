using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;
    private Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public int maxHealth = 100;
    public int currentHealth;
    //public HealthBar healthBar;

    private void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    void Start() {
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
    }

    private void Update() {

        // Handle Jump
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) || IsGrounded() && Input.GetKey(KeyCode.W)) {
            //animator.SetTrigger("takeOf");
            float jumpVelocity = 7f;
            rigidbody2d.velocity = Vector2.up * jumpVelocity;
        }

        if (IsGrounded() == true) {
            //animator.SetBool("isJumping", false);
        } else {
            //animator.SetBool("isJumping", true);
        }

        if(Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.Mouse0)) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            TakeDamage(20);
        }
    }

    private void FixedUpdate() {
        float moveSpeed = 8f;
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (Input.GetKey(KeyCode.A)) {
            rigidbody2d.velocity = new Vector2(-moveSpeed, rigidbody2d.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
            //animator.SetBool("isRunning", true);
        } else {
            if (Input.GetKey(KeyCode.D)) {
                rigidbody2d.velocity = new Vector2(+moveSpeed, rigidbody2d.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
                //animator.SetBool("isRunning", true);
            } else {
                // No keys pressed
                rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
                rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                //animator.SetBool("isRunning", false);
            }
        }
    }

    void TakeDamage(int damage) {
        currentHealth -= damage;

        //animator.SetTrigger("Hurt");

        //healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        animator.SetBool("isDead", true);

        //GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void Attack() {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        foreach(Collider2D enemy in hitEnemies) {
            //enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private bool IsGrounded() {
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);

        Color rayColor;
        if (raycastHit.collider != null) {
            rayColor = Color.green;
        } else {
            rayColor = Color.red;
        }
        //Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);
        //Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }
}
