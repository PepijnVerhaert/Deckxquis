using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    
    public int Speed { get => calculateSpeed(); }
    
    private int calculateSpeed() {
        return 2;
    }
    
    public void Update() {
        
    }
}
