using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;

    public bool manual;

    public Vector2 bottomOffset;

    public Vector2 leftOffset;
    public Vector2 rightOffset;

    public bool isGround;

    public bool touchLeftWall;
    public bool touchRightWall;

    public float checkRaduis;

    public LayerMask groundLayer;

    private void Awake()
    {
        coll= GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x+coll.offset.x)/2,coll.bounds.size.y/2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }

    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //ºÏ≤‚µÿ√Ê
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);

        //«ΩÃÂ≈–∂œ
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis);
    }
    

}
