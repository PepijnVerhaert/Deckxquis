using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState {
    CardSelect,
    ActionSelect,
    EnemyTurn,
}

public class GameMangerBehavior : MonoBehaviour
{
    private InputState _gameState;
    public Ray _ray = new Ray();
    public RaycastHit2D _hit;
    [SerializeField] public Camera _camera;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CardPickerBehaviour _cardPicker;
    private GameObject _hitObject;
    
    public void Start()
    {
        _playerInput.actions["Activate"].performed += Activate;
        _playerInput.actions["Deactivate"].performed += Deactivate;
    }
    
    public void SetInputState(InputState state) 
    {
        _gameState = state;
    }

    public void Activate(InputAction.CallbackContext callback)
    {
        _ray = _camera.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        _hit = Physics2D.Raycast(_ray.origin, _ray.direction, 1000);
        if (_hit.collider != null)
        {
            _hitObject = _hit.collider.gameObject;
            // TODO get card and filter by layer
            CardProperties clickedCardProperties;
            Debug.Log("HIT!");
            switch (_gameState)
            {
                case InputState.CardSelect:
                    _cardPicker.handleCardPick(clickedCardProperties);
                    break;
                case InputState.ActionSelect:
                    // TODO if turntracker is clicked -> notify turnTracker
                    // TODO if player -> handle picked action
                    break;
                case InputState.EnemyTurn:
                    // TODO if turntracker is clicked -> notify turnTracker
                    break;
            }
        }
    }

    public void Deactivate(InputAction.CallbackContext callback)
    {
        _hitObject = null;
        Debug.Log("UNHIT!");
    }
}
