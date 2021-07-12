using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderSelected : MonoBehaviour
{
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private BlockController blockController;
    [SerializeField] private float selectTimer = 0.25f;
    [SerializeField] private float fade = 1;
    [SerializeField] private float currentFade;
    [SerializeField] public Image image;

    private float originalSelectTimer;
    [SerializeField] public bool fadingSelected;
    [SerializeField] public bool fadingUnselected;
    [SerializeField] private bool isFading;
    private float destroyTimer;
    private float originalDestroyTimer;





    private void Start()
    {
        originalSelectTimer = selectTimer;
        image.material = new Material(selectedMaterial);
        destroyTimer = blockController.DestroyTime;
        originalDestroyTimer = destroyTimer;

    }

    private void Update()
    {
        if (blockController.Destroyed == true)
        {
            destroyTimer -= Time.deltaTime;
            currentFade = Mathf.Lerp(0, fade, destroyTimer);
            image.material.SetFloat("_DestroyFade", currentFade);
        }

        else if (isFading == true && blockController.Destroyed == false)
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

        if (blockController.Destroyed == false)
        {
            image.material.SetFloat("_DestroyFade", 1);
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
        selectTimer -= Time.deltaTime;
        currentFade = Mathf.Lerp(fade, 0, selectTimer);

        if (selectTimer <= 0)
        {
            selectTimer = originalSelectTimer;
            isFading = false;
            fadingSelected = false;
            fade = 1;
        }

        image.material.SetFloat("_Fade", currentFade);
    }
    private void FadeUnselect()
    {
        selectTimer -= Time.deltaTime;
        currentFade = Mathf.Lerp(0, fade, selectTimer);

        if (selectTimer <= 0)
        {
            selectTimer = originalSelectTimer;
            isFading = false;
            fadingUnselected = false;
            fade = 1;
        }

        image.material.SetFloat("_Fade", currentFade);
    }

    public void ResetShader()
    {
        destroyTimer = originalDestroyTimer;
        selectTimer = originalSelectTimer;
        fadingSelected = false;
        fadingUnselected = false;
        image.material.SetFloat("_Fade", 0);
        image.material.SetFloat("_DestroyFade", 1);
    }
}