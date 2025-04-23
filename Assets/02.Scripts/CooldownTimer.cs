using System.Collections;
using System.Threading;
using UnityEngine;

public class CooldownTimer : MonoBehaviour
{
    public float Cooltime = 3.0f;
    private float _timer;

    private void Start()
    {
        StartCoroutine(WaitTime(3f)); // 3�� �� �Ʒ� �ڵ尡 ����ȴ�.

        Debug.Log("������ ���ξ�.");
    }

    private void Update()
    {
        //_timer += Time.deltaTime;
        //if (_timer >= Cooltime)
        //{
        //    Debug.Log($"{Cooltime}�� �������ϴ�.");
        //}
    }

    // �ڷ�ƾ: ����Ƽ�� �񵿱� �Լ� ���� ������� ����� �ٸ��� ���� �ð��� ���ļ� ���� ������ ������ �� �ִ�.
    // Coroutine: IEnumerator, IEnumerable, yield
    private IEnumerator WaitTime(float waitTime)
    {
        // yield: �ڷ�ƾ�� �ٽ����� �κ�����, ������ �����ϰ� ����Ƽ�� ���� Ư�� ����(������, ��, ��Ʈ��ũ)�� ������ ������ ��ٸ��� �ϴ� ����
        yield return new WaitForSeconds(waitTime);

        Debug.Log($"{waitTime}�� �������ϴ�.");
    }
}
