using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderSelected : MonoBehaviour
{
    [SerializeField] private Material selectedMaterial;
    [SerializeField] public bool triggerSelected = false;
    [SerializeField] public bool triggerUnselected = false;
    [SerializeField] private float time = 1;
    [SerializeField] private float fade = 1;
    [SerializeField] private float currentFade;
    [SerializeField] private Image image;

    private float originalTimer;

    private void Start()
    {
        originalTimer = time;
        image.material = new Material(selectedMaterial);
    }

    private void Update()
    {
        if (triggerSelected == true)
        {
            time -= Time.deltaTime;
            currentFade = Mathf.Lerp(fade, 0, time);

            if (time <= 0)
            {
                time = originalTimer;
                triggerSelected = false;
                fade = 1;
            }

            image.material.SetFloat("_Fade", currentFade);
        }

        if (triggerUnselected == true)
        {
            time -= Time.deltaTime;
            currentFade = Mathf.Lerp(0, fade, time);

            if (time <= 0)
            {
                time = originalTimer;
                triggerUnselected = false;
                fade = 1;
            }

            image.material.SetFloat("_Fade", currentFade);
        }
    }
}