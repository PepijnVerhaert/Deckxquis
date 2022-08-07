using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTrackerBehaviour : MonoBehaviour
{
    private int _maxBaseEnergyLevel;
    private int _maxExtraEnergyLevel = 10;
    private int _baseEnergyLevel;
    private int _extraEnergyLevel;

    [SerializeField]
    TokenBehavior[] _baseEnergyTokenBehaviors;

    [SerializeField]
    TokenBehavior[] _extraEnergyTokenBehaviors;

    public int EnergyLevel { get => _baseEnergyLevel + _extraEnergyLevel; }

    public int MaxBaseEnergyLevel
    {
        set
        {
            _maxBaseEnergyLevel = value;
            SetBaseTokenVisibility();
        }
    }


    public void useEnergy(int amount=1) {
        if (_extraEnergyLevel > 0) {
            if (amount > _extraEnergyLevel) {
                _baseEnergyLevel -= amount - _extraEnergyLevel;
                _extraEnergyLevel -= 0;
            } else {
                _extraEnergyLevel -= amount;
            }
        } else {
            _baseEnergyLevel -= amount;
        }
        SetExtraTokenVisibility();
        SetBaseTokenAvailability();
    }

    public void addEnergy(int amount) {
        _extraEnergyLevel += amount;
        _extraEnergyLevel = Mathf.Min(_extraEnergyLevel, _maxExtraEnergyLevel);
        SetExtraTokenVisibility();
    }

    public void resetEnergy() {
        _baseEnergyLevel = _maxBaseEnergyLevel;
        _extraEnergyLevel = 0;
        SetExtraTokenVisibility();
        SetBaseTokenAvailability();
    }

    private void SetBaseTokenVisibility()
    {
        for (int i = 0; i < _baseEnergyTokenBehaviors.Length; i++)
        {
            bool visible = false;
            if (i < _maxBaseEnergyLevel)
                visible = true;
            _baseEnergyTokenBehaviors[i].SetVisibility(visible);
        }
    }

    private void SetExtraTokenVisibility()
    {
        for (int i = 0; i < _extraEnergyTokenBehaviors.Length; i++)
        {
            bool visible = false;
            if (i < _extraEnergyLevel)
                visible = true;
            _extraEnergyTokenBehaviors[i].SetVisibility(visible);
        }
    }

    private void SetBaseTokenAvailability()
    {
        for (int i = 0; i < _baseEnergyTokenBehaviors.Length; i++)
        {
            bool available = false;
            if (i < _baseEnergyLevel)
                available = true;
            _baseEnergyTokenBehaviors[i].SetAvailablility(available);
        }
    }

}
