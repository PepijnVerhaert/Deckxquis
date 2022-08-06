using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    // TYPES
       
    public void setProperties(CardProperties properties)
    {
        _properties = properties;
    }

    public void setCardValues()
    {
        CardIconSlotFiller slotFiller = GetComponent<CardIconSlotFiller>();
        slotFiller.SetSlotsCorrect();
        switch (GetCardType)
        {
            case CardType.Arm:
            case CardType.Enemy:
                slotFiller.SetSlotsToCardTop(true);
                break;
            case CardType.Torso:
            case CardType.Leg:
                slotFiller.SetSlotsToCardTop(false);
                break;
            default:
                break;
        }
    }

    private CardProperties _properties;
    
    public CardProperties Properties { get; }

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
    public string Id { get => _properties.Id; }
    public string ImageName { get => _properties.ImageName; }
}
