using System.Collections;
using System.Threading;
using UnityEngine;

public class CooldownTimer : MonoBehaviour
{
    public float Cooltime = 3.0f;
    private float _timer;

    private void Start()
    {
        StartCoroutine(WaitTime(3f)); // 3초 후 아래 코드가 실행된다.

        Debug.Log("고마워요 수민씨.");
    }

    private void Update()
    {
        //_timer += Time.deltaTime;
        //if (_timer >= Cooltime)
        //{
        //    Debug.Log($"{Cooltime}이 지났습니다.");
        //}
    }

    // 코루틴: 유니티의 비동기 함수 실행 방식으로 동기와 다르게 여러 시간에 걸쳐서 안의 내용을 수행할 수 있다.
    // Coroutine: IEnumerator, IEnumerable, yield
    private IEnumerator WaitTime(float waitTime)
    {
        // yield: 코루틴의 핵심적인 부분으로, 실행을 중지하고 유니티의 다음 특정 조건(프레임, 초, 네트워크)이 충족될 때까지 기다리게 하는 역할
        yield return new WaitForSeconds(waitTime);

        Debug.Log($"{waitTime}이 지났습니다.");
    }
}
