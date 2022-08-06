using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private CardBehavior _cardBehavior;

    private List<bool> _attackPattern;
    private int _attackCount;

    private int _currentDefence;
    private int _currentHealth;

    public CardBehavior CardBehavior
    {
        get { return _cardBehavior; }
        set 
        {
            _cardBehavior = value;
            _currentDefence = 0;
            _currentHealth = _cardBehavior.Uses;
        }
    }

    public int CurrentDefence { get => _currentDefence; }
    public int CurrentHealth { get => _currentHealth; }
    public List<bool> AttackPattern { set => _attackPattern = value; }

    public void PlayTurn()
    {
        if (_attackPattern.Count != 0)
        {
            if (_attackPattern[_attackCount])
            {
                Attack();
            }
            else
            {
                Defend();
            }

            ++_attackCount;
            _attackCount %= _attackPattern.Count;
        }
    }

    public int Attack()
    {
        if (!IsAlive()) return 0;
        _currentDefence = 0;
        _currentHealth -= _cardBehavior.HealthCost;
        return _cardBehavior.Attack;
    }

    public void Defend()
    {
        if (!IsAlive()) return;
        _currentDefence = _cardBehavior.Defence;
        _currentHealth += _cardBehavior.Health;
    }

    public void ChangeHealth(int amount)
    {
        if(amount > 0)
        {
            //heal
            _currentHealth += amount;
            if(_currentHealth > _cardBehavior.Uses)
            {
                _currentHealth = _cardBehavior.Uses;
            }
        }
        else if(amount < 0)
        {
            //damage
            _currentDefence += amount;
            if(_currentDefence < 0)
            {
                //add negative defence to health
                _currentHealth += _currentDefence;
                _currentDefence = 0;
            }
        }
    }

    public bool IsAlive()
    {
        if(_currentHealth > 0)
        {
            return true;
        }

        return false;
    }
}
