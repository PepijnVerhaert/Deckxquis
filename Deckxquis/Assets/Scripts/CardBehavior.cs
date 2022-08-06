using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    // TYPES
       
    public void setProperties(CardProperties properties)
    {
        _properties = properties;
    }

    private CardProperties _properties;

    // PROPERTIES
    public int Speed { get => _properties.Speed; }
    public int EnergyCost { get => _properties.EnergyCost; }
    public int HealthCost { get => _properties.HealthCost; }
    public int Energy { get => _properties.Energy; }
    public int Defence { get => _properties.Defence; }
    public int Attack { get => _properties.Attack; }
    public int Uses { get => _properties.Uses; }
    public int Health { get => _properties.Health; }
    public CardType GetCardType { get => _properties.Type; }
}
