using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    // Start is called before the first frame update
    void Update()
    {
        float move = (Input.mousePosition.x - Screen.width / 2f) / Screen.width / 2f;
        transform.DOMoveX(move, 5.0f);
    }
}
