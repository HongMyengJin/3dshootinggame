using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField]
    public float JumpPower; // = 5f;
    [SerializeField]
    public float Gravity; // = -9.8f;
    [SerializeField]
    public float MaxStamina; //  = 100.0f;

    [SerializeField]
    public float StaminaUseSpeed; // = 100.0f;
    [SerializeField]
    public float StaminaFillSpeed; // = 100.0f;

    [SerializeField]
    public float StaminaDashUseSpeed; // = 30.0f;
}
