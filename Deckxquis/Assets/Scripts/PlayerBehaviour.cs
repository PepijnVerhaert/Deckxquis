using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private CardPickerBehaviour _cardPickerBehaviour;
    [SerializeField] private HeadBehavior _head;
    [SerializeField] private BodyPartBehavior _torso;
    [SerializeField] private BodyPartBehavior _leftArm;
    [SerializeField] private BodyPartBehavior _rightArm;
    [SerializeField] private BodyPartBehavior _leftLeg;
    [SerializeField] private BodyPartBehavior _rightLeg;

    [SerializeField]
    Text _characterName;

    private PlayerDeckBehaviour _playerDeckBehaviour;

    private TurnTrackerBehavior _turnTracker;

    private HealthTrackerBehaviour _healthTracker = null;
    public HealthTrackerBehaviour HealthTracker { get { if (_healthTracker == null) _healthTracker = FindObjectOfType<HealthTrackerBehaviour>(); return _healthTracker; } }

    private EnergyTrackerBehaviour _energyTracker = null;
    public EnergyTrackerBehaviour EnergyTracker { get { if (_energyTracker == null) _energyTracker = FindObjectOfType<EnergyTrackerBehaviour>(); return _energyTracker; } }

    public int Speed { get => calculateSpeed(); }

    private bool _isPicking = false;
    private bool _pickedHead = false;

    private bool _isInitialized = false;
    private GameMangerBehavior _gameMangerBehavior;

    public void Start()
    {
        _gameMangerBehavior = GameObject.Find("GameManager").GetComponent<GameMangerBehavior>();
        _turnTracker = GameObject.Find("TurnTracker").GetComponent<TurnTrackerBehavior>();
        _playerDeckBehaviour = GameObject.Find("PlayerDecks").GetComponent<PlayerDeckBehaviour>();
    }

    private int calculateSpeed()
    {
        int totalSpeed = 0;
        totalSpeed += _head.Speed;
        totalSpeed += _torso.Speed;
        totalSpeed += _leftArm.Speed;
        totalSpeed += _rightArm.Speed;
        totalSpeed += _leftLeg.Speed;
        totalSpeed += _rightLeg.Speed;
        return totalSpeed;
    }

    public void AddBodyPart(CardProperties cardProperties)
    {
        switch (cardProperties.Type)
        {
            case CardType.Head:
                _head.SetCardProperties(cardProperties);
                HealthTracker.MaxHealthLevel = _head.MaxHealth;
                HealthTracker.removeDefence();
                EnergyTracker.MaxBaseEnergyLevel = _head.MaxEnergy;
                EnergyTracker.resetEnergy();
                _pickedHead = true;
                break;
            case CardType.Arm:
                if (_leftArm.IsEmpty)
                {
                    _leftArm.setCardProperties(cardProperties);
                }
                else if (_rightArm.IsEmpty)
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

    private void HideEmpty()
    {
        if (_torso.IsEmpty) _torso.Hide();
        if (_leftArm.IsEmpty) _leftArm.Hide();
        if (_rightArm.IsEmpty) _rightArm.Hide();
        if (_leftLeg.IsEmpty) _leftLeg.Hide();
        if (_rightLeg.IsEmpty) _rightLeg.Hide();
    }

    private void PickBodyParts()
    {
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
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Head, PickContext.CardFromDeck);
        }
        if (_torso.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Torso) > 0)
        {
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Torso, PickContext.CardFromDeck);
        }
        if (_leftArm.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Arm) > 0)
        {
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Arm, PickContext.CardFromDeck);
        }
        if (_rightArm.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Arm) > 0)
        {
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Arm, PickContext.CardFromDeck);
        }
        if (_leftLeg.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Leg) > 0)
        {
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Leg, PickContext.CardFromDeck);
        }
        if (_rightLeg.IsEmpty && _playerDeckBehaviour.CardsLeft(CardType.Leg) > 0)
        {
            _isPicking = true;
            _cardPickerBehaviour.PickNewCard(CardType.Leg, PickContext.CardFromDeck);
        }
    }

    public void Update()
    {
        HideEmpty();
        PickBodyParts();
        UpdateName();
        if (!_isInitialized && HasBodyParts())
        {
            _isInitialized = true;
            _turnTracker.SetPlayer(_head.Id, _head.Speed);
            _gameMangerBehavior.PlayerReady();
        }
    }

    private void UpdateName()
    {
        string headName = string.Empty;
        if (_pickedHead)
        {
            headName = _head.Name + " ";
        }

        string theThe = string.Empty;
        if (_pickedHead && !_torso.IsEmpty)
        {
            theThe = "The ";
        }

            string leftArmName = string.Empty;
        if(!_leftArm.IsEmpty)
            leftArmName = _leftArm.Name + " ";

        string rightArmName = string.Empty;
        if(!_rightArm.IsEmpty)
            rightArmName = _rightArm.Name + " ";

        string leftLegName = string.Empty;
        if(!_leftLeg.IsEmpty)
            leftLegName = _leftLeg.Name + " ";

        string rightLegName = string.Empty;
        if(!_rightLeg.IsEmpty)
            rightLegName = _rightLeg.Name + " ";

        string torsoName = string.Empty;
        if(!_torso.IsEmpty)
            torsoName = _torso.Name;

        string fullName = headName + theThe + leftArmName + rightArmName + leftLegName + rightLegName + torsoName;
        _characterName.text = fullName;
    }
}
