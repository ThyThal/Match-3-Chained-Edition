using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderDissolve : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private bool isDissolving = false;
    [SerializeField] private float fade = 1;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDissolving = true;
        }

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0)
            {
                fade = 0f;
                isDissolving = false;
            }

            material.SetFloat("_Fade", fade);
        }
    }

    public void ResetValues()
    {
        material = GetComponent<SpriteRenderer>().material;
        fade = 1;
    }
}
