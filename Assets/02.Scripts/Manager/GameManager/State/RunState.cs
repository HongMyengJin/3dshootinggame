using System.Collections;
using UnityEngine;
public class RunState : IGameState
{
    public void Enter()
    {
        Debug.Log("Enter Run");
        PlayerManager.Instance.EnableControl();
        CameraManager.Instance.EnableControl();
        GameManager.Instance.StartCoroutine(ShowStartText());
    }

    public void Exit()
    {
        Debug.Log("Exit Run");
    }

    private IEnumerator ShowStartText()
    {
        GameUIManager.Instance.SetStateText("Run!");

        yield return new WaitForSeconds(2f);

        GameUIManager.Instance.SetStateText("");
    }
}
