using UnityEngine;

public class PlayerTPSMoveStrategy : IPlayerMoveStrategy
{
    public void Move(IPlayerMoveContext context)
    {
        Vector3 inputDir = new Vector3(context.Input.x, 0, context.Input.y);
        Transform cameraTransform = CameraManager.Instance.GetCurrentCamera().transform;

        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        Vector3 right = cameraTransform.right;
        right.y = 0;

        Vector3 moveDir = forward * inputDir.z + right * inputDir.x;
        Vector3 move = moveDir.normalized * context.Speed;
        if (context is PlayerController player)
            move.y = player.GetVerticalVelocity();
        context.CharacterController.Move(move * Time.deltaTime);

        float speed = inputDir.magnitude;
        context.Animator.SetFloat("MoveSpeed", speed);
    }

}
