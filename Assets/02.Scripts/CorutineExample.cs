using UnityEngine;

public class CorutineExample : MonoBehaviour
{
    // ㄴ 게임 오브젝트 + '유니티 이벤트 함수(라이프 사이클)'
    //                             ㄴ Awake, Start, Update, FixedUpdate
    // Coroutine: 유니티에서 특정 함수 내용을 여러 프레임에 걸쳐 실행하는 것으로
    //          : 비동기 코드를 구현하기 위한 방법 중 하나입니다.

    private void Start()
    {
        // 동기 코드: 어떤 작업을 실행할 때 그 작업이 모두 끝나기를 기다리는 방식
        //          -> 장점: 작업이 끝날 때까지 기다리므로, 작업의 순서를 보장한다.
        Sum(2, 3); // 10초 후 아래 코드가 실행되고
        Sub(2, 3); // 30초 후 아래 코드가 실행이 됩니다.
        Debug.Log("Hello");

        // 비동기(Asynchronus): 어떤 작업을 실행할 때 그 작업이 완료되지 않더라도 다음 코드를 실행하는 방식
        //                      즉, 작업이 완료되지 않았더라도 결과를 기다리지 않고 다음 코드를 실행하는 것이다.
        // -> 시간 절약, 병렬 처리 가능
        // -> 유니티는 비동기를 Coroutine이라는 이름으로 구현 해놨다.
        // -> 자주 쓰는 경우: '시간 제어', '네트 워크', '데이터 로딩' 등에 쓰인다.

    }
    public int Sum(int number1, int number2)
    {
        // 반복문 등으로 인해서 10초가 걸리는 함수

        return number1 + number2;
    }
    public int Sub(int number1, int number2)
    {
        // 반복문 등으로 인해서 30초가 걸리는 함수

        return number1 - number2;
    }
}
