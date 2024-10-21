using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Translate(1 * speed, 0, 0);
        if(transform.position.x > 30)
        {
            transform.position = new Vector3(-20, Random.Range(10, 20), 0);
        }
    }
}
