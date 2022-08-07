using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeckBehaviour : MonoBehaviour
{
    [SerializeField]
    private CardRepositoryBehaviour _cardRepository;
    private Dictionary<CardType, List<CardProperties>> _playerCards;
    private Dictionary<CardType, GameObject> _visuals;
    System.Random random = new System.Random();

    [SerializeField]
    private Text _armText;
    [SerializeField]
    private Text _torsoText;
    [SerializeField]
    private Text _legText;

    void Start()
    {
        InitializeDecks();
        InitializeVisuals();
    }

    void Update()
    {
        CardType[] types = { CardType.Arm, CardType.Torso, CardType.Leg };
        GameObject visualOfType;
        int cardsLeft;
        foreach (CardType type in types)
        {
            cardsLeft = CardsLeft(type);
            _visuals.TryGetValue(type, out visualOfType);
            visualOfType.SetActive(cardsLeft > 0);
        }
        _armText.text = CardsLeft(CardType.Arm).ToString();
        _torsoText.text = CardsLeft(CardType.Torso).ToString();
        _legText.text = CardsLeft(CardType.Leg).ToString();
    }

    private void InitializeVisuals()
    {
        _visuals = new Dictionary<CardType, GameObject>();
        GameObject visuals = gameObject.transform.GetChild(0).gameObject;
        _visuals.Add(CardType.Arm, visuals.transform.Find("Arm").gameObject);
        _visuals.Add(CardType.Leg, visuals.transform.Find("Leg").gameObject);
        _visuals.Add(CardType.Torso, visuals.transform.Find("Torso").gameObject);
    }

    private void InitializeDecks()
    {
        _playerCards = new Dictionary<CardType, List<CardProperties>>();
        _playerCards.Add(
            CardType.Head,
            new List<CardProperties>(_cardRepository.GetCards(CardType.Head, 3))
        );
        _playerCards.Add(
            CardType.Torso,
            new List<CardProperties>(_cardRepository.GetCards(CardType.Torso, 3))
        );
        _playerCards.Add(
            CardType.Arm,
            new List<CardProperties>(_cardRepository.GetCards(CardType.Arm, 3))
        );
        _playerCards.Add(
            CardType.Leg,
            new List<CardProperties>(_cardRepository.GetCards(CardType.Leg, 3))
        );

        CardType[] types = { CardType.Arm, CardType.Torso, CardType.Leg };
        List<CardProperties> cardProperties;
        foreach (CardType type in types)
        {
            _playerCards.TryGetValue(type, out cardProperties);
            foreach (var properties in cardProperties)
            {
                _cardRepository.SetInPlay(properties);
            }
        }

    }

    public int CardsLeft(CardType type)
    {
        List<CardProperties> cardsLeftOfType;
        _playerCards.TryGetValue(type, out cardsLeftOfType);
        return cardsLeftOfType.Count;
    }

    public void AddCard(CardProperties cardProperties)
    {
        if (!_cardRepository.GetInPlay(cardProperties))
            _cardRepository.SetInPlay(cardProperties);
        List<CardProperties> cardsOfType;
        _playerCards.TryGetValue(cardProperties.Type, out cardsOfType);
        cardsOfType.Add(cardProperties);
    }

    public CardProperties[] DrawCards(CardType type, int amount)
    {
        List<CardProperties> drawn = new List<CardProperties>();
        List<CardProperties> cardsOfType;
        _playerCards.TryGetValue(type, out cardsOfType);
        for (int i = 0; i < amount; i++)
        {
            if (cardsOfType.Count == 0) break;
            CardProperties drawnCard = cardsOfType[random.Next(cardsOfType.Count - 1)];
            cardsOfType.Remove(drawnCard);
            drawn.Add(drawnCard);
        }
        return drawn.ToArray();
    }
}
