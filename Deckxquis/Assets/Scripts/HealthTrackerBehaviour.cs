using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTrackerBehaviour : MonoBehaviour
{
    public int _maxHealthLevel;
    public int _maxDefenceLevel = 10; 
    private int _healthLevel;
    private int _defenceLevel;

    [SerializeField]
    TokenBehavior[] _healthTokenBehaviors;

    [SerializeField]
    TokenBehavior[] _defenceTokenBehaviors;

    public int HealthLevel { get => _healthLevel; }
    public int DefenceLevel { get => _defenceLevel; }
    public int MaxHealthLevel 
    {
        set
        {
            _maxHealthLevel = value;
            _healthLevel = value;
            SetHealthTokenVisibility();
        }
    }
    public int MaxDefenceLevel { set => _maxDefenceLevel = value; }

    public void takeDamage(int amount) {
        if (_defenceLevel > 0) {
            if (amount > _defenceLevel) {
                _healthLevel -= amount - _defenceLevel;
                _defenceLevel = 0;
            } else {
                _defenceLevel -= amount;
            }
        } else {
            _healthLevel -= amount;
        }
        SetHealthTokenAvailability();
        SetDefenceTokenVisibility();
    }
    
    public void reduceHealth(int amount) {
        _healthLevel -= amount;
        SetHealthTokenAvailability();
    }
    
    public void heal(int amount) {
        _healthLevel += amount;
        _healthLevel = Mathf.Min(_healthLevel, _maxHealthLevel);
        SetHealthTokenAvailability();
    }
    
    public void increaseDefence(int amount) {
        _defenceLevel += amount;
        _defenceLevel = Mathf.Min(_defenceLevel, _maxDefenceLevel);
        SetDefenceTokenVisibility();
    }

    public void removeDefence()
    {
        _defenceLevel = 0;
        SetDefenceTokenVisibility();
    }

    private void SetHealthTokenVisibility()
    {
        for (int i = 0; i < _healthTokenBehaviors.Length; i++)
        {
            bool visible = false;
            if (i < _maxHealthLevel)
                visible = true;
            _healthTokenBehaviors[i].SetVisibility(visible);
        }
    }

    private void SetDefenceTokenVisibility()
    {
        for (int i = 0; i < _defenceTokenBehaviors.Length; i++)
        {
            bool visible = false;
            if (i < _defenceLevel)
                visible = true;
            _defenceTokenBehaviors[i].SetVisibility(visible);
        }
    }

    private void SetHealthTokenAvailability()
    {
        for (int i = 0; i < _healthTokenBehaviors.Length; i++)
        {
            bool available = false;
            if (i < _healthLevel)
                available = true;
            _healthTokenBehaviors[i].SetAvailablility(available);
        }
    }
}
