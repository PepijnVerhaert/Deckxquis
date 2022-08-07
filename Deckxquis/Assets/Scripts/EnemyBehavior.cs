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

    private HealthTrackerBehaviour _healthTrackerBehaviour;
    public CardBehavior CardBehavior
    {
        get { return _cardBehavior; }
    }

    public void SetProperties(CardProperties properties)
    {
        _cardBehavior.SetProperties(properties);
        _currentDefence = 0;
        _currentHealth = _cardBehavior.Uses;
    }

    public int CurrentDefence { get => _currentDefence; }
    public int CurrentHealth { get => _currentHealth; }
    public List<bool> AttackPattern { set => _attackPattern = value; }

    private void Start()
    {
        _cardBehavior = GetComponentInChildren<CardBehavior>();
        _healthTrackerBehaviour = GameObject.Find("HealthTracker").GetComponent<HealthTrackerBehaviour>();
    }

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

    public void DeclareIntent()
    {
        if (_attackPattern.Count != 0)
        {
            if (_attackPattern[_attackCount])
            {
                DeclareAttack();
            }
            else
            {
                DeclareDefence();
            }
        }
    }

    private void DeclareAttack()
    {
        CardIconSlotFiller iconSetter = _cardBehavior.gameObject.GetComponent<CardIconSlotFiller>();
        if (iconSetter == null) return;
        iconSetter.SetCustomCardIcons(0, _cardBehavior.HealthCost, 0, 0, _cardBehavior.Attack, 0);
    }

    private void DeclareDefence()
    {
        CardIconSlotFiller iconSetter = _cardBehavior.gameObject.GetComponent<CardIconSlotFiller>();
        if(iconSetter == null) return;
        iconSetter.SetCustomCardIcons(0, 0, 0, _cardBehavior.Health, 0, _cardBehavior.Defence);
    }

    private int Attack()
    {
        if (!IsAlive()) return 0;
        _currentDefence = 0;
        _currentHealth -= _cardBehavior.HealthCost;
        _healthTrackerBehaviour.takeDamage(_cardBehavior.Attack);
        return _cardBehavior.Attack;
    }

    private void Defend()
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
