using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{

    Rigidbody2D rd;

    [HideInInspector] public Animator anim;

    [HideInInspector] public PhysicsCheck physicsCheck;

    public float normalSpeed;
    public float chaseSpeed;

    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;
    public float hurtForce;

    public float waitTime;
    public float waitTimeCounter;
    public bool wait;

    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    public float lostTime;
    public float lostTimeCounter;

    public bool isHurt;
    public bool isDead;
    public Transform attacker;
    public Vector3 spwanPoint;

    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState skillState;
    


    protected virtual void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        spwanPoint = transform.position;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }


    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        currentState.LogicUpdate();

        TimeCounter();

    }

    private void FixedUpdate()
    {
        if(!isHurt&&!isDead&&!wait)
        Move();

        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    public virtual void Move()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("PreMove")&&!anim.GetCurrentAnimatorStateInfo(0).IsName("SnailRecover"))
        rd.velocity = new Vector2(currentSpeed*faceDir.x*Time.deltaTime,rd.velocity.y);
    }

    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        if (!FindPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter -=Time.deltaTime;
        }
        //else
        //{
        //    lostTimeCounter = lostTime;
        //}
    }

    public virtual bool FindPlayer()
    {
        return  Physics2D.BoxCast(transform.position + (Vector3)centerOffset,checkSize,0,faceDir,checkDistance,attackLayer);
        
    }

    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }


    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        //转身
        if (attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //受伤被击退

        isHurt = true;

        anim.SetTrigger("hurt");

        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x,0).normalized;
        rd.velocity = new Vector2(0, rd.velocity.y);

        StartCoroutine(OnHurt(dir));
    }


    private IEnumerator OnHurt(Vector2 dirr)
    {
        rd.AddForce(dirr * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }


    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    public void SwitchState(NPCstate state) 
    {
        var newState = state switch
        {
            NPCstate.Patrol => patrolState,
            NPCstate.Chase => chaseState,
            NPCstate.Skill => skillState,
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }


    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset, 0.2f);
    }

}
