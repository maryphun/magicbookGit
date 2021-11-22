using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rune : MonoBehaviour
{
    [SerializeField] float floatingRange = 1.0f;
    [SerializeField] float floatingSpeed = 1.0f;
    private Vector2 originalPos;
    private float count;

    private void Start()
    {
        originalPos = transform.position;
    }
    void Update()
    {
        count += Time.deltaTime * floatingSpeed;
        transform.DOMoveY(originalPos.y + Mathf.Sin(count) * floatingRange, 0.0f);
    }
}
