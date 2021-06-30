using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCustomization : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private PlayerComboMode playerComboMode = PlayerComboMode.HORIZONTAL;
    [SerializeField] private ChainComboMode chainComboMode = ChainComboMode.HORIZONTAL;
    [SerializeField] private UpdatingMode updatingMode = UpdatingMode.UPDATE_USED;
    [SerializeField] private int comboAmount = 3;
    [SerializeField] private int startingCombos;
    [SerializeField] private int minimumCombos = 3;
    [SerializeField] private int maximumCombos = 5;
    public enum PlayerComboMode
    {
        DIAGONAL,
        HORIZONTAL
    }
    public enum UpdatingMode
    {
        UPDATE_EVERYTING,
        UPDATE_USED
    }
    public enum ChainComboMode
    {
        DIAGONAL,
        HORIZONTAL
    }

    public PlayerComboMode GetComboMode()
    { return playerComboMode; }
    public UpdatingMode GetUpdatingMode()
    { return updatingMode; }
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
