using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockView : MonoBehaviour
{
    [SerializeField] private Image artwork;
    [SerializeField] private ShaderElectricity shaderElectricity;

    public Image Artwork
    {
        get { return artwork; }
        set { artwork = value; }
    }

    public void EnableSelectedMaterial()
    {
        shaderElectricity.isSelected = true;
    }
}
