using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCustomization : MonoBehaviour
{
    [SerializeField] public StartingCustomization startingCustomization;
    [SerializeField] public PlayerCustomization playerCustomization;
    [SerializeField] public AutomaticChainCustomization automaticChain;

    [System.Serializable]
    public class StartingCustomization
    {
        [SerializeField] private int minimumCombos = 3;
        [SerializeField] private int maximumCombos = 5;
        private int startingCombos = 3;

        public int StartingCombos
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
    }

    [System.Serializable]
    public class PlayerCustomization
    {
        [SerializeField] private PlayerChainMode playerChainMode = PlayerChainMode.HORIZONTAL;
        [SerializeField] private int comboAmount = 3;

        public PlayerChainMode PlayerChainMode
        {
            get { return playerChainMode; }
        }
        public int ComboAmount
        {
            get { return comboAmount; }
        }
    }

    [System.Serializable]
    public class AutomaticChainCustomization
    {
        [SerializeField] private AutomaticChainMode automaticChainMode = AutomaticChainMode.HORIZONTAL;
        [SerializeField] private int comboAmount = 3;

        public AutomaticChainMode AutomaticChainMode
        {
            get { return automaticChainMode; }
        }
        public int ComboAmount
        {
            get { return comboAmount; }
        }
    }

    [System.Serializable]
    public class RegenerateCustomization
    {
        [SerializeField] private RegenerateMode regenerateMode = RegenerateMode.EVERYTHING;
        public RegenerateMode RegenerateMode
        {
            get { return regenerateMode; }
        }
    }

    public enum PlayerChainMode
    {
        DIAGONAL,
        HORIZONTAL
    }
    public enum AutomaticChainMode
    {
        DIAGONAL,
        HORIZONTAL
    }
    public enum RegenerateMode
    {
        EVERYTHING,
        FALLING
    }
}
