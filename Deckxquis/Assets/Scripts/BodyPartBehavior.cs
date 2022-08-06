using UnityEngine;
using System;

public class BodyPartBehavior : MonoBehaviour
{
    public enum Side
    {
        Unassigned,
        Left,
        Right,
    }

    private CardBehavior _cardBehavior;
    [SerializeField] private Side _cardSide = Side.Unassigned;
    private int _currentUses = -1;

    // ACTIONS
    public Action<float> UseEnergy = (float amount) => {};
    public Action<float> UseHealth = (float amount) => {};

    public Action<float> GiveEnergy = (float amount) => {};
    public Action<float> GiveHealth = (float amount) => {};
    public Action<float> GiveDefence = (float amount) => {};
    public Action<float> GiveAttack = (float amount) => {};

    public Action<CardBehavior.CardType, Side> OnBodyPartBroken = (CardBehavior.CardType type, Side side) => {};

    public int Speed { get => _cardBehavior.Speed; }
    public Side CardSide { get => _cardSide; set => _cardSide = value; }

    public bool IsEmpty { get => _cardBehavior == null; }
    public CardBehavior.CardType GetCardType { get => _cardBehavior.GetCardType; }

    public void NewBodyPart(CardBehavior newCardBehvavior)
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
            OnBodyPartBroken(_cardBehavior.GetCardType, _cardSide);
            _cardBehavior = null;
        }
    }
}