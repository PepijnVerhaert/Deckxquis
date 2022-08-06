using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTrackerBehaviour : MonoBehaviour
{
    private int _energyLevel;

    public int EnergyLevel { get => _energyLevel; }

    public void useEnergy(int amount=1) {
        _energyLevel -= amount;
    }
}
