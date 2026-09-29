using UnityEngine;

namespace skillveri_Assignment
{
    public class IdleState : IState
    {
        private EnemyAI enemy;

        public IdleState(EnemyAI enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            if (enemy == null) return;
            enemy.waitTimer = enemy.waitTimeAtPoint;
            enemy.SetAnimation("Idle");
            Debug.Log("Enemy Idle");
        }

        public void Update()
        {
            if (enemy == null) return;

            if (enemy.EnemyAutoSwitch() && enemy.GetDistanceToPlayer() <= enemy.detectionRange)
            {
                enemy.ChangeState(enemy.AttackState);
                return;
            }

            enemy.waitTimer -= Time.deltaTime;

            if (enemy.EnemyAutoSwitch() && enemy.waitTimer <= 0f)
            {
                enemy.currentPointIndex = (enemy.currentPointIndex + 1) % enemy.patrolPoints.Length;
                enemy.ChangeState(enemy.PatrolState);
            }
        }

        public void Exit() { }
    }
}