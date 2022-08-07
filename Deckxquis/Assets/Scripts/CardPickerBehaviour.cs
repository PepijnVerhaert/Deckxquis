using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
    public enum PickContext {
        NewCard,
        CardFromDeck,
        None,
    };

public class CardPickerBehaviour : MonoBehaviour
{
    private PickContext _context;
    private CardProperties[] _drawnCards;
    
    [SerializeField] CardBehavior _cardA;
    [SerializeField] CardBehavior _cardB;
    [SerializeField] CardBehavior _cardC;
    [SerializeField] CardRepositoryBehaviour _cardRepository;
    [SerializeField] PlayerDeckBehaviour _playerDeck;
    [SerializeField] PlayerBehaviour _player;
    [SerializeField] GameMangerBehavior _gameManager;

    private List<(CardType, PickContext)> _queue = new List<(CardType, PickContext)>();
    private bool _isPicking;

    public bool IsPicking { get => _isPicking; }

    private bool _newWave = false;

    public void Update()
    {
        if (_queue.Count > 0 && !_isPicking)
        {
            _isPicking = true;
            var pick = _queue.First();
            _queue.RemoveAt(0);

            switch (pick.Item2)
            {
                case PickContext.NewCard:
                    PickCardFromRepository(pick.Item1);
                    break;
                case PickContext.CardFromDeck:
                    PickCardFromDeck(pick.Item1);
                    break;
                default:
                    break;
            }
        }
    }

    private void ClearCards()
    {
        _cardA.Show(CardSide.None);        
        _cardB.Show(CardSide.None);        
        _cardC.Show(CardSide.None);        
    }
    
    private void SetCards(CardProperties[] cardProperties) 
    {
        CardBehavior[] cardBehaviors = {_cardA, _cardB, _cardC};
        for (int i = 0; i < cardProperties.Length; i++)
        {
            cardBehaviors[i].SetProperties(cardProperties[i]);
            cardBehaviors[i].Show(CardSide.Back);
        }
    }
    
    public void PickNewCard(CardType type, PickContext context) 
    {
        _queue.Add((type, context));
    }

    private bool PickCardFromDeck(CardType type) 
    {
        if (_playerDeck.CardsLeft(type) <= 0)
        {
            return false;
        }
        ClearCards();
        _drawnCards = _playerDeck.DrawCards(type, 3);
        SetCards(_drawnCards);
        _context = PickContext.CardFromDeck;
        _gameManager.SetInputState(InputState.CardSelect);
        return true;
    }
    
    private bool PickCardFromRepository(CardType type) 
    {
        ClearCards();
        var drawnCards = _cardRepository.GetCards(type, 3);
        SetCards(drawnCards);
        _context = PickContext.NewCard;
        _gameManager.SetInputState(InputState.NewCardSelect);
        return true;
    }
    
    public IEnumerator handleCardPick(CardBehavior cardBehavior) {
        CardProperties cardProperties = cardBehavior.Properties;

        int queueCount = _queue.Count;

        switch (_context)
        {
            case PickContext.NewCard:
                _playerDeck.AddCard(cardProperties);
                _cardRepository.SetInPlay(cardProperties);
                _context = PickContext.None;
                _newWave = true;
                break;
            case PickContext.CardFromDeck:
                cardBehavior.Show(CardSide.Front);
                yield return new WaitForSeconds(1f);
                
                foreach (CardProperties cardProperty in _drawnCards)
                {
                    if (cardProperty.Id == cardProperties.Id) 
                        _player.AddBodyPart(cardProperties);
                    else 
                        // return not picked cards to deck
                        _playerDeck.AddCard(cardProperty);
                }
                _context = PickContext.None;
                break;
        }

        ClearCards();
        _isPicking = false;

        if (_newWave && queueCount == 0)
        {
            _gameManager.PickedCardFromRepository();
            _newWave = false;
        }
    }
}
