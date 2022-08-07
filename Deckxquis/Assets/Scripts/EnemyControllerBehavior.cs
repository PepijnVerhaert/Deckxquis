using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PatternList
{
    public string _name;
    public List<bool> _boolList;
}

public class EnemyControllerBehavior : MonoBehaviour
{
    [SerializeField] private List<PatternList> _enemyPattern = new List<PatternList>();

    private Dictionary<string, int> _enemySpeed = new Dictionary<string, int>();

    public Dictionary<string, int> EnemySpeed { get => _enemySpeed; }

    private TurnTrackerBehavior _turnTrackerBehavior;
    private GameMangerBehavior _gameMangerBehavior;
    private EnemyDeckBehaviour _enemyDeckBehaviour;
    private CardPickerBehaviour _cardPicker;
    private CardBehavior[] _cardBehaviors;
    private EnemyBehavior[] _enemyBehaviors;

    [SerializeField] private float _enemyTurnTime = 2f;

    private void Start()
    {
        _turnTrackerBehavior = GameObject.Find("TurnTracker").GetComponent<TurnTrackerBehavior>();
        _enemyDeckBehaviour = GameObject.Find("EnemyDecks").GetComponent<EnemyDeckBehaviour>();
        _cardBehaviors = GetComponentsInChildren<CardBehavior>();
        _enemyBehaviors = GetComponentsInChildren<EnemyBehavior>();
        _gameMangerBehavior = GameObject.Find("GameManager").GetComponent<GameMangerBehavior>();
    }

    public IEnumerator DrawEnemies()
    {
        var enemyCards = _enemyDeckBehaviour.DrawCards(4);
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < _enemyBehaviors.Length; ++i)
        {
            _enemyBehaviors[i].SetProperties(enemyCards[i]);
            _enemyBehaviors[i].CardBehavior.Show(CardSide.Front);

            AddEnemy(_enemyBehaviors[i]);

            _turnTrackerBehavior.AddEnemy(_enemyBehaviors[i].CardBehavior.Id, _enemyBehaviors[i].CardBehavior.Speed);
            yield return new WaitForSeconds(0.5f);
        }

        _gameMangerBehavior.EnemiesReady();
    }

    public void AddEnemy(EnemyBehavior enemyBehavior)
    {
        foreach (var pattern in _enemyPattern)
        {
            if (pattern._name == enemyBehavior.CardBehavior.ImageName)
            {
                enemyBehavior.AttackPattern = pattern._boolList;
                break;
            }
        }

        enemyBehavior.DeclareIntent();
        _enemySpeed.Add(enemyBehavior.CardBehavior.Id, enemyBehavior.CardBehavior.Speed);
    }

    public Dictionary<string, int> GetEnemySpeeds()
    {
        return _enemySpeed;
    }

    public IEnumerator GiveTurn(string id)
    {
        foreach (var behavior in _enemyBehaviors)
        {
            if (behavior.CardBehavior.Id == id)
            {
                if (behavior.IsAlive())
                {
                    behavior.PlayTurn();
                    if (!behavior.IsAlive())
                    {
                        behavior.gameObject.SetActive(false);
                        // TODO: ENEMYCONTROLLER: enemy dies
                        _turnTrackerBehavior.EnemyDied(id);
                       
                        if (AreAllEnemiesDead())
                        {
                            // TODO: ENEMYCONTROLLER: delete all current enemies
                            // TODO: ENEMYCONTROLLER: draw new player card
                            // TODO: ENEMYCONTROLLER: draw new enemy cards
                        }
                    }
                }
                yield return new WaitForSeconds(_enemyTurnTime);
                _turnTrackerBehavior.EndTurn();
                break;
            }
        }
    }

    public bool AreAllEnemiesDead()
    {
        foreach (var behavior in _enemyBehaviors)
        {
            if (behavior.IsAlive())
            {
                return false;
            }
        }
        return true;
    }
}
