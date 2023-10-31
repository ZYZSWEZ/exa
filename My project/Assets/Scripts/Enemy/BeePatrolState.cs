using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
       
    }

    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        
    }
}
