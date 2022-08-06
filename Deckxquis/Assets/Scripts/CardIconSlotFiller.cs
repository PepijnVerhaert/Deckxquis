using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIconSlotFiller : MonoBehaviour
{
    [SerializeField]
    private GameObject _frontSlots;

    [SerializeField]
    private SpriteRenderer[] _frontCostSlots;

    [SerializeField]
    private SpriteRenderer[] _frontGainSlots;

    [SerializeField]
    private SpriteRenderer[] _backCostSlots;

    [SerializeField]
    private SpriteRenderer[] _backGainSlots;

    [SerializeField]
    private SpriteRenderer[] _backSpeedSlots;

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
        Vector3 position = _frontSlots.transform.position;
        if (topSlots)
        {
            position.y = 3;
        }
        else
        {
            position.y = -3;
        }
        _frontSlots.transform.position = position;
    }

    public void SetSlotsCorrect()
    {
        CardBehavior cardBehavior = GetComponent<CardBehavior>();
        if (cardBehavior == null) return;

        if (cardBehavior.GetCardType == CardType.Head)
        {
            _frontSlots.SetActive(false);
        }
        else
        {
            _frontSlots.SetActive(true);
            SetFrontSlots(cardBehavior);
        }

        SetBackSlots(cardBehavior);
    }

    private void SetFrontSlots(CardBehavior cardBehavior)
    {
        //set cost
        int healthCost = cardBehavior.HealthCost;
        int energyCost = cardBehavior.EnergyCost;
        for (int i = 0; i < _frontCostSlots.Length; i++)
        {
            if (healthCost > 0)
            {
                _frontCostSlots[i].enabled = true;
                _frontCostSlots[i].sprite = _healthSprite;
                --healthCost;
            }
            else if (energyCost > 0)
            {
                _frontCostSlots[i].enabled = true;
                _frontCostSlots[i].sprite = _energySprite;
                --energyCost;
            }
            else
            {
                _frontCostSlots[i].enabled = false;
            }
        }

        //set gain
        int attack = cardBehavior.Attack;
        int defenceGain = cardBehavior.Defence;
        int healthGain = cardBehavior.Health;
        int energyGain = cardBehavior.Energy;
        for (int i = 0; i < _frontGainSlots.Length; i++)
        {
            if (attack > 0)
            {
                _frontGainSlots[i].enabled = true;
                _frontGainSlots[i].sprite = _attackSprite;
                --attack;
            }
            else if (defenceGain > 0)
            {
                _frontGainSlots[i].enabled = true;
                _frontGainSlots[i].sprite = _defenceSprite;
                --defenceGain;
            }
            else if (healthGain > 0)
            {
                _frontGainSlots[i].enabled = true;
                _frontGainSlots[i].sprite = _healthSprite;
                --healthGain;
            }
            else if (energyGain > 0)
            {
                _frontGainSlots[i].enabled = true;
                _frontGainSlots[i].sprite = _energySprite;
                --energyGain;
            }
            else
            {
                _frontGainSlots[i].enabled = false;
            }
        }
    }

    private void SetBackSlots(CardBehavior cardBehavior)
    {
        //set cost
        int healthCost = cardBehavior.HealthCost;
        int energyCost = cardBehavior.EnergyCost;
        for (int i = 0; i < _backCostSlots.Length; i++)
        {
            if (healthCost > 0)
            {
                _backCostSlots[i].enabled = true;
                _backCostSlots[i].sprite = _healthSprite;
                --healthCost;
            }
            else if (energyCost > 0)
            {
                _backCostSlots[i].enabled = true;
                _backCostSlots[i].sprite = _energySprite;
                --energyCost;
            }
            else
            {
                _backCostSlots[i].enabled = false;
            }
        }

        //set gain
        int attack = cardBehavior.Attack;
        int defenceGain = cardBehavior.Defence;
        int healthGain = cardBehavior.Health;
        int energyGain = cardBehavior.Energy;
        for (int i = 0; i < _backGainSlots.Length; i++)
        {
            if (attack > 0)
            {
                _backGainSlots[i].enabled = true;
                _backGainSlots[i].sprite = _attackSprite;
                --attack;
            }
            else if (defenceGain > 0)
            {
                _backGainSlots[i].enabled = true;
                _backGainSlots[i].sprite = _defenceSprite;
                --defenceGain;
            }
            else if (healthGain > 0)
            {
                _backGainSlots[i].enabled = true;
                _backGainSlots[i].sprite = _healthSprite;
                --healthGain;
            }
            else if (energyGain > 0)
            {
                _backGainSlots[i].enabled = true;
                _backGainSlots[i].sprite = _energySprite;
                --energyGain;
            }
            else
            {
                _backGainSlots[i].enabled = false;
            }
        }

        int speed = cardBehavior.Speed;
        for (int i = 0; i < _backSpeedSlots.Length; i++)
        {
            if (speed > 0)
            {
                _backSpeedSlots[i].enabled = true;
                --speed;
            }
            else
            {
                _backSpeedSlots[i].enabled = false;
            }
        }
    }

}
