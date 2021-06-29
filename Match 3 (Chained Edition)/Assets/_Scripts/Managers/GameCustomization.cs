using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCustomization : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private ComboMode comboMode;
    public enum ComboMode
    {
        DIAGONAL,
        HORIZONTAL
    }

    public ComboMode GetComboMode()
    { return comboMode; }
}
