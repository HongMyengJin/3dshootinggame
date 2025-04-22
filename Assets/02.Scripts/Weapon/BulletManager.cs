using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance;

    public GameObject BulletPrefab;
    private List<Bullet> _bulletList = new List<Bullet>();
    public int MaxBulletN = 50;

    private int _bulletIdx = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SettingBullets();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SettingBullets()
    {
        for(int i = 0; i < MaxBulletN; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.transform.parent = gameObject.transform;

            _bulletList.Add(bullet.GetComponent<Bullet>());
            bullet.gameObject.SetActive(false);
        }   
    }

    public GameObject UseBullet(Vector3 position)
    {
        // 3개 미만 인덱스 사용
        _bulletIdx = (++_bulletIdx) % MaxBulletN;

        
        GameObject BulletObject = _bulletList[_bulletIdx].gameObject;
        BulletObject.SetActive(true);
        _bulletList[_bulletIdx].PlayEffect(position);

        BulletUI.Instance.UpdateBulletN(MaxBulletN - _bulletIdx);
        return BulletObject;
    }

    public void ResetBullet()
    {
        for (int i = 0; i < MaxBulletN; i++)
        {
            _bulletList[i].gameObject.SetActive(false);
        }
        _bulletIdx = 0;
        BulletUI.Instance.UpdateBulletN(MaxBulletN - _bulletIdx);
    }
}
