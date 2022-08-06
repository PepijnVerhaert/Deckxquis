using UnityEngine;

public enum CardSide {
    Front,
    Back,
    None,
};
     
public class CardBehavior : MonoBehaviour
{
    [SerializeField]
    Sprite _frontImage;

    [SerializeField]
    Sprite _backImage;

    [SerializeField]
    Sprite _headBackImage;

    [SerializeField]
    Sprite _armBackImage;

    [SerializeField]
    Sprite _torsoBackImage;

    [SerializeField]
    Sprite _legBackImage;

    [SerializeField]
    Sprite _enemyBackImage;



    public void Show(CardSide side) {
        switch (side) 
        {
            case CardSide.Front:

                break;
            case CardSide.Back:

                break;
            case CardSide.None:
            default:

                
                // TODO show this side
                break;
        }
    }
      
    public void SetProperties(CardProperties properties)
    {
        _properties = properties;
        SetCardVisuals();
    }

    private void SetCardVisuals()
    {
        _frontImage = Resources.Load<Sprite>(_properties.ImageName);

        switch (_properties.Type)
        {
            case CardType.Head:
                _backImage = _headBackImage;
                break;
            case CardType.Arm:
                _backImage = _armBackImage;
                break;
            case CardType.Torso:
                _backImage = _torsoBackImage;
                break;
            case CardType.Leg:
                _backImage = _legBackImage;
                break;
            case CardType.Enemy:
                _backImage = _enemyBackImage;
                break;
            default:
                break;
        }

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
