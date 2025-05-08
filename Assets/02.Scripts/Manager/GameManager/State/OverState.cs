using System.Collections;
using UnityEngine;

public class OverState : IGameState
{
    public void Enter()
    {
        Debug.Log("Enter Over");
        GameManager.Instance.StartCoroutine(OverRoutine());
    }

    public void Exit()
    {
        Debug.Log("Exit Over");
    }

    private IEnumerator OverRoutine()
    {
        GameUIManager.Instance.SetStateText("Game Over");

        PlayerManager.Instance.DisableControl();
        CameraManager.Instance.DisableControl();

        yield return new WaitForSeconds(2f);
    }
}
