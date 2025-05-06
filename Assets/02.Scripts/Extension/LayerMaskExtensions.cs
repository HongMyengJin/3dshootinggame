using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask mask, GameObject obj)
    {
        return ((1 << obj.layer) & mask.value) != 0;
    }
}
