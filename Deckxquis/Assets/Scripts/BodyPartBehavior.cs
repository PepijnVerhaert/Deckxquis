using UnityEngine;
using System;
using UnityEngine.UI;

public class BodyPartBehavior : MonoBehaviour
{
    public enum Side
    {
        Unassigned,
        Left,
        Right,
    }

    [SerializeField]
    Text _currentUsesText;
    [SerializeField]
    Text _totalUsesText;

    [SerializeField]
    private CardBehavior _cardBehavior;
    [SerializeField] private Side _cardSide = Side.Unassigned;
    private int _currentUses = -1;

    private GameMangerBehavior _gameManagerBehavior;
    private EnergyTrackerBehaviour _energyTrackerBehavior;
    private HealthTrackerBehaviour _healthTrackerBehavior;

    // ACTIONS
    public Action<float> UseEnergy = (float amount) => {};
    public Action<float> UseHealth = (float amount) => {};

    public Action<float> GiveEnergy = (float amount) => {};
    public Action<float> GiveHealth = (float amount) => {};
    public Action<float> GiveDefence = (float amount) => {};
    public Action<float> GiveAttack = (float amount) => {};

    public Action<CardType, Side> OnBodyPartBroken = (CardType type, Side side) => {};
    
    public int Speed { get => _cardBehavior.Speed; }
    public string Name { get => _cardBehavior.Name; }
    public string Id { get => _cardBehavior.Id; }
    public Side CardSide { get => _cardSide; set => _cardSide = value; }

    public bool IsEmpty { get => _cardBehavior.Properties == null || _currentUses <= 0; }
    public CardType GetCardType { get => _cardBehavior.GetCardType; }

    public void Start()
    {
        _gameManagerBehavior = GameObject.Find("GameManager").GetComponent<GameMangerBehavior>();
        _energyTrackerBehavior = GameObject.Find("EnergyTracker").GetComponent<EnergyTrackerBehaviour>();
        _healthTrackerBehavior = GameObject.Find("HealthTracker").GetComponent<HealthTrackerBehaviour>();
    }

    public void Hide()
    {
        _cardBehavior.Show(global::CardSide.None);
    }

    public void setCardProperties(CardProperties cardProperties) {
        _cardBehavior.SetProperties(cardProperties);
        _currentUses = cardProperties.Uses;
        _cardBehavior.Show(global::CardSide.Front);

        _totalUsesText.text = cardProperties.Uses.ToString();
        _currentUsesText.text = cardProperties.Uses.ToString();
    }

    public void UseBodyPart()
    {
        if (_energyTrackerBehavior.EnergyLevel < _cardBehavior.EnergyCost)
        {
            return;
        }

        if (_cardBehavior.Attack > 0)
        {
            _gameManagerBehavior.SetInputState(InputState.EnemySelect);
            _gameManagerBehavior.Damage = _cardBehavior.Attack;
            _gameManagerBehavior.BodyPartBehavior = this;
        }
        else
        {
            UsePassives();
        }
    }

    public void UsePassives()
    {
        --_currentUses;
        _currentUsesText.text = _currentUses.ToString();

        _energyTrackerBehavior.useEnergy(_cardBehavior.EnergyCost);
        _healthTrackerBehavior.reduceHealth(_cardBehavior.HealthCost);

        _energyTrackerBehavior.addEnergy(_cardBehavior.Energy);
        _healthTrackerBehavior.heal(_cardBehavior.Health);
        _healthTrackerBehavior.increaseDefence(_cardBehavior.Defence);

        if (_currentUses <= 0)
        {
            OnBodyPartBroken(_cardBehavior.GetCardType, _cardSide);
            _currentUsesText.text = String.Empty;
            _totalUsesText.text = String.Empty;
        }
    }
}
