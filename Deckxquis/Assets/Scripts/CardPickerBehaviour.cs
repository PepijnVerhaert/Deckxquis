using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CardPickerBehaviour : MonoBehaviour
{
    private enum PickContext {
        NewCard,
        CardFromDeck,
        None,
    };
    
    private PickContext _context;
    private CardProperties[] _drawnCards;
    
    [SerializeField] CardBehavior _cardA;
    [SerializeField] CardBehavior _cardB;
    [SerializeField] CardBehavior _cardC;
    [SerializeField] CardRepositoryBehaviour _cardRepository;
    [SerializeField] PlayerDeckBehaviour _playerDeck;
    [SerializeField] PlayerBehaviour _player;
    [SerializeField] GameMangerBehavior _gameManager;
    
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
    
    public void PickNewCard(CardType type) 
    {
        ClearCards();
        SetCards(_cardRepository.GetCards(type, 3));
        _context = PickContext.NewCard;
        _gameManager.SetInputState(InputState.CardSelect);
    }

    public void PickCardFromDeck(CardType type) 
    {
        ClearCards();
        _drawnCards = _playerDeck.DrawCards(type, 3);
        SetCards(_drawnCards);
        _context = PickContext.CardFromDeck;
        _gameManager.SetInputState(InputState.CardSelect);
    }
    
    public void handleCardPick(CardProperties cardProperties) {
        switch (_context)
        {
            case PickContext.NewCard:
                _player.AddBodyPart(cardProperties);
                _cardRepository.SetInPlay(cardProperties);
                _context = PickContext.None;
                break;
            case PickContext.CardFromDeck:
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
    }
}
