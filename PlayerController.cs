using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Walk,
    Shoot,
    Attack,
    Intaract,
    idle,
    stagger
}
public class PlayerController : MonoBehaviour
{
    public Image staminaProgress = null;
    public CanvasGroup slider = null;
    public static PlayerController instance;
    public PlayerState state;
    public float sprintSpeed, sprintCost;
    public float moveSpeed;
    public Rigidbody2D theRB;
    private Vector3 change;
    public bool canShoot;
    private Animator anim;
    

    public SpriteRenderer theSR;
    public Sprite[] playerDir;

    /*
    public FloatValue currentHealth;
    public Signals healthSig;
    */
    public GameObject projectile;
    public int currentAmmo;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
            MoveCharecter();
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
            UpdateAnimAndMove();
            if (Input.GetButtonDown("attack") && state != PlayerState.Attack && state != PlayerState.Shoot && state != PlayerState.stagger)
        {
                StartCoroutine(AttackCo());
            } else if (Input.GetButtonDown("shoot") && state != PlayerState.Attack && state != PlayerState.Shoot && state != PlayerState.stagger)
        {
            StartCoroutine(ShootCo());
        }
            else if (state == PlayerState.Walk || state == PlayerState.idle)
            {
                UpdateAnimAndMove();
            } 
    }

 

 

   
 
    void UpdateAnimAndMove()
    {
        if (change != Vector3.zero)
        {
            anim.SetFloat("MoveX", change.x);
            anim.SetFloat("MoveY", change.y);
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }
    void MoveCharecter()
    {
        theRB.MovePosition(transform.position + change * moveSpeed * Time.fixedDeltaTime);
    }
   
    private IEnumerator AttackCo()
    {
        anim.SetBool("Attack", true);
        state = PlayerState.Attack;
        yield return null;
        anim.SetBool("Attack", false);
        yield return  new WaitForSeconds(.33f);
        state = PlayerState.Walk;
    }

    private IEnumerator ShootCo()
    {
        if (canShoot)
        {
            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
            }
            if (currentAmmo > 0)
            {
                anim.SetBool("shoot", true);
                state = PlayerState.Attack;
                yield return null;
                MakeBullet();
                anim.SetBool("shoot", false);
                yield return new WaitForSeconds(.33f);
                state = PlayerState.Walk;
            }
            currentAmmo--;
        } else
        {
            canShoot = false;
        }
    }


 

    private void MakeBullet()
    {
        Vector2 temp = new Vector2(anim.GetFloat("MoveX"), anim.GetFloat("MoveY"));
        Bullet bullet = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.SetUp(temp, BulletDir());
    }

    public Vector3 BulletDir()
    {
        float temp = Mathf.Atan2(anim.GetFloat("MoveY"), anim.GetFloat("MoveX") * Mathf.Rad2Deg);
        return new Vector3(0, 0, temp);
    }

    public void Knock( float knocktime)
    {
        StartCoroutine(Knockco(knocktime));
    }

    private IEnumerator Knockco(float knockTime)
    {
        if (theRB != null)
        {
            yield return new WaitForSeconds(knockTime);
            theRB.velocity = Vector2.zero;
            state = PlayerState.idle;
            theRB.velocity = Vector2.zero;
        }
    }

}
