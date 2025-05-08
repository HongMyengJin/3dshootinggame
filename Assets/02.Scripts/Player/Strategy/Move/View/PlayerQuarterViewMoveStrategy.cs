using UnityEngine;

public class PlayerQuarterViewMoveStrategy : IPlayerMoveStrategy
{
    private bool isMoving;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveProgress;

    public void Move(IPlayerMoveContext context)
    {
        Vector3 move = Vector3.zero;

        if (TryGetClickedPoint(out Vector3 clickedPoint))
            SetMoveTarget(context.Transform.position, clickedPoint);

        if (isMoving)
            move = MoveToTarget(context);

        if (context is PlayerController player)
            move.y = player.GetVerticalVelocity();

        context.CharacterController.Move(move * Time.deltaTime);
        context.Animator.SetFloat("MoveSpeed", moveProgress);
    }

    private bool TryGetClickedPoint(out Vector3 point)
    {
        point = Vector3.zero;
        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                point = hit.point;
                return true;
            }
        }
        return false;
    }

    private void SetMoveTarget(Vector3 start, Vector3 target)
    {
        startPosition = start;
        targetPosition = target;
        moveProgress = 1f;
        isMoving = true;
    }

    private Vector3 MoveToTarget(IPlayerMoveContext context)
    {
        Vector3 direction = targetPosition - context.Transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
        {
            moveProgress -= Time.deltaTime;
            if (moveProgress <= 0f)
                isMoving = false;
            return Vector3.zero;
        }

        float totalDistance = Vector3.Distance(startPosition, targetPosition);
        float remainingDistance = Vector3.Distance(context.Transform.position, targetPosition);

        direction.Normalize();
        moveProgress = remainingDistance / totalDistance;

        Vector3 move = direction * context.Speed;
        return move;
    }
}
