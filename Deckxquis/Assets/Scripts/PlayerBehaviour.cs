using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private CardPickerBehaviour _cardPickerBehaviour;
    [SerializeField] private BodyPartBehavior[] _bodyParts = new BodyPartBehavior[5];
    public int Speed { get => calculateSpeed(); }
    private CardType _pickingType;
    private bool _isPicking = false;
    
    private int calculateSpeed() {
        int totalSpeed = 0;
        foreach (BodyPartBehavior bodyPart in _bodyParts) {
            totalSpeed += bodyPart.Speed;
        }
        return totalSpeed;
    }
     
    public void AddBodyPart(CardProperties cardProperties) {
        foreach (BodyPartBehavior bodyPart in _bodyParts) {
            if (bodyPart.IsEmpty && _pickingType == bodyPart.GetCardType) {
                bodyPart.setCardProperties(cardProperties);
                _isPicking = true;
            }
        }
    }
    
    public void Update() {
        foreach (BodyPartBehavior bodyPart in _bodyParts) {
            if (!_isPicking && bodyPart.IsEmpty) {
                _pickingType = bodyPart.GetCardType;
                _isPicking = true;
                _cardPickerBehaviour.PickCardFromDeck(bodyPart.GetCardType);
            }
        }
    }
}
