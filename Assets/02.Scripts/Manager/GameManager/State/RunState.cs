using System.Collections;
using UnityEngine;
public class RunState : IGameState
{
    public void Enter()
    {
        PlayerManager.Instance.EnableControl();
        CameraManager.Instance.EnableControl();
        GameManager.Instance.StartCoroutine(ShowStartText());
    }

    public void Exit()
    {
    }

    private IEnumerator ShowStartText()
    {
        GameUIManager.Instance.SetStateText("Run!");

        yield return new WaitForSeconds(2f);

        GameUIManager.Instance.SetStateText("");
    }
}
