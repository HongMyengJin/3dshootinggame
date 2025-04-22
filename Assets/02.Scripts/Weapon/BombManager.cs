using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;

    public GameObject BombPrefab;
    private List<Bomb> _bombList = new List<Bomb>();
    public int MaxBombN = 3;

    private int _bombIdx = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SettingBombs();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SettingBombs()
    {
        for(int i = 0; i < MaxBombN; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);
            _bombList.Add(bomb.GetComponent<Bomb>());
            bomb.gameObject.SetActive(false);
        }   
    }

    public GameObject UseBomb(Vector3 position)
    {
        // 3개 미만 인덱스 사용
        _bombIdx = (++_bombIdx) % MaxBombN;

        GameObject BombObject = _bombList[_bombIdx].gameObject;

        BombObject.SetActive(true);
        BombObject.transform.position = position;

        return BombObject;
    }
}
