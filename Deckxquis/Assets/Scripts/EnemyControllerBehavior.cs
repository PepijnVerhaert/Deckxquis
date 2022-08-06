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
    private List<EnemyBehavior> _behaviors = new List<EnemyBehavior>();

    private Dictionary<string, int> _enemySpeed = new Dictionary<string, int>();

    public Dictionary<string, int> EnemySpeed { get => _enemySpeed; }

    private TurnTrackerBehavior _turnTrackerBehavior;

    [SerializeField] private float _enemyTurnTime = 2f;

    private void Start()
    {
        _turnTrackerBehavior = GameObject.Find("TurnTracker").GetComponent<TurnTrackerBehavior>();
    }

    public void AddEnemy(EnemyBehavior behavior)
    {
        foreach (var pattern in _enemyPattern)
        {
            if (pattern._name == behavior.name)
            {
                behavior.AttackPattern = pattern._boolList;
                break;
            }
        }
        _behaviors.Add(behavior);
        _enemySpeed.Add(behavior.CardBehavior.Id, behavior.CardBehavior.Speed);
    }

    public Dictionary<string, int> GetEnemySpeeds()
    {
        return _enemySpeed;
    }

    public IEnumerator GiveTurn(string id)
    {
        foreach (var behavior in _behaviors)
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
                        // TODO: ENEMYCONTROLLER: take out of combat tracker
                        if (AreAllEnemiesDead())
                        {
                            _turnTrackerBehavior.EnemyDied(id);
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
        foreach (var behavior in _behaviors)
        {
            if (behavior.IsAlive())
            {
                return false;
            }
        }
        return true;
    }
}
