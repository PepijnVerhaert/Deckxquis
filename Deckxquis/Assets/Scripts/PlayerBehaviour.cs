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

    private HealthTrackerBehaviour _healthTracker = null;
    public HealthTrackerBehaviour HealthTracker { get { if (_healthTracker == null) _healthTracker = FindObjectOfType<HealthTrackerBehaviour>(); return _healthTracker; } }

    private EnergyTrackerBehaviour _energyTracker = null;
    public EnergyTrackerBehaviour EnergyTracker { get { if (_energyTracker == null) _energyTracker = FindObjectOfType<EnergyTrackerBehaviour>(); return _energyTracker; } }

    
    
    public int Speed { get => calculateSpeed(); }

    private BodyPartBehavior _pickingPart;
    private bool _isPicking = false;
    private bool _isPickingHead = false;
    private bool _pickedHead = false;

    
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
        if (_isPickingHead) {
            _head.SetCardProperties(cardProperties);
            HealthTracker.MaxHealthLevel = _head.MaxHealth;
            EnergyTracker.MaxBaseEnergyLevel = _head.MaxEnergy;
            _isPickingHead = false;
            _pickedHead = true;
        }  else  {
            _pickingPart.setCardProperties(cardProperties);
        }
        _isPicking = false;
    }
    
    public void Update() {
        if (!_isPicking) {
            if (!_pickedHead)  {
                _isPickingHead = true;
                _isPicking = true;
                _cardPickerBehaviour.PickCardFromDeck(CardType.Head);
            } else if (_torso.IsEmpty) {
                _pickingPart = _torso;
                _isPicking = true;
                _cardPickerBehaviour.PickCardFromDeck(CardType.Torso);
            } else if (_leftArm.IsEmpty) {
                _pickingPart = _leftArm;
                _isPicking = true;
                _cardPickerBehaviour.PickCardFromDeck(CardType.Arm);
            } else if (_rightArm.IsEmpty) {
                _pickingPart = _rightArm;
                _isPicking = true;
                _cardPickerBehaviour.PickCardFromDeck(CardType.Arm);
            } else if (_leftLeg.IsEmpty) {
                _pickingPart = _leftLeg;
                _isPicking = true;
                _cardPickerBehaviour.PickCardFromDeck(CardType.Leg);
            } else if (_rightLeg.IsEmpty) {
                _pickingPart = _rightLeg;
                _isPicking = true;
                _cardPickerBehaviour.PickCardFromDeck(CardType.Leg);
            }
        }
    }
}
