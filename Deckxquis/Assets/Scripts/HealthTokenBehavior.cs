using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTokenBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject _visual = null;

    [SerializeField]
    SpriteRenderer _spriteRenderer = null;

    public void SetVisibility(bool visible)
    {
        if (!_visual) return;
        _visual.SetActive(visible);
    }

    public void SetAvailablility(bool available)
    {
        if(!_spriteRenderer) return;
        _spriteRenderer.enabled = !available;
    }
}
