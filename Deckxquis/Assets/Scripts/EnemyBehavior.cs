using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    Text _healthAmount;

    [SerializeField]
    Text _defenceAmount;

    [SerializeField]
    Text _name;

    private CardBehavior _cardBehavior;

    private List<bool> _attackPattern;
    private int _attackCount;

    private int _currentDefence;
    private int _currentHealth;
    private int _lastFrameHealth;
    private bool _dead;

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

    private void Update()
    {
        if(_lastFrameHealth > 0 && _currentHealth <= 0 && !_dead)
        {
            _name.text = string.Empty;
            _healthAmount.text = string.Empty;
            _defenceAmount.text = string.Empty;
        }
        _lastFrameHealth = _currentHealth;

        if(_currentHealth <= 0)
        {
            _dead = true;
        }
        else
        {
            _dead = false;
        }

        if(!_dead)
        {
            _name.text = _cardBehavior.Name;
            _healthAmount.text = _currentHealth.ToString();
            _defenceAmount.text = _currentDefence.ToString();
        }
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
