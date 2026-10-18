using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEVIew : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Direction, Sprite> arrows;
    [SerializeField]
    private Image arrowImage;
    [SerializeField]
    private PlayerQTEController controller;

    private void Start()
    {
        controller.OnArrowDirectionChanged += SwitchToNextDirection;
    }

    private void OnDestroy()
    {
        controller.OnArrowDirectionChanged -= SwitchToNextDirection;
    }

    private void SwitchToNextDirection(Direction direction)
    {
        arrowImage.sprite = arrows[direction];
    }
}
