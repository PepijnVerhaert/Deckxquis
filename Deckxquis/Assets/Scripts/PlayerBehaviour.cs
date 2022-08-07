using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private CardPickerBehaviour _cardPickerBehaviour;
    [SerializeField] private HeadBehavior _head;
    [SerializeField] private BodyPartBehavior _torso;
    [SerializeField] private BodyPartBehavior _leftArm;
    [SerializeField] private BodyPartBehavior _rightArm;
    [SerializeField] private BodyPartBehavior _leftLeg;
    [SerializeField] private BodyPartBehavior _rightLeg;

    private PlayerDeckBehaviour _playerDeckBehaviour;

    private TurnTrackerBehavior _turnTracker;

    private HealthTrackerBehaviour _healthTracker = null;
    public HealthTrackerBehaviour HealthTracker { get { if (_healthTracker == null) _healthTracker = FindObjectOfType<HealthTrackerBehaviour>(); return _healthTracker; } }

    private EnergyTrackerBehaviour _energyTracker = null;
    public EnergyTrackerBehaviour EnergyTracker { get { if (_energyTracker == null) _energyTracker = FindObjectOfType<EnergyTrackerBehaviour>(); return _energyTracker; } }

    public int Speed { get => calculateSpeed(); }

    private bool _isPicking = false;
    private bool _isPickingHead = false;
    private bool _pickedHead = false;

    private bool _isInitialized = false;
    private GameMangerBehavior _gameMangerBehavior;

    public void Start()
    {
        _gameMangerBehavior = GameObject.Find("GameManager").GetComponent<GameMangerBehavior>();
        _turnTracker = GameObject.Find("TurnTracker").GetComponent<TurnTrackerBehavior>();
        _playerDeckBehaviour = GameObject.Find("PlayerDecks").GetComponent<PlayerDeckBehaviour>();
    }

    private int calculateSpeed() {
        int totalSpeed = 0;
        totalSpeed += _head.Speed;
        totalSpeed += _torso.Speed;
        totalSpeed += _leftArm.Speed;
        totalSpeed += _rightArm.Speed;
        totalSpeed += _leftLeg.Speed;
        totalSpeed += _rightLeg.Speed;
        return totalSpeed;
    }
     
    public void AddBodyPart(CardProperties cardProperties) {
        switch (cardProperties.Type)
        {
            case CardType.Head:
                _head.SetCardProperties(cardProperties);
                HealthTracker.MaxHealthLevel = _head.MaxHealth;
                HealthTracker.removeDefence();
                EnergyTracker.MaxBaseEnergyLevel = _head.MaxEnergy;
                EnergyTracker.resetEnergy();
                _isPickingHead = false;
                _pickedHead = true;
                break;
            case CardType.Arm:
                if (_leftArm.IsEmpty)
                {
                    _leftArm.setCardProperties(cardProperties);
                }
                else if(_rightArm.IsEmpty)
                {
                    _rightArm.setCardProperties(cardProperties);
                }
                break;
            case CardType.Torso:
                _torso.setCardProperties(cardProperties);
                break;
            case CardType.Leg:
                if (_leftLeg.IsEmpty)
                {
                    _leftLeg.setCardProperties(cardProperties);
                }
                else if (_rightLeg.IsEmpty)
                {
                    _rightLeg.setCardProperties(cardProperties);
                }
                break;
            default:
                break;
        }
    }

    public void PlayerReset()
    {
        _healthTracker.removeDefence();
        _energyTracker.resetEnergy();
    }
    
    private bool HasBodyParts()
    {
        return (_pickedHead && !_torso.IsEmpty && !_leftArm.IsEmpty && !_rightArm.IsEmpty && !_leftLeg.IsEmpty && !_rightLeg.IsEmpty);

    }

    public void Update() {

        if (HasBodyParts())
        {
            _isPicking = false;
        }

        if (_isPicking)
        {
            return;
        }

        if (!_pickedHead)
        {
            _isPickingHead = true;
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Head, PickContext.CardFromDeck);
        }
        if (_torso.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Torso) > 0)
        {
            _torso.Hide();
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Torso, PickContext.CardFromDeck);
        }
        if (_leftArm.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Arm) > 0)
        {
            _leftArm.Hide();
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Arm, PickContext.CardFromDeck);
        }
        if (_rightArm.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Arm) > 0)
        {
            _rightArm.Hide();
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Arm, PickContext.CardFromDeck);
        }
        if (_leftLeg.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Leg) > 0)
        {
            _leftLeg.Hide();
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Leg, PickContext.CardFromDeck);
        }
        if (_rightLeg.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Leg) > 0)
        {
            _rightLeg.Hide();
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Leg, PickContext.CardFromDeck);
        }
        if (!_isInitialized && _pickedHead && !_torso.IsEmpty && !_leftArm.IsEmpty && !_rightArm.IsEmpty && !_leftLeg.IsEmpty && !_rightLeg.IsEmpty)
        {
            _isInitialized = true;
            _turnTracker.SetPlayer(_head.Id, _head.Speed);
            _gameMangerBehavior.PlayerReady();
        }
    }
}
