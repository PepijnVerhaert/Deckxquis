using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehavior : MonoBehaviour
{
    private CardBehavior _cardBehavior;

    public int MaxHealth
    {
        get { return _cardBehavior.Health; }
    }

    public int MaxEnergy
    {
        get { return _cardBehavior.Energy; }
    }

    public int Speed
    {
        get { return _cardBehavior.Speed; }
    }

    public void SetHead(CardBehavior cardBehavior)
    {
         _cardBehavior = cardBehavior;
    }
}
