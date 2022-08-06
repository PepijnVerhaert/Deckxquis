using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardType
{
    Head,
    Arm,
    Torso,
    Leg,
    Enemy,
}
 

[System.Serializable]
public class CardProperties
{
    public string Name;
    public string ImageName;
    public CardType Type;
    public int Speed;
    public int EnergyCost;
    public int HealthCost;
    public int Energy;
    public int Defence;
    public int Attack;
    public int Uses;
    public int Health;
}

[System.Serializable]
public class CardsProperties
{
    public CardProperties[] cardsProperties;
}

public class CardRepositoryBehaviour : MonoBehaviour
{
    
    private CardProperties[] _cardsProperties;
    public TextAsset jsonFile;

    private void loadCards() {
        CardsProperties cardsInJson = JsonUtility.FromJson<CardsProperties>(jsonFile.text);
        _cardsProperties = cardsInJson.cardsProperties;
 
        int found = 0;
        foreach (CardProperties cardProperties in _cardsProperties)
        {
            Debug.Log("Found card: " + cardProperties.Name);
            found++;
        }
        Debug.Log("Found "+found+" total cards");
    }

    void Start()
    {
        loadCards();
    }
}
