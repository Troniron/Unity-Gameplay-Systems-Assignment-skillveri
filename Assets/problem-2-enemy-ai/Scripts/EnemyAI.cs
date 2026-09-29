using UnityEngine;
using UnityEngine.UI;

namespace skillveri_Assignment
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("References")]
        public Transform player;
        public Animator animator;

        [Header("Patrol")]
        public Transform[] patrolPoints;
        public float moveSpeed = 2f;
        public float waitTimeAtPoint = 2f;
        public float reachDistance = 0.3f;

        [Header("Detection")]
        public float detectionRange = 6f;
        public float attackRange = 1.8f;
        public float attackCooldown = 1.5f;

        public IState CurrentState { get; private set; }

        public PatrolState PatrolState { get; private set; }
        public IdleState IdleState { get; private set; }
        public AttackState AttackState { get; private set; }

        [HideInInspector] public int currentPointIndex = 0;
        [HideInInspector] public float waitTimer = 0f;
        [HideInInspector] public float attackTimer = 0f;
        public bool IsEnemyAutoDetect = false;
        public Toggle EnemyAuto;
        private void Start()
        {
            PatrolState = new PatrolState(this);
            IdleState = new IdleState(this);
            AttackState = new AttackState(this);

            ChangeState(PatrolState);
        }
        public bool EnemyAutoSwitch()
        {
            return EnemyAuto != null && EnemyAuto.isOn;
        }
        private void Update()
        {
            if (this == null || player == null) return;

            CurrentState?.Update();
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangeState(IdleState);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangeState(PatrolState);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                ChangeState(AttackState);
        }

        public void ChangeState(IState newState)
        {
            if (this == null) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }

        public void SetAnimation(string animName)
        {
            if (this == null || animator == null) return;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            {
                animator.Play(animName);
            }
        }

        public float GetDistanceToPlayer()
        {
            if (this == null || player == null) return Mathf.Infinity;
            return Vector3.Distance(transform.position, player.position);
        }

        private void OnDestroy()
        {
            CurrentState = null;
            PatrolState = null;
            IdleState = null;
            AttackState = null;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            if (patrolPoints == null || patrolPoints.Length < 2) return;

            Gizmos.color = Color.cyan;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] == null) continue;

                Vector3 current = patrolPoints[i].position;
                Vector3 next = patrolPoints[(i + 1) % patrolPoints.Length].position;
                Gizmos.DrawLine(current, next);
                Gizmos.DrawSphere(current, 0.2f);
            }
        }
#endif
    }
}