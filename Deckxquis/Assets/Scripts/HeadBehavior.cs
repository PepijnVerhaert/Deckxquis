using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehavior : MonoBehaviour
{
    [SerializeField]
    private CardBehavior _cardBehavior;
    
    public void SetCardProperties(CardProperties cardProperties) {
        _cardBehavior.SetProperties(cardProperties);
        _cardBehavior.Show(global::CardSide.Front);
    }

    public int MaxHealth
    {
        get { return _cardBehavior.HealthCost; }
    }

    public string Name
    {
        get { return _cardBehavior.Name; }
    }

    public int MaxEnergy
    {
        get { return _cardBehavior.Energy; }
    }

    public int Speed
    {
        get { return _cardBehavior.Speed; }
    }
    public string Id { get => _cardBehavior.Id; }

    public void SetHead(CardBehavior cardBehavior)
    {
         _cardBehavior = cardBehavior;
    }
}
