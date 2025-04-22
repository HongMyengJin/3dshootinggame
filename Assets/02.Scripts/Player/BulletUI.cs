using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BulletUI : MonoBehaviour
{
    public static BulletUI Instance;
    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI StateText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateBulletN(int BulletN)
    {
        BulletText.text = BulletN.ToString();
    }

    public void UpdateBulletTimer(float Timer)
    {
        TimerText.text = Timer.ToString("F1");
    }

    public void UpdateState(PlayerFire.ShootEnum shootEnum)
    {
        switch (shootEnum)
        {
            case PlayerFire.ShootEnum.None:
                StateText.text = "";
                break;
            case PlayerFire.ShootEnum.Load:
                StateText.text = "¿Â¿¸ ¡ﬂ";
                break;
        }
    }
}
