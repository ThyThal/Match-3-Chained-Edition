using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCustomization : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private ComboMode comboMode = ComboMode.HORIZONTAL;
    [SerializeField] private int comboAmount = 3;
    [SerializeField] private int startingCombos;
    [SerializeField] private int minimumCombos = 3;
    [SerializeField] private int maximumCombos = 5;
    public enum ComboMode
    {
        DIAGONAL,
        HORIZONTAL
    }

    public ComboMode GetComboMode()
    { return comboMode; }
    public int GetStartingCombos
    {
        get
        {
            if (startingCombos <= 0) 
            { 
                startingCombos = Random.Range(minimumCombos, maximumCombos); 
            }

            return startingCombos;
        }
    }
    public int GetComboAmount
    {
        get { return comboAmount; }
    }
}
