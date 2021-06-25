using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderElectricity : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private float time = 1;
    [SerializeField] private float maxPower = 2;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSelected = true;
        }

        if (isSelected)
        {
            time -= Time.deltaTime;
            float power = Mathf.Lerp(maxPower, 0, time);

            if (time <= 0)
            {
                time = 0f;
                isSelected = false;
            }

            material.SetFloat("_ElectricityPower", power);
        }
    }


    public void ResetValues()
    {
        material = GetComponent<SpriteRenderer>().material;
        time = 1;
    }
}
