using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Linear Progress Bar")]
    public static void AddRadialProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Linear Progress Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform);
    }


#endif

    public int maximum;
    public int minimum;
    public int current;

    [SerializeField] private Image mask;
    [SerializeField] private Image fill;
    [SerializeField] private Color fillColor;
    [SerializeField] private float lerpSpeed = 1;
    [SerializeField] private ParticleSystem particleSystem;

    private void Update()
    {
        GetCurrentFill();
    }

    private void Awake()
    {
        particleSystem.Stop();
    }

    [ContextMenu("Get Current Fill")]
    void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;

        if (mask.fillAmount != fillAmount)
        {
            mask.fillAmount = Mathf.Lerp(mask.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
            fill.color = fillColor;
            ParticlePosition();
        }

        if (mask.fillAmount >= fillAmount && particleSystem.isPlaying)
        {
            particleSystem.Stop();
            Debug.Log("stop");
        }
    }

    private void ParticlePosition()
    {
        if (particleSystem.isPlaying == false)
        {
            particleSystem.Play();
        }

        float pos = Mathf.Lerp(-254f, 254f, mask.fillAmount);
        Vector3 newPosition = new Vector3(pos, 0, 0);
        particleSystem.transform.localPosition = newPosition;
    }
}
