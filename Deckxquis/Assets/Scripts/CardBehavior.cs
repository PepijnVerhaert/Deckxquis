using UnityEngine;

public enum CardSide {
    Front,
    Back,
    None,
};
     
public class CardBehavior : MonoBehaviour
{
    //sprite renderers
    [SerializeField]
    SpriteRenderer _frontImage;

    [SerializeField]
    SpriteRenderer _backImage;

    //images
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

    //visuals
    [SerializeField]
    GameObject _frontVisuals;

    [SerializeField]
    GameObject _backVisuals;

    [SerializeField]
    GameObject _emptyVisuals;


    public void Show(CardSide side) {
        switch (side) 
        {
            case CardSide.Front:
                _frontVisuals.SetActive(true);
                _backVisuals.SetActive(false);
                _emptyVisuals.SetActive(false);
                break;
            case CardSide.Back:
                _frontVisuals.SetActive(false);
                _backVisuals.SetActive(true);
                _emptyVisuals.SetActive(false);
                break;
            case CardSide.None:
            default:
                _frontVisuals.SetActive(false);
                _backVisuals.SetActive(false);
                _emptyVisuals.SetActive(true);

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
        _frontImage.sprite = Resources.Load<Sprite>(_properties.ImageName);

        switch (_properties.Type)
        {
            case CardType.Head:
                _backImage.sprite = _headBackImage;
                break;
            case CardType.Arm:
                _backImage.sprite = _armBackImage;
                break;
            case CardType.Torso:
                _backImage.sprite = _torsoBackImage;
                break;
            case CardType.Leg:
                _backImage.sprite = _legBackImage;
                break;
            case CardType.Enemy:
                _backImage.sprite = _enemyBackImage;
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
