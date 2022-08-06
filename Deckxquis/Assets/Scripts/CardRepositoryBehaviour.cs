using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData 
{
    public string name;
}

[System.Serializable]
public class CardsData
{
    public CardData[] cardsData;
}

public class CardRepositoryBehaviour : MonoBehaviour
{
    
    private CardData[] _cardsData;
    public TextAsset jsonFile;

    private void loadCards() {
        CardsData cardsInJson = JsonUtility.FromJson<CardsData>(jsonFile.text);
        _cardsData = cardsInJson.cardsData;
 
        foreach (CardData cardData in cardsInJson.cardsData)
        {
            Debug.Log("Found card: " + cardData.name);
        }
    }

    void Start()
    {
        loadCards();
    }
    
}
