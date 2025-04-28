using System.Collections;
using UnityEngine;
public class ReadyState : IGameState
{
    public void Enter()
    {
        Debug.Log("Enter Ready");
        GameManager.Instance.StartCoroutine(ReadyRoutine());
    }

    public void Exit()
    {
        Debug.Log("Exit Ready");
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