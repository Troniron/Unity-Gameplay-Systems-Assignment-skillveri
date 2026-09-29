using UnityEngine;

namespace skillveri_Assignment
{
    public class PatrolState : IState
    {
        private EnemyAI enemy;

        public PatrolState(EnemyAI enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            if (enemy == null) return;
            enemy.SetAnimation("Walk");
            Debug.Log("Enemy Patrol");
        }

        public void Update()
        {
            if (enemy == null) return;

            if (enemy.EnemyAutoSwitch() && enemy.GetDistanceToPlayer() <= enemy.detectionRange)
            {
                enemy.ChangeState(enemy.AttackState);
                return;
            }

            if (enemy.patrolPoints == null || enemy.patrolPoints.Length == 0) return;

            Transform targetPoint = enemy.patrolPoints[enemy.currentPointIndex];
            if (targetPoint == null) return;
            float distance = Vector3.Distance(enemy.transform.position, targetPoint.position);
            if (enemy.EnemyAutoSwitch() && distance <= enemy.reachDistance)
            {
                enemy.ChangeState(enemy.IdleState);
                return;
            }
            else if (distance <= enemy.reachDistance)
            {
                enemy.currentPointIndex = (enemy.currentPointIndex + 1) % enemy.patrolPoints.Length;
            }

            Vector3 direction = (targetPoint.position - enemy.transform.position).normalized;
            direction.y = 0;

            enemy.transform.position += direction * enemy.moveSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRot, 5f * Time.deltaTime);
            }
        }

        public void Exit() { }
    }
}