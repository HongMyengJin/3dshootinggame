using UnityEngine;

public class BombGaugeUI : GaugeUI
{
    public static BombGaugeUI Instance;

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
}
