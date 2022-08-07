using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnTrackerBehavior : MonoBehaviour
{
    private Dictionary<string, float> _speedPerTurn = new Dictionary<string, float>();
    private Dictionary<string, float> _accumulatedSpeed = new Dictionary<string, float>();
    private List<string> _imageList = new List<string>();
    private int _imageAmount = 8;

    private string _playerId;

    private PlayerBehaviour _playerBehaviour;
    private EnemyControllerBehavior _enemyBehaviour;
    private GameMangerBehavior _gameMangerBehavior;

    // REMOVE ENEMIES
    private string deadEnemy;
    private bool IsMatchingEnemy(string s)
    {
        return s == deadEnemy;
    }

    private void Start()
    {
        _playerBehaviour = GameObject.Find("PlayerCharacter").GetComponent<PlayerBehaviour>();
        _enemyBehaviour = GameObject.Find("EnemyCard").GetComponent<EnemyControllerBehavior>();
        _gameMangerBehavior = GameObject.Find("GameManager").GetComponent<GameMangerBehavior>();
    }

    public void StartCombat()
    {
        StartTurn();
    }

    public void SetPlayer(string playerId, int speed)
    {
        _playerId = playerId;
        _speedPerTurn.Add(playerId, speed);
        _accumulatedSpeed.Add(playerId, 100);
    }

    public void FillTurnList()
    {
        while (_speedPerTurn.Count != 0 && _imageList.Count < _imageAmount)
        {
            var descendingAccSpeedVec = _accumulatedSpeed.OrderByDescending(pair => pair.Value);

            foreach (var accSpeed in descendingAccSpeedVec)
            {
                if (_accumulatedSpeed[accSpeed.Key] >= 100)
                {
                    _accumulatedSpeed[accSpeed.Key] -= 100;
                    _imageList.Add(accSpeed.Key);
                }
                else
                {
                    break;
                }

                if (_imageList.Count == _imageAmount)
                {
                    break;
                }
            }

            if (_imageList.Count == _imageAmount)
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
        _imageList.Clear();

        _accumulatedSpeed[_playerId] = 100;
        FillTurnList();
    }

    public void EnemyDied(string id)
    {
        deadEnemy = id;
        _imageList.RemoveAll(IsMatchingEnemy);

        FillTurnList();
    }

    public void EndTurn()
    {
        _imageList.RemoveAt(0);
        StartTurn();
    }

    private void StartTurn()
    {
        FillTurnList();

        if (_imageList.Count == 0)
        {
            return;
        }

        if (_imageList[0] == _playerId)
        {
            _gameMangerBehavior.SetInputState(InputState.PlayerSelect);
        }
        else
        {
            _gameMangerBehavior.SetInputState(InputState.EnemyTurn);
            StartCoroutine(_enemyBehaviour.GiveTurn(_imageList[0]));
        }
    }
}
