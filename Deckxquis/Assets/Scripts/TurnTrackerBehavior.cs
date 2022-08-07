using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnTrackerBehavior : MonoBehaviour
{
    private Dictionary<string, float> _speedPerTurn = new Dictionary<string, float>();
    private Dictionary<string, float> _accumulatedSpeed = new Dictionary<string, float>();
    private List<string> _turnList = new List<string>();
    private int _imageAmount = 8;
    private string _playerId;

    private PlayerBehaviour _playerBehaviour;
    private EnemyControllerBehavior _enemyBehaviour;
    private GameMangerBehavior _gameMangerBehavior;
    private CardRepositoryBehaviour _cardRepositoryBehaviour;
    private CardBehavior[] _cards = new CardBehavior[8];

    // REMOVE ENEMIES
    private string deadEnemy;
    private bool IsMatchingEnemy(string s)
    {
        return s == deadEnemy;
    }

    private void Start()
    {
        // Gather the data sources
        _playerBehaviour = GameObject.Find("PlayerCharacter").GetComponent<PlayerBehaviour>();
        _enemyBehaviour = GameObject.Find("EnemyCharacter").GetComponent<EnemyControllerBehavior>();
        _gameMangerBehavior = GameObject.Find("GameManager").GetComponent<GameMangerBehavior>();
        _cardRepositoryBehaviour = GameObject.Find("CardRepository").GetComponent<CardRepositoryBehaviour>();
        // Gather the visuals
        GameObject turns = gameObject.transform.GetChild(0).gameObject;
        for (int i = 0; i < 8; i++)
            _cards[i] = turns.transform.GetChild(i).GetComponent<CardBehavior>();
    }

    public void Update()
    {
        // Hide each card
        foreach (CardBehavior card in _cards)
        {
            card.Show(CardSide.Back);
        }
        // Set the correct visual for each turn we have
        for (int i = 0; i < _turnList.Count; i++)
        {
            CardProperties fullProperties = _cardRepositoryBehaviour.GetCardById(_turnList[i]);
            _cards[i].SetProperties(fullProperties);
            _cards[i].Show(CardSide.Front);
        }
    }

    public void StartCombat()
    {
        StartTurn();
    }

    public void SetPlayer(string playerId, int speed)
    {
        _playerId = playerId;
        _speedPerTurn.Add(playerId, speed);
        _accumulatedSpeed.Add(playerId, 50);
    }

    public void AddEnemy(string id, int speed)
    {
        _speedPerTurn.Add(id, speed);
        _accumulatedSpeed.Add(id, 0);
    }

    public void FillTurnList()
    {
        while (_speedPerTurn.Count != 0 && _turnList.Count < _imageAmount)
        {
            var descendingAccSpeedVec = _accumulatedSpeed.OrderByDescending(pair => pair.Value);

            foreach (var accSpeed in descendingAccSpeedVec)
            {
                if (_accumulatedSpeed[accSpeed.Key] >= 100)
                {
                    _accumulatedSpeed[accSpeed.Key] -= 100;
                    _turnList.Add(accSpeed.Key);
                }
                else
                {
                    break;
                }

                if (_turnList.Count == _imageAmount)
                {
                    break;
                }
            }

            if (_turnList.Count == _imageAmount)
            {
                break;
            }

            foreach (var turn in _speedPerTurn)
            {
                _accumulatedSpeed[turn.Key] += _speedPerTurn[turn.Key];
            }
        }
    }

    public void RecalculateTurnList()
    {
        // Reset turn list
        foreach (var turn in _speedPerTurn)
        {
            _accumulatedSpeed[turn.Key] = 0;
        }
        _turnList.Clear();

        _accumulatedSpeed[_playerId] = 100;
        FillTurnList();
    }

    public void EnemyDied(string id)
    {
        deadEnemy = id;
        _turnList.RemoveAll(IsMatchingEnemy);

        FillTurnList();
    }

    public void EndTurn()
    {
        _turnList.RemoveAt(0);
        StartTurn();
    }

    private void StartTurn()
    {
        FillTurnList();

        if (_turnList[0] == _playerId)
        {
            _gameMangerBehavior.SetInputState(InputState.PlayerSelect);
        }
        else
        {
            _gameMangerBehavior.SetInputState(InputState.EnemyTurn);
            StartCoroutine(_enemyBehaviour.GiveTurn(_turnList[0]));
        }
    }
}
