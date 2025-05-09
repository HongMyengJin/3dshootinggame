﻿using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FPSView : ICameraView
{
    private Transform cameraTransform;
    private Transform weaponTransform;
    private Transform playerMeshTransform;
    private GameObject player;

    private float yaw;
    private float pitch;

    private readonly Vector3 offset = new Vector3(0, 0.5f, 0.0f);

    public FPSView(Transform _weaponTransform, GameObject _player)
    {
        this.weaponTransform = _weaponTransform;
        this.player = _player;

        SettingCursor();
    }

    public void UpdateView()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -60.0f, 60.0f);

        player.transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }

    public void SettingCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
