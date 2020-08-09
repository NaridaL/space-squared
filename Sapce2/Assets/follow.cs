using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class follow : MonoBehaviour
{

    // The target we are following
    [SerializeField]
    public Transform target;
    public int freedomFactor;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        if ((Math.Abs(target.transform.position.x - transform.position.x) + Math.Abs(target.transform.position.y - transform.position.y)) > freedomFactor)
        {
            rb.velocity = target.GetComponent<Rigidbody2D>().velocity;
        }
        else
        {
            rb.velocity = new Vector2((target.transform.position.x - transform.position.x), (target.transform.position.y - transform.position.y));
        }
        
    }
}
