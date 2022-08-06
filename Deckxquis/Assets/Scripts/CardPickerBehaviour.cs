using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CardPickerBehaviour : MonoBehaviour
{
    
    [SerializeField] CardBehavior _cardBehaviourA;
    [SerializeField] CardBehavior _cardBehaviourB;
    [SerializeField] CardBehavior _cardBehaviourC;
    [SerializeField] CardRepositoryBehaviour _cardRepository;
    
    private void ClearCards()
    {
        _cardBehaviourA.clearCardValues();        
        _cardBehaviourB.clearCardValues();        
        _cardBehaviourC.clearCardValues();        
    }
    
    private void SetCards(CardProperties[] cardProperties) 
    {
        CardBehavior[] cardBehaviors = {_cardBehaviourA, _cardBehaviourB, _cardBehaviourC};
        for (int i = 0; i < cardBehaviors.Length; i++)
        {
            cardBehaviors[i].setProperties(cardProperties[i]);
            cardBehaviors[i].setCardValues();
        }
    }
    
    public CardBehavior PickNewCard(CardType type) 
    {
        ClearCards();
        SetCards(_cardRepository.GetCards(type, 3));
        // TODO let user pick and return
        return new CardBehavior();
    }

    public CardBehavior PickCardFromDeck(CardType type) 
    {
        ClearCards();
        // TODO
        return new CardBehavior();
    }
}
