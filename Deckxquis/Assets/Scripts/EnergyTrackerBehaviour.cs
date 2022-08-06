using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTrackerBehaviour : MonoBehaviour
{
    public int _energyLevel { get; }
    
    public void decrementEnergyLevel(int amount=1) {
        _energyLevel -= amount;
    }
}
