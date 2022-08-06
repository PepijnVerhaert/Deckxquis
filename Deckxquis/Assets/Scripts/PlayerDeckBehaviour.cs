using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckBehaviour : MonoBehaviour
{
    [SerializeField]
    private CardRepositoryBehaviour _cardRepository;
    private Dictionary<CardType, List<CardBehavior>> _playerCards;
    System.Random random = new System.Random();

    void Start()
    {
        _playerCards = new Dictionary<CardType, List<CardBehavior>>();
        _playerCards.Add(CardType.Torso, new List<CardBehavior>());
        _playerCards.Add(CardType.Arm, new List<CardBehavior>());
        _playerCards.Add(CardType.Leg, new List<CardBehavior>());
    }
    
    public int CardsLeft(CardType type) 
    {
        List<CardBehavior> cardsLeftOfType;
        _playerCards.TryGetValue(type, out cardsLeftOfType);
        return cardsLeftOfType.Count;
    }
    
    public void AddCard(CardBehavior cardBehavior) {
        if (!_cardRepository.GetInPlay(cardBehavior.Properties))
            _cardRepository.SetInPlay(cardBehavior.Properties);
        List<CardBehavior> cardsOfType;
        _playerCards.TryGetValue(cardBehavior.Properties.Type, out cardsOfType);
        cardsOfType.Add(cardBehavior);
    }

    public CardBehavior[] DrawCards(CardType type, int amount) {
        List<CardBehavior> drawn = new List<CardBehavior>();
        List<CardBehavior> cardsOfType;
        _playerCards.TryGetValue(type, out cardsOfType);
        for (int i = 0; i < amount; i++)
        {
            CardBehavior drawnCard = cardsOfType[random.Next(cardsOfType.Count)];
            cardsOfType.Remove(drawnCard);
            drawn.Add(drawnCard);
        }
        return drawn.ToArray();
    }
}
