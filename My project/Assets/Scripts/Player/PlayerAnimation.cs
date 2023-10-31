using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    private Rigidbody2D rd;

    private PhysicsCheck physicsCheck;

    private PlayerController playerController;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rd = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        animator.SetFloat("velocityX", Mathf.Abs(rd.velocity.x));
        animator.SetFloat("velocityY", rd.velocity.y);
        animator.SetBool("isGround",physicsCheck.isGround);
        animator.SetBool("isCrouch",playerController.isCrouch);
        animator.SetBool("isDead", playerController.isDead);
        animator.SetBool("isAttack",playerController.isAttack);
        
    }
    
    public void PlayHurt()
    {
        animator.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        animator.SetTrigger("attack");
    }

}
