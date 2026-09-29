using UnityEngine;

namespace skillveri_Assignment
{
    public class AttackState : IState
    {
        private EnemyAI enemy;

        public AttackState(EnemyAI enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            if (enemy == null) return;
            enemy.attackTimer = 0f;
            Debug.Log("Enemy Attack");
        }

        public void Update()
        {
            if (enemy == null) return;
            if (enemy.EnemyAutoSwitch())
            {
                float distance = enemy.GetDistanceToPlayer();

                if (distance > enemy.detectionRange)
                {
                    enemy.ChangeState(enemy.PatrolState);
                    return;
                }

                Vector3 direction = (enemy.player.position - enemy.transform.position).normalized;
                direction.y = 0;

                if (direction != Vector3.zero)
                {
                    Quaternion lookRot = Quaternion.LookRotation(direction);
                    enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRot, 8f * Time.deltaTime);
                }

                if (distance <= enemy.attackRange)
                {
                    if (enemy.attackTimer <= 0f)
                    {
                        enemy.SetAnimation("Attack");
                        enemy.attackTimer = enemy.attackCooldown;
                    }
                    else
                    {
                        enemy.attackTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    enemy.transform.position += direction * enemy.moveSpeed * Time.deltaTime;
                    enemy.SetAnimation("Walk");
                    enemy.attackTimer -= Time.deltaTime;
                }
            }
            else
            {
                enemy.transform.position = Vector3.MoveTowards(
         enemy.transform.position,
         enemy.player.position,
         enemy.moveSpeed * Time.deltaTime
     );
            }

        }

        public void Exit() { }
    }
}