using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EagleScript : MonoBehaviour
{
    public GameObject canvas;
    public bool canMove;
    [SerializeField] private float speed = 0.075f;
    [SerializeField] private float catchingSpeed = 0.3f;
    public static EagleScript instance = null;
    private Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        animator.SetBool("cought", false);
        canMove = true;
        StartCoroutine(FollowSquirrel());
    }
    IEnumerator FollowSquirrel()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (SquirrelMovement.instance.fire == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(5, 0, 0), speed);
            }
            if (SquirrelMovement.instance.transform.position.x - transform.position.x < 3)
            {
                canMove = false;
                animator.SetBool("cought", true);
                StartCoroutine(DropOnSquirrel());
                yield break;
            }
            if(this.transform.position.x > 244f)
            {
                Time.timeScale = 0;
            }
        }
    }
    IEnumerator DropOnSquirrel()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (SquirrelMovement.instance.fire == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, SquirrelMovement.instance.transform.position, catchingSpeed);
            }
            if (SquirrelMovement.instance.transform.position.x < this.transform.position.x)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
            } else
            {
                this.transform.localScale = new Vector3(-1, 1, 1);

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Squirrel")
        {
            Time.timeScale = 0;
            canvas.SetActive(true);
        }
    }

}
