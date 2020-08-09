using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    // Next Planet
    public Rigidbody2D nextPlanet;
    public Rigidbody2D sfo;
    Movement mv;

    public Vector2 gravityDirection;
    public Vector2 gravityForce;



    // Start is called before the first frame update
    void Start()
    {
      mv = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {

        Apply();

    }


    void Apply()
    {
        //Calculate Gravity Vectors
        gravityDirection = nextPlanet.position - sfo.position;
        gravityDirection = Vector3.Normalize(gravityDirection);

        gravityForce = gravityDirection * 0.2f;
        //Debug.Log(gravityForce);

        //Apply Gravity Force
        sfo.velocity = sfo.velocity + gravityForce;

        //Rotate to core
        //sfo.transform.eulerAngles = gravityDirection;
        Debug.Log(gravityDirection);

        float toRotate = (gravityDirection.y + 1) * 180 / 2;


        if (gravityDirection.x >= 0)
        {
             toRotate = (gravityDirection.y + 1) * 180 / 2;
        }
        else
        {
             toRotate = -1 * ((gravityDirection.y + 1) * 180 / 2);
        }


        sfo.SetRotation(toRotate);

    }
}
