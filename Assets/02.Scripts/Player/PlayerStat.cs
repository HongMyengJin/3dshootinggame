using UnityEngine;
using UnityEngine.UI;
public class PlayerStat : MonoBehaviour
{
    public PlayerSO PlayerDataSo;
    public Slider Slider;
    public float CurStamina {
        get; 
        set; 
    }


    private void Start()
    {
        CurStamina = PlayerDataSo.MaxStamina;
        ChangeStamina();
    }
    public void ChangeStamina()
    {
        if(CurStamina < 0.0f)
            CurStamina = 0.0f;
        else if (CurStamina > PlayerDataSo.MaxStamina)
            CurStamina = PlayerDataSo.MaxStamina;

        Slider.value = CurStamina / PlayerDataSo.MaxStamina;
    }

}
