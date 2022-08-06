using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIconSlotFiller : MonoBehaviour
{
    [SerializeField]
    private GameObject _slots;

    [SerializeField]
    private SpriteRenderer[] _costSlots;

    [SerializeField]
    private SpriteRenderer[] _gainSlots;

    [SerializeField]
    private Sprite _attackSprite;

    [SerializeField]
    private Sprite _energySprite;

    [SerializeField]
    private Sprite _healthSprite;

    [SerializeField]
    private Sprite _defenceSprite;

    public void SetSlotsToCardTop(bool topSlots)
    {
        Vector3 position = _slots.transform.position;
        if (topSlots)
        {
            position.y = 3;
        }
        else
        {
            position.y = -3;
        }
        _slots.transform.position = position;
    }

    public void SetSlotsCorrect()
    {
        CardBehavior cardBehavior = GetComponent<CardBehavior>();
        if (cardBehavior == null) return;

        //set cost
        int healthCost = cardBehavior.HealthCost;
        int energyCost = cardBehavior.EnergyCost;
        for (int i = 0; i < _costSlots.Length; i++)
        {
            if (healthCost > 0)
            {
                _costSlots[i].enabled = true;
                _costSlots[i].sprite = _healthSprite;
                --healthCost;
            } 
            else if(energyCost > 0)
            {
                _costSlots[i].enabled = true;
                _costSlots[i].sprite = _energySprite;
                --energyCost;
            }
            else
            {
                _costSlots[i].enabled = false;
            }
        }

        //set gain
        int attack = cardBehavior.Attack;
        int defenceGain = cardBehavior.Defence;
        int healthGain = cardBehavior.Health;
        int energyGain = cardBehavior.Energy;
        for (int i = 0; i < _gainSlots.Length; i++)
        {
            if(attack > 0)
            {
                _gainSlots[i].enabled = true;
                _gainSlots[i].sprite = _attackSprite;
                --attack;
            }
            else if(defenceGain > 0)
            {
                _gainSlots[i].enabled = true;
                _gainSlots[i].sprite = _defenceSprite;
                --defenceGain;
            }
            else if (healthGain > 0)
            {
                _gainSlots[i].enabled = true;
                _gainSlots[i].sprite = _healthSprite;
                --healthGain;
            }
            else if(energyGain > 0)
            {
                _gainSlots[i].enabled = true;
                _gainSlots[i].sprite = _energySprite;
                --energyGain;
            }
            else
            {
                _gainSlots[i].enabled = false;
            }
        }
    }
}
