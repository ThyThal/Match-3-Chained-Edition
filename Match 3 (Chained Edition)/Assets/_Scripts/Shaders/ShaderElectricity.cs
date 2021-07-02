using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderElectricity : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material electricityMaterial;
    [SerializeField] private Image image;
    [SerializeField] public bool isSelected = false;
    [SerializeField] private float time = 1;
    [SerializeField] private float maxPower = 2;

    private void Start()
    {
        image = GetComponent<Image>();
        defaultMaterial = image.material;
        electricityMaterial = new Material(electricityMaterial);
    }

    private void Update()
    {
        if (isSelected)
        {
            if (image.material != electricityMaterial)
            {
                image.material = electricityMaterial;
            }

            time -= Time.deltaTime;
            float power = Mathf.Lerp(maxPower, 0, time);

            if (time <= 0)
            {
                time = 0f;
                isSelected = false;
            }

            electricityMaterial.SetFloat("_ElectricityPower", power);
        }
    }


    public void ResetValues()
    {
        defaultMaterial = GetComponent<SpriteRenderer>().material;
        time = 1;
    }
}
