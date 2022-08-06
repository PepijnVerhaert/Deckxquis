using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTrackerBehaviour : MonoBehaviour
{
    private int _defaultBaseEnergyLevel;
    private int _maxExtraEnergyLevel;
    private int _baseEnergyLevel;
    private int _extraEnergyLevel;

    public int EnergyLevel { get => _baseEnergyLevel + _extraEnergyLevel; }

    public void useEnergy(int amount=1) {
        if (_extraEnergyLevel > 0) {
            if (amount > _extraEnergyLevel) {
                _baseEnergyLevel -= amount - _extraEnergyLevel;
                _extraEnergyLevel -= 0;
            } else {
                _extraEnergyLevel -= amount;
            }
        } else {
            _energyLevel -= amount;
        }
    }
    
    public void addEnergy(int amount) {
        _extraEnergyLevel += amount;
        _extraEnergyLevel = Mathf.Min(_extraEnergyLevel, _maxExtraEnergyLevel);
    }
    
    public void resetEnergy() {
        _baseEnergyLevel = _defaultBaseEnergyLevel;
        _extraEnergyLevel = 0;
    }
}
