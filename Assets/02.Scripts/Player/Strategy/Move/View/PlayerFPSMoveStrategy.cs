using UnityEngine;

public class PlayerFPSMoveStrategy : IPlayerMoveStrategy
{
    public void Move(IPlayerMoveContext context)
    {
        Vector3 inputDir = new Vector3(context.Input.x, 0, context.Input.y);
        Transform cameraTransform = CameraManager.Instance.GetCurrentCamera().transform;
        Vector3 worldDir = cameraTransform.TransformDirection(inputDir);
        worldDir.y = 0;

        Vector3 move = worldDir.normalized * context.Speed;
        if (context is PlayerController player)
            move.y = player.GetVerticalVelocity();
        context.CharacterController.Move(move * Time.deltaTime);

        float speed = inputDir.magnitude;
        context.Animator.SetFloat("MoveSpeed", speed);
    }
}