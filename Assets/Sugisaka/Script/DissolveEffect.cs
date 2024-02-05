using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public sealed class DissolveEffect : MonoBehaviour
{
    [SerializeField]
    Renderer[] renderers = {};
    [SerializeField, Min(0)]
    float EffectDuration = 1f;
    [SerializeField]
    Ease effectEase = Ease.Linear;
    [SerializeField]
    string progressParamName = "_Progress";

    List<Material> materials = new List<Material>();
    Sequence sequence;

    private void Start()
    {
        GetMaterials();
    }

    public void DisableIn()
    {
        sequence = DOTween.Sequence().SetLink(gameObject).SetEase(effectEase);

        foreach (Material m in materials)
        {
            m.SetFloat(progressParamName, 0);
            sequence.Join(m.DOFloat(1, progressParamName, EffectDuration));
        }
        sequence.Play();
    }

    public void DisableOut()
    {
        sequence = DOTween.Sequence().SetLink(gameObject).SetEase(effectEase);

        foreach (Material m in materials)
        {
            m.SetFloat(progressParamName, 1);
            sequence.Join(m.DOFloat(0, progressParamName, EffectDuration));
        }
        sequence.Play();
    }



    void GetMaterials()
    {
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                materials.Add(m);
            }
        }
    }
}
