using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SquirrelMovement : MonoBehaviour
{
    public GameObject canvas;
    public bool fire = false;
    private bool onGround = false;
    [SerializeField] GameObject finalCanvas;
    private Vector2 movement = new Vector2(0f, 0f);
    [SerializeField] private float movementVelocity = 450f;
    [SerializeField] private float jumpForce = 6f;
    private Coroutine groundCheckCoroutine;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    public static SquirrelMovement instance = null;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        fire = false;
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        groundCheckCoroutine = StartCoroutine(GroundCheck());
        Time.timeScale = 1;
    }
    void Update()
    {

        if (this.transform.position.x > -30f && this.transform.position.x < 233f)
        {
            Camera.main.transform.position = new Vector3(this.transform.position.x, 0.5f, -10);
        }
        if (EagleScript.instance.canMove == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.Normalize();
            if (movement.x != 0)
            {
                this.transform.localScale = new Vector3(movement.x, 1, 1);
                rb.velocity = new Vector2(movementVelocity * movement.x * Time.deltaTime, rb.velocity.y);
                animator.SetBool("standing", false);
                animator.SetBool("running", true);

            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("standing", true);
                animator.SetBool("running", false);
            }
            if (onGround)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool("jumping", true);
                    animator.SetBool("running", false);
                    animator.SetBool("standing", false);
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    onGround = false;
                    groundCheckCoroutine = StartCoroutine(GroundCheck());
                }
            }
        }

        if (Input.GetKey("x"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (this.transform.position.x > 244f)
        {
            EagleScript.instance.canMove = false;
            finalCanvas.SetActive(true);
        }
    }
    IEnumerator GroundCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, new Vector2(0.8f, 0.8f), 0, Vector2.down, 1.1f, groundLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    animator.SetBool("jumping", false);
                    animator.SetBool("running", false);
                    animator.SetBool("standing", true);
                    StopCoroutine(groundCheckCoroutine);
                    onGround = true;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fire")
        {
            fire = true;
            Time.timeScale = 0;
            EagleScript.instance.canMove = false;
            canvas.SetActive(true);
        }
    }
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}