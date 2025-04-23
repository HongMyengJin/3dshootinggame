using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Gun : MonoBehaviour
{
    public static Gun Instance;

    public float recoilForce;
    public float recoilAngle;
    public float recoilDuraion;

    public float delayTime;

    private Camera mainCamera;
    private Quaternion originalCameraRotation;
    private Vector3 originalCameraPosition;

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

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void Shoot()
    {
        originalCameraRotation = mainCamera.transform.localRotation;
        originalCameraPosition = mainCamera.transform.localPosition;
        StartCoroutine(ApplyRecoil(delayTime));
    }

    public void Rebound(float elapsedTime, float startAngle, float endAngle)
    {
        float time = elapsedTime / recoilDuraion;
        float value = Mathf.Lerp(startAngle, endAngle, time);
        mainCamera.transform.localRotation = originalCameraRotation * Quaternion.Euler(-value, 0.0f, 0.0f);
        mainCamera.transform.localPosition = originalCameraPosition + transform.forward * value * Time.deltaTime * 20.0f;
    }

    private IEnumerator ApplyRecoil(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        float elapsedTime = 0.0f;
        while (elapsedTime < recoilDuraion)
        {
            Rebound(elapsedTime, 0.0f, recoilAngle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;
        while (elapsedTime < recoilDuraion)
        {
            Rebound(elapsedTime, recoilAngle, 0.0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localRotation = originalCameraRotation;
        mainCamera.transform.localPosition = originalCameraPosition;
    }
}
