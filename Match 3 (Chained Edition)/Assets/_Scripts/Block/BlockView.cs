using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockView : MonoBehaviour
{
    [SerializeField] private Image artwork;
    [SerializeField] private ShaderSelected shaderSelected;
    public Image Artwork
    {
        get { return artwork; }
        set { artwork = value; }
    }

    public void TriggerShaderSelect()
    {
        shaderSelected.triggerSelected = true;
    }

    public void TriggerShaderUnselect()
    {
        shaderSelected.triggerUnselected = true;
    }
}
