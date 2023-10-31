using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerInputControl inputControl;

    private Rigidbody2D rd;

    private CapsuleCollider2D capsuleCollider;

    public Vector2 inputDirection;

    private PhysicsCheck physicsCheck;

    private PlayerAnimation playerAnimation;



    public float speed;
    private float runSpeed;
    private float walkSpeed => speed/2.5f;
    public float jumpForce;
    

    public bool isCrouch;

    private Vector2 oriOffset;
    private Vector2 oriSize;

    public float hurtForce;
    public bool isHurt;

    public bool isDead;
    public bool isAttack;

    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;


    private void Awake()
    {
        rd = GetComponent<Rigidbody2D>();       
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        oriOffset = capsuleCollider.offset;
        oriSize = capsuleCollider.size;

        inputControl.Gameplay.Jump.started += Jump;


        //shift走路
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
                speed = runSpeed;
        };

        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;

    }

    

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt)
        Move();
    }

    


    public void Move()
    {
        if (!isCrouch&&!isAttack)
        {
            rd.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rd.velocity.y);
        }

        

        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;

        //人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);

        //人物下蹲
        isCrouch = inputDirection.y<-0.5f&&physicsCheck.isGround;

        if (isCrouch)
        {
            capsuleCollider.offset = new Vector2(-0.05f, 0.85f);
            capsuleCollider.size = new Vector2(0.7f,1.7f);
        }
        else 
        {
            capsuleCollider.size = oriSize;
            capsuleCollider.offset = oriOffset;
        }

    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(physicsCheck.isGround)
        rd.AddForce(transform.up * jumpForce,ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.PlayerAttack();
        isAttack = true;
       
    }


    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rd.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x-attacker.position.x),0).normalized;

        rd.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }


    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    private void CheckState()
    {
        capsuleCollider.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

}
