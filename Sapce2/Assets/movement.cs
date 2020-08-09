using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    // Config
    public float walkingForce = 0.25f;
    public float speed = 5;
    public float jumpForce = 8;
    public float boost_speed = 20;

    //Variables
    Rigidbody2D rb;
    public SpriteRenderer sr;
    public Sprite yellow_rowling;
    public Sprite yellow_normal;
    public Sprite yellow_burst;

    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    private int rolling_status = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Rotate();
        CheckIfGrounded();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveByX = x * speed;
        float moveByX_boost = x * boost_speed;
        rb.velocity = new Vector2(moveByX, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rolling_status = 50;
            sr.sprite = yellow_rowling;
        }

        if (rolling_status > 0)
        {
            rb.velocity = new Vector2(moveByX_boost, rb.velocity.y);
            rolling_status--;

            if(moveByX_boost > 0)
            {
                rb.transform.Rotate(0, 0, -20, Space.Self);
            }

            if (moveByX_boost < 0)
            {
                rb.transform.Rotate(0, 0, 20, Space.Self);
            }

        }

        if (sr.sprite == yellow_rowling && rolling_status == 0)
        {
            sr.sprite = yellow_normal;
        }

    }

    void Rotate()
    {

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    rb.transform.Rotate(0, 0, 1, Space.Self);
        //}

        //if (Input.GetKey(KeyCode.E))
        //{
        //    rb.transform.Rotate(0, 0, -1, Space.Self);
        //}

        if(rolling_status == 0)
            rb.transform.rotation = new Quaternion(0, 0, 0, 0);

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
        } else if (isGrounded && (rb.velocity.x != 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + walkingForce);
        } else if (Input.GetKey(KeyCode.Space) && !isGrounded && rb.velocity.y <= jumpForce / 5)
        {
           rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce / 20);
           sr.sprite = yellow_burst;

        } else if (!Input.GetKey(KeyCode.Space) && sr.sprite == yellow_burst)
        {
            sr.sprite = yellow_normal;
        }


    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
