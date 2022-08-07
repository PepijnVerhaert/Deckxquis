using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
    private Text[] _names = new Text[8];

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
        GameObject turn;
        for (int i = 0; i < 8; i++)
        {
            turn = turns.transform.GetChild(i).gameObject;
            _cards[i] = turn.transform.GetChild(0).GetComponent<CardBehavior>();
            _names[i] = turn.transform.GetChild(1).GetComponentInChildren<Text>();
        }
    }

    public void Update()
    {
        // Hide each card and text
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].Show(CardSide.None);
            _names[i].text = "";
        }

        if (!ShowTurnTracker()) return;

        for (int i = 0; i < _turnList.Count; i++)
        {
            CardProperties fullProperties = _cardRepositoryBehaviour.GetCardById(_turnList[i]);
            _cards[i].SetProperties(fullProperties);
            _cards[i].Show(CardSide.Front);
            _names[i].text = fullProperties.Name;
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

    private bool ShowTurnTracker()
    {
        if (_turnList.Count <= 1) return false;
        string firstEl = _turnList[0];
        bool isSame = true;
        for (int i = 1; i < _turnList.Count; i++)
            if (_turnList[i] != firstEl) isSame = false;
        return !isSame;
    }

    private void FillTurnList()
    {
        while (_turnList.Count < _imageAmount)
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

        FillTurnList();
    }

    public void EnemyDied(string id)
    {
        deadEnemy = id;
        _turnList.RemoveAll(IsMatchingEnemy);
        _speedPerTurn.Remove(id);
        _accumulatedSpeed.Remove(id);

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

        if (IsPlayerTurn())
        {
            _gameMangerBehavior.SetInputState(InputState.PlayerSelect);
            _playerBehaviour.PlayerReset();
        }
        else
        {
            _gameMangerBehavior.SetInputState(InputState.EnemyTurn);
            StartCoroutine(_enemyBehaviour.GiveTurn(_turnList[0]));
        }
    }

    public bool IsPlayerTurn()
    {
        return _playerId != null && _turnList[0] == _playerId;
    }

    public void AllEnemiesDied()
    {
        _turnList.Clear();
    }
}
