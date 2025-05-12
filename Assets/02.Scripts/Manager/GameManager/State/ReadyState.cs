using System.Collections;
using UnityEngine;
public class ReadyState : IGameState
{
    public void Enter()
    {
        GameManager.Instance.StartCoroutine(ReadyRoutine());
    }

    public void Exit()
    {
    }

    private IEnumerator ReadyRoutine()
    {
        GameUIManager.Instance.SetStateText("Ready...");

        PlayerManager.Instance.DisableControl();
        CameraManager.Instance.DisableControl();

        yield return new WaitForSeconds(2f);

        GameManager.Instance.ChangeState(new RunState());
    }
}