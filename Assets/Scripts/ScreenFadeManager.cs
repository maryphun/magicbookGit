using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFadeManager : MonoBehaviour
{
    [SerializeField] Image fadeAlpha;

    float currentAlpha = 0.0f;

    public float GetCurrentAlpha()
    {
        return currentAlpha;
    }

    public void SetScreenAlpha(float alpha, float time)
    {
        fadeAlpha.DOFade(alpha, time);
    }
}
