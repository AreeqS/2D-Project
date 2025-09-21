using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour

{
    public float moveSpeed = 5f;
    public float jumpforce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;


    public Image healthImage;


    public int health = 5;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;

    private SpriteRenderer spriteRenderer;  
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

     
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
        }

        SetAnimation(moveInput);
        Flip(moveInput);

        healthImage.fillAmount = health / 5f;
        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0 )
            {
                animator.Play("Player_idle");
            }
            else
            {
                animator.Play("Player_run");
            }

        }


       else
        {
            if (rb.linearVelocity.y > 0)
            {
                animator.Play("Player_jump");
            }
            else
            {
                animator.Play("Player_fall");
            }
        } 
            
     }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            health -= 1;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
            StartCoroutine(BlinkRed());
        }

        if (collision.gameObject.tag == "HighDamage")
        {
            health = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
            StartCoroutine(BlinkRed());
        }

        if (health <= 0)
        {
            Die();

          
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;


    }

    private void Die()
    {
       
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }


    private void Flip(float moveInput)
    {
        if (moveInput > 0 )
            spriteRenderer.flipX = false;

        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
  
           
        
 





