using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody2D theRB;
    
   public void SetUp(Vector2 velocity, Vector3 dir)
    {
        theRB.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(dir);
    }
}
