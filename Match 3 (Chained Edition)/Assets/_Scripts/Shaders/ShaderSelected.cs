using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderSelected : MonoBehaviour
{
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private float time = 1;
    [SerializeField] private float fade = 1;
    [SerializeField] private float currentFade;
    [SerializeField] private Image image;

    private float originalTimer;
    [SerializeField] public bool fadingSelected;
    [SerializeField] public bool fadingUnselected;
    [SerializeField] private bool isFading;





    private void Start()
    {
        originalTimer = time;
        image.material = new Material(selectedMaterial);
    }

    private void Update()
    {
        if (isFading)
        {
            if (fadingSelected == true && fadingUnselected == false)
            {
                FadeSelect();
            }

            if (fadingUnselected == true && fadingSelected == false)
            {
                FadeUnselect();
            }
        }
    }

    public void StartFade()
    {
        if (isFading == true)
        {
            OverrideFade();
        }

        if (isFading == false)
        {
            isFading = true;
        }
    }

    private void OverrideFade()
    {
        if (fadingSelected == true) // Override Selected Fade
        {
            fadingSelected = false;
            fadingUnselected = true;
        }

        else if (fadingUnselected == true) // Override Unselected Fade
        {
            fadingUnselected = false;
            fadingSelected = true;
        }
    }

    private void FadeSelect()
    {
        time -= Time.deltaTime;
        currentFade = Mathf.Lerp(fade, 0, time);

        if (time <= 0)
        {
            time = originalTimer;
            isFading = false;
            fadingSelected = false;
            fade = 1;
        }

        image.material.SetFloat("_Fade", currentFade);
    }
    private void FadeUnselect()
    {
        time -= Time.deltaTime;
        currentFade = Mathf.Lerp(0, fade, time);

        if (time <= 0)
        {
            time = originalTimer;
            isFading = false;
            fadingUnselected = false;
            fade = 1;
        }

        image.material.SetFloat("_Fade", currentFade);
    }
}