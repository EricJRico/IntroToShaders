using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DamageOnClick : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private float range = 5f;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            Shoot();
    }

    void Shoot()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.value);

        if (!Physics.Raycast(ray, out RaycastHit hit, range))
            return;

        var enemy = hit.transform.GetComponent<Enemy>();

        if (enemy == null)
            return;

        enemy.Damage();
    }
}