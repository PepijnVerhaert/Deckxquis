using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    // TYPES
    enum CardType
    {
        Head,
        Arm,
        Torso,
        Leg,
        Enemy,
    }

    private CardType _cardType;

    // PASSIVE
    private int _speed = 0;

    // COST
    private int _energyCost = 0;
    private int _healthCost = 0;

    // ACTIVE
    private int _energy = 0;
    private int _defence = 0;
    private int _attack = 0;
    private int _health = 0;

    // USES
    private int _uses = -1;

    // PROPERTIES
    public int Speed { get => _speed; set => _speed = value; }
    public int EnergyCost { get => _energyCost; set => _energyCost = value; }
    public int HealthCost { get => _healthCost; set => _healthCost = value; }
    public int Energy { get => _energy; set => _energy = value; }
    public int Defence { get => _defence; set => _defence = value; }
    public int Attack { get => _attack; set => _attack = value; }
    public int Uses { get => _uses; set => _uses = value; }
    public int Health { get => _health; set => _health = value; }
    private CardType CardType1 { get => _cardType; set => _cardType = value; }
}
