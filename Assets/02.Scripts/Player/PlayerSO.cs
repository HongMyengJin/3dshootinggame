using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField]
    public float JumpPower; 
    [SerializeField]
    public float Gravity; 
    [SerializeField]
    public float MaxStamina;

    [SerializeField]
    public float StaminaUseSpeed;
    [SerializeField]
    public float StaminaFillSpeed; 

    [SerializeField]
    public float StaminaDashUseSpeed;

    [SerializeField]
    public float MaxHealth;
}
