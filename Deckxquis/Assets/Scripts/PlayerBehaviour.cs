using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private BodyPartBehavior[] _bodyParts = new BodyPartBehavior[5];
    public int Speed { get => calculateSpeed(); }
    
    private int calculateSpeed() {
        int totalSpeed = 0;
        foreach (BodyPartBehavior bodyPart in _bodyParts) {
            totalSpeed += bodyPart.Speed;
        }
        return totalSpeed;
    }
    
    public void Update() {
        foreach (BodyPartBehavior bodyPart in _bodyParts) {
            if (bodyPart.IsEmpty) return;
                // bodyPart.NewBodyPart(CardPickerBehaviour.PickCard(bodyPart.GetCardType));
        }
    }
}
