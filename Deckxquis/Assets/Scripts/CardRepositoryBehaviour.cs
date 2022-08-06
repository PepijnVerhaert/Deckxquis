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
    public string _Type;
    public CardType Type;
    public int Speed;
    public int EnergyCost;
    public int HealthCost;
    public int Energy;
    public int Defence;
    public int Attack;
    public int Uses;
    public int Health;
    public string Id;
    public string toString() {
        return System.String.Format("{0} {1} {2}", Id, Type, Name);
    }
}

[System.Serializable]
public class CardsProperties
{
    public CardProperties[] cardsProperties;
}

public class CardRepositoryBehaviour : MonoBehaviour
{
    private System.Random random = new System.Random();
    private CardProperties[] _cardsProperties;
    private List<string> _inPlay = new List<string>();
    public TextAsset jsonFile;

    private void loadCards() {
        CardsProperties cardsInJson = JsonUtility.FromJson<CardsProperties>(jsonFile.text);
        _cardsProperties = cardsInJson.cardsProperties;
 
        for (int i = 0; i < _cardsProperties.Length; i++)
        {
            _cardsProperties[i].Type = (CardType) System.Enum.Parse(typeof(CardType), _cardsProperties[i]._Type);
            _cardsProperties[i].Id = System.Guid.NewGuid().ToString();
            Debug.Log("Found card: " + _cardsProperties[i].toString());
        }
        Debug.Log("Found "+_cardsProperties.Length+" total cards");
    }
    
    private CardProperties[] GetAvailable(CardType type) {
        List<CardProperties> available = new List<CardProperties>();
        foreach (CardProperties cardProperties in _cardsProperties)
        {
            if (cardProperties.Type == type && !_inPlay.Contains(cardProperties.Id)) 
                available.Add(cardProperties);
        }
        return available.ToArray();
    }
    
    public CardProperties[] GetCards(CardType type, int amount) {
        CardProperties[] available;
        List<CardProperties> picked = new List<CardProperties>();

        for (int i = 0; i < amount; i++)
        {
            available = GetAvailable(type);
            if (available.Length == 0) break;
            CardProperties pickedProperties = available[random.Next(available.Length)];
            picked.Add(pickedProperties);
            _inPlay.Add(pickedProperties.Id);
        }
        
        return picked.ToArray();
    }
    
    public CardProperties[] GetAllCardsInOrder(CardType type) {
        return GetAvailable(type);
    }

    void Start()
    {
        loadCards();
    }
}
