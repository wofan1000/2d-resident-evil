using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Zombie : EnemyController
{
    GenaricHealth temp;
    public Rigidbody2D theRB;
    public Transform target;
    public Transform homePos;
    public float chaseRadius, attackRadius, grabRadius;
    public float mashDelay = .5f, mash;
    public float isPressed;
    public Animator anim;
    bool mashStarted;
    public float grabForce;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        
        mash = mashDelay;
        currentState = EnemyStates.idle;
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    public virtual void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius
         && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            // keep for monster attack
            // transform.position = Vector3.MoveTowards(transform.position, target.position, attackRadius);
            if (currentState == EnemyStates.idle || currentState == EnemyStates.walk && currentState != EnemyStates.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                theRB.MovePosition(temp);
                ChangeState(EnemyStates.walk);
            }
        }  else if (Vector3.Distance(target.position, transform.position) < chaseRadius
       && Vector3.Distance(target.position, transform.position) <= grabRadius)
        {
            if (currentState == EnemyStates.walk && currentState != EnemyStates.stagger)
            {
                StartCoroutine(GrabCo());
            }
         }
        }

    public void ChangeState(EnemyStates newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }
    private void setAnimFloat(Vector2 setvec)
    {
        anim.SetFloat("MoveX", setvec.x);
        anim.SetFloat("MoveY", setvec.y);
    }

    public void ChangeAnim(Vector2 dir)
    {
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if(dir.x > 0)
            {
                setAnimFloat(Vector2.right);
            } else if (dir.x < 0)
            {
                setAnimFloat(Vector2.left);

            }
        } else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
        {
            if (dir.y > 0)
            {
                setAnimFloat(Vector2.up);
            }
            else if (dir.y < 0)
            {
                setAnimFloat(Vector2.down);
            }
        }
    }

    public IEnumerator GrabCo()
    {
        PlayerController.instance.theSR.enabled = false;
        PlayerController.instance.moveSpeed = 0;
        PlayerController.instance.canShoot = false;
        anim.SetBool("Grab", true);
       
            if (Input.GetKeyUp(KeyCode.Space))
        {
            isPressed++;
            if(isPressed > 0)
            {
                yield return new WaitForSeconds(1f);
                anim.SetBool("Grab", false);
                theRB.AddForce(transform.position * grabForce * 30);
                PlayerController.instance.theSR.enabled = true;
                PlayerController.instance.moveSpeed = 7;
                PlayerController.instance.canShoot = true;
                 currentState = EnemyStates.walk;
                
            } else
            {
                
            }
        }     
    }
}
