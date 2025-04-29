using System;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStat : MonoBehaviour
{
    public enum Stat
    {
        Stamina,
        Health,
        StatEnd
    }

    public PlayerSO PlayerDataSo;
    public Slider[] Slider;

    private float[] _curValue = new float[(int)Stat.StatEnd];

    public float CurStamina => _curValue[(int)Stat.Stamina];
    public float CurHealth => _curValue[(int)Stat.Health];
    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        _curValue[(int)Stat.Stamina] = PlayerDataSo.MaxStamina;
        _curValue[(int)Stat.Health] = PlayerDataSo.MaxHealth;
        ChangeValue(Stat.Stamina);
        ChangeValue(Stat.Health);
    }
    public void ChangeValue(Stat stat, float add = 0.0f)
    {
        float maxValue = stat == Stat.Health ? PlayerDataSo.MaxHealth : PlayerDataSo.MaxStamina;

        _curValue[(int)stat] += add;
        if (_curValue[(int)stat] < 0.0f)
            _curValue[(int)stat] = 0.0f;
        else if (_curValue[(int)stat] > maxValue)
            _curValue[(int)stat] = maxValue;

        Slider[(int)stat].value = _curValue[(int)stat] / maxValue;
    }
}
