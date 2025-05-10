using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyJumpAttackStrategy : EnemyAttackStrategyBase
{
    private Vector3 _startPos;
    private float _elapsed;
    private bool _jumping;
    private Vector3 _direction;
    private float _distance;
    private Quaternion _targetRotation;
    public EnemyJumpAttackStrategy()
    {
        _cooldown = 5f;
        _duration = 1.0f;
    }

    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        // ctx.Agent.isStopped = true;
        ctx.Collider.isTrigger = true;
        ctx.Animator.SetTrigger("JumpAttack");
        Vector3 position = ctx.Self.position;
        Vector3 targetPosition = ctx.Target.position;
        StartJump(position);

        position.y = 0.0f;
        targetPosition.y = 0.0f;

        _distance = Vector3.Distance(targetPosition, position);
        _direction = (targetPosition - position).normalized;

        _targetRotation = Quaternion.LookRotation(_direction);
    }

    public override void Update(IEnemyAttackContext ctx)
    {
        Transform transform = ctx.Self;
        Transform targetTransform = ctx.Target;

        Vector3 direction = (targetTransform.position - transform.position).normalized;
        direction.x = 0.0f;
        direction.y = 0.0f;

        UpdateJump(ctx.Self, ctx.Agent, ctx.Collider);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime);
    }

    public override void Exit(IEnemyAttackContext ctx) 
    {
        //ctx.Agent.isStopped = false;
        //ctx.Agent.Warp(ctx.Self.position);
    }

    public void StartJump(Vector3 _startPosition)
    {
        _startPos = _startPosition;
        _elapsed = 0f;
        _jumping = true;
    }

    public void UpdateJump(Transform transform, NavMeshAgent agent, Collider collider)
    {
        if (!_jumping) return;

        _elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsed / _duration);

        // 등속 궤적 (올라가다 내려옴)
        float yOffset = t <= 0.5f
            ? Mathf.Lerp(0, 5.0f, t * 2f)                   // 상승 (0~0.5 구간)
            : Mathf.Lerp(5.0f, 0, (t - 0.5f) * 2f);          // 하강 (0.5~1 구간)

        Vector3 horizontalOffset = _distance * _direction * t;

        Vector3 nextPos = _startPos + horizontalOffset + new Vector3(0, yOffset, 0);

        RaycastHit hit;
        int groundOnlyMask = LayerMask.GetMask("Ground", "Player");
        if (t < 1f)
            transform.position = nextPos;
        else if(Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 2.0f, groundOnlyMask))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            _jumping = false;
            collider.isTrigger = false;
        }
    }

}
