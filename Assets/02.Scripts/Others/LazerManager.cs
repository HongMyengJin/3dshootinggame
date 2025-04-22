using System.Collections;
using System.Drawing;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LazerManager : MonoBehaviour
{
    public static LazerManager Instance;

    public LineRenderer LineRenderer;
    private Vector3 _dir;

    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private float _length;

    private float _moveSpeed = 15.0f;


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

    public void SettingLine(Vector3 StartPoint, Vector3 EndPoint)
    {
        LineRenderer.gameObject.SetActive(true);
        SetPoints(StartPoint, EndPoint);
        UpdateLine();

        _length = Vector3.Distance(_endPoint, _startPoint);
        _dir = (_endPoint - _startPoint).normalized;

        StartCoroutine(CoMove());
    }


    public void Moving()
    {
        _startPoint += _dir * Time.deltaTime * 3.0f * _moveSpeed;
        UpdateLine();
    }

    public void UpdateLine()
    {
        LineRenderer.SetPosition(0, _startPoint);
        LineRenderer.SetPosition(1, _endPoint);

    }

    public void SetPoints(Vector3 StartPoint, Vector3 EndPoint)
    {
        _startPoint = StartPoint;
        _endPoint = EndPoint;
    }

    IEnumerator CoMove()
    {
        while (0.0f < _length)
        {
            _length -= Time.deltaTime * 3.0f * _moveSpeed;
            Moving();
            yield return null;
        }

        LineRenderer.gameObject.SetActive(false);
    }

}
