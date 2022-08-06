using System.Collections.Generic;
using System;
using UnityEngine;

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

    public void GiveTurn(string id)
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
                            // TODO: ENEMYCONTROLLER: delete all current enemies
                            // TODO: ENEMYCONTROLLER: draw new player card
                            // TODO: ENEMYCONTROLLER: draw new enemy cards
                        }
                    }
                }
                return;
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
