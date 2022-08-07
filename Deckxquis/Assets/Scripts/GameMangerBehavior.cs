using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

public enum InputState {
    CardSelect,
    PlayerSelect,
    EnemySelect,
    EnemyTurn,
    None,
}

public class GameMangerBehavior : MonoBehaviour
{
    private InputState _inputState;
    public Ray _ray = new Ray();
    public RaycastHit2D _hit;
    [SerializeField] public Camera _camera;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CardPickerBehaviour _cardPicker;
    private GameObject _hitObject;

    private TurnTrackerBehavior _turnTrackerBehavior;
    
    public void Start()
    {
        _playerInput.actions["Activate"].performed += Activate;
        _playerInput.actions["Deactivate"].performed += Deactivate;
        _turnTrackerBehavior = GameObject.Find("TurnTracker").GetComponent<TurnTrackerBehavior>();
    }

    public void StartCombat()
    {
        _inputState = InputState.None;
        _turnTrackerBehavior.StartCombat();
    }

    public void SetInputState(InputState state) 
    {
        _inputState = state;
    }

    private string GetLayerMask()
    {
        // Layers: "Player", "Enemy", "Picker", "Turn"
        switch (_inputState)
        {
            case InputState.CardSelect:
                return "Picker";
            case InputState.PlayerSelect:
                return "Player";
            case InputState.EnemySelect:
                return "Enemy";
            case InputState.EnemyTurn:
                return "Turn";
            case InputState.None:
                break;
            default:
                Debug.Log("boze error message!");
                break;
        }
        return null;
    }

    public void Activate(InputAction.CallbackContext callback)
    {
        _ray = _camera.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        ContactFilter2D contactFilter = new ContactFilter2D();
        List<RaycastHit2D> hitResults = new List<RaycastHit2D>();
        var hit = Physics2D.Raycast(_ray.origin, _ray.direction, contactFilter, hitResults, 100);

        if (hit > 0)
        {
            foreach (var hitResult in hitResults)
            {
                if (hitResult.collider.gameObject.layer == LayerMask.NameToLayer(GetLayerMask()))
                {
                    _hit = hitResult;
                    _hitObject = _hit.collider.gameObject;

                    if (_inputState == InputState.PlayerSelect && _hitObject.CompareTag("EndTurn"))
                    {
                        _inputState = InputState.None;
                        Debug.Log("EndTurn");
                        //_turnTrackerBehavior.EndTurn();
                        return;
                    }

                    Debug.Log("HIT!");
                    CardBehavior clickedCardBehavior = _hitObject.GetComponent<CardBehavior>();

                    switch (_inputState)
                    {
                        case InputState.CardSelect:
                            _inputState = InputState.None;
                            StartCoroutine(_cardPicker.handleCardPick(clickedCardBehavior));
                            break;
                        case InputState.PlayerSelect:
                            // TODO if turntracker is clicked -> notify turnTracker
                            // TODO if player -> handle picked action
                            break;
                        case InputState.EnemyTurn:
                            // TODO if turntracker is clicked -> notify turnTracker
                            break;
                    }
                    return;
                }
            }
        }
    }

    public void Deactivate(InputAction.CallbackContext callback)
    {
        _hitObject = null;
        Debug.Log("UNHIT!");
    }
}
