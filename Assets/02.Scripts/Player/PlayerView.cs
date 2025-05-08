using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform meshTransform;
    public Transform MeshTransform => meshTransform;
}