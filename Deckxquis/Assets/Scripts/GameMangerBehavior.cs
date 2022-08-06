using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMangerBehavior : MonoBehaviour
{
    public Ray _ray = new Ray();
    public RaycastHit2D _hit;
    [SerializeField] public Camera _camera;

    [SerializeField] private PlayerInput _playerInput;

    private GameObject _hitObject;

    public void Start()
    {
        _playerInput.actions["Activate"].performed += Activate;
        _playerInput.actions["Deactivate"].performed += Deactivate;
    }

    public void Activate(InputAction.CallbackContext callback)
    {
        _ray = _camera.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        _hit = Physics2D.Raycast(_ray.origin, _ray.direction, 1000);
        if (_hit.collider != null)
        {
            _hitObject = _hit.collider.gameObject;
            Debug.Log("HIT!");
        }
    }

    public void Deactivate(InputAction.CallbackContext callback)
    {
        _hitObject = null;
        Debug.Log("UNHIT!");
    }
}
