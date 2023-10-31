using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [Header("ÒÆ¶¯·¶Î§")]
    public float patroRadius;

    public override bool FindPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
        {
            attacker = obj.transform;
        }
        return obj;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patroRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patroRadius, patroRadius);
        var targetY = Random.Range(-patroRadius, patroRadius);

        return spwanPoint+new Vector3 (targetX, targetY);
    }

}
