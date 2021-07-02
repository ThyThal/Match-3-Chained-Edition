using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockView : MonoBehaviour
{
    [SerializeField] private Image artwork;
    public Image Artwork
    {
        get { return artwork; }
        set { artwork = value; }
    }
}
