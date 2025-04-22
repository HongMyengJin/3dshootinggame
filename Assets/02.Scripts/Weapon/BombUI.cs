using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BombUI : MonoBehaviour
{
    public static BombUI Instance;

    public TextMeshProUGUI CurText;
    public TextMeshProUGUI MaxText;

    public Image[] BombImage;

    private const int _maxBomb = 3;

    public int CurBomb
    {
        get;
        set;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CurBomb = _maxBomb;

        ChangeText();
    }

    public void ChangeText()
    {
        CurText.text = CurBomb.ToString();
        MaxText.text = _maxBomb.ToString();
    }
    public void UpdateImage(int unActiveIdx)
    {
        if (BombImage.Length <= unActiveIdx || 0 > unActiveIdx)
            return;
        BombImage[unActiveIdx].enabled = false;
    }

    public bool CheckUseBomb()
    {
        if (CurBomb <= 0)
            return false;

        return true;
    }
    public void UseBomb()
    {
        --CurBomb;
        UpdateImage(_maxBomb - CurBomb -1);
        ChangeText();
    }
}
