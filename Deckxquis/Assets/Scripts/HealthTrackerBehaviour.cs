using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTrackerBehaviour : MonoBehaviour
{
    public int _maxHealthLevel;
    public int _maxDefenceLevel; 
    private int _healthLevel;
    private int _defenceLevel;
    
    public int HealthLevel { get => _healthLevel; }
    public int DefenceLevel { get => _defenceLevel; }
    public int MaxHealthLevel { set => _maxHealthLevel = value; }
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
    }
    
    public void heal(int amount) {
        _healthLevel += amount;
        _healthLevel = Mathf.Min(_healthLevel, _maxHealthLevel);
    }
    
    public void increaseDefence(int amount) {
        _defenceLevel += amount;
        _defenceLevel = Mathf.Min(_defenceLevel, _maxDefenceLevel);
    }
}
