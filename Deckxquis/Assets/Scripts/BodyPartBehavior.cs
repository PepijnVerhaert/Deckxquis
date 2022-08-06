using UnityEngine;
using System;

public class BodyPartBehavior : MonoBehaviour
{
    CardBehavior _cardBehavior;
    private int _currentUses = -1;

    // ACTIONS
    public Action<float> UseEnergy = (float amount) => {};
    public Action<float> UseHealth = (float amount) => {};

    public Action<float> GiveEnergy = (float amount) => {};
    public Action<float> GiveHealth = (float amount) => {};
    public Action<float> GiveDefence = (float amount) => {};
    public Action<float> GiveAttack = (float amount) => {};

    public Action BodyPartBroken = () => {};

    void NewBodyPart(CardBehavior newCardBehvavior)
    {
        if (_currentUses <= 0)
        {
            _cardBehavior = newCardBehvavior;
            _currentUses = newCardBehvavior.Uses;
        }
    }

    void UseBodyPart()
    {
        --_currentUses;
        
        UseEnergy(_cardBehavior.EnergyCost);
        UseHealth(_cardBehavior.HealthCost);
        GiveEnergy(_cardBehavior.Energy);
        GiveHealth(_cardBehavior.Health);
        GiveDefence(_cardBehavior.Defence);
        GiveAttack(_cardBehavior.Attack);

        if (_currentUses <= 0)
        {
            BodyPartBroken();
        }
    }
}
