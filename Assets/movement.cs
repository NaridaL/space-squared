using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Config
    public float walkingForce = 0.25f;
    public float speed = 5;
    public float jumpForce = ;
    public float jetpackForce = 1;
    public float boost_speed = 20;

    //Variables
    Rigidbody2D rb;
    public Gravity g;
    public SpriteRenderer sr;
    public Sprite yellow_rowling;
    public Sprite yellow_normal;
    public Sprite yellow_burst;

    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    private int rolling_status = 0;
    private float current_movement;

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

        //Normal walking
        if (isGrounded)
        {
            Vector2 walk_right = Vector2.Perpendicular(g.gravityDirection);
            Vector2 walk_left = Vector2.Perpendicular(g.gravityDirection) * -1;
            rb.velocity = walk_right * x * speed;

        }



        float moveByX = x * speed;
        float moveByX_boost = x * boost_speed;
        float velocity_x = moveByX;



        // Begin Rolling cycle
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rolling_status = 50;
            sr.sprite = yellow_rowling;
        }

        // Apply Boost and Rotation if rolling
        if (rolling_status > 0)
        {
            velocity_x = moveByX_boost;
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

        //Update Sprite at the end of rolling
        if (sr.sprite == yellow_rowling && rolling_status == 0)
        {
            sr.sprite = yellow_normal;
        }


    }

    void Rotate()
    {

        //if(rolling_status == 0)
        //    rb.transform.rotation = new Quaternion(0, 0, 0, 0);

    }

    void Jump()
    {
        //If grounded jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = rb.velocity + g.gravityDirection * -jumpForce;

        //If not grounded start jetpack
        } else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            rb.velocity = rb.velocity + g.gravityDirection * -jetpackForce;
            sr.sprite = yellow_burst;

        //Stop jetpack
        } else if (!Input.GetKey(KeyCode.Space) && sr.sprite == yellow_burst)
        {
            sr.sprite = yellow_normal;
        }


    }

    public bool CheckIfGrounded()
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
        return isGrounded;
    }
}
