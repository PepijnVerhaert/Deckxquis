using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckBehaviour : MonoBehaviour
{
    [SerializeField]
    private CardRepositoryBehaviour _cardRepository;
    private CardProperties[] _enemyCardProperties;
    private int _lastEnemyIndex = 0;
    public int CardsLeft { get => _enemyCardProperties.Length - _lastEnemyIndex; }
    
    void Start()
    {
        _enemyCardProperties = _cardRepository.GetAllCardsInOrder(CardType.Enemy);
    }
    
    public CardProperties[] DrawCards(int amount) {
        List<CardProperties> drawn = new List<CardProperties>();
        for (int i = 0; i < amount; i++)
        {
            int nextIndex = _lastEnemyIndex + i;
            if (nextIndex == _enemyCardProperties.Length) break;
            drawn.Add(_enemyCardProperties[nextIndex]);
        }
        return drawn.ToArray();
    }
}
