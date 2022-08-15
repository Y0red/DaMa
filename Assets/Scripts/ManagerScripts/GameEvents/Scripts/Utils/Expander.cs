using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO:Must be tested to ensure it functions with overflow animated text. Specifically, the "typewriter" effect that causes each letter to be typed individually over a set time.
/// hypothesis: if <see cref="TextMeshProUGUI.preferredHeight"/> accounts for this difference the expander will function as expected
/// </summary>
public class Expander : MonoBehaviour
{
    public Toggle expandToggle;
    public RectTransform toExpandRectT;

    public float duration;

    public bool canExpandHorizontal;
    public bool canExpandVertical;
    
    private Vector2 _condensedSizeDelta;
    private Vector2 _expandedSizeDelta;
    
    public void Initialize(float width, float height)
    {
        _condensedSizeDelta = toExpandRectT.sizeDelta;
        _expandedSizeDelta = _condensedSizeDelta + new Vector2(canExpandHorizontal ? width : 0, canExpandVertical ? height : 0);

        if (expandToggle != null)
        {
            expandToggle.onValueChanged.AddListener(Expand);
        }
    }
    
    public void Expand(bool isExpand)
    {
        toExpandRectT.DOComplete();

       // toExpandRectT.DOSizeDelta(isExpand ? _expandedSizeDelta : _condensedSizeDelta, duration);
    }
}
