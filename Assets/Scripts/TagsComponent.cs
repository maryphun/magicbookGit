using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TagsComponent : MonoBehaviour
{
    [SerializeField] TAG tag;
    [SerializeField] bool isRandomTag = false;
    [SerializeField, Range(-10f, 10f)] private float hoverMoveX = -10.0f;
    [SerializeField] private SpriteRenderer selection;

    private BookController controller;
    private Collider2D collider;
    private bool onHover;
    private bool onSelected;
    private SpriteRenderer graphic;
    private float originalPosX;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider2D>();
        graphic = GetComponentInChildren<SpriteRenderer>();
        onHover = false;
        originalPosX = transform.position.x;
        controller = FindObjectOfType<BookController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onSelected) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        if (collider.bounds.Contains(mousePos2D))
        {
            if (!onHover)
            {
                onHover = true;
                graphic.transform.DOMoveX(originalPosX + hoverMoveX, 0.2f);
                selection.DOFade(1.0f, 0.0f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                controller.ChangeToTag(tag);
                if (isRandomTag) controller.ChangeToRandomPage();
                selection.DOFade(0.0f, 0.0f);
            }
        }
        else
        {
            if (onHover)
            {
                onHover = false;
                graphic.transform.DOMoveX(originalPosX, 0.2f);
                selection.DOFade(0.0f, 0.0f);
            }
        }
    }

    public void SetSelected(bool boolean)
    {
        if (boolean == onSelected) return;

        onSelected = boolean;

        if (onSelected)
        {
            graphic.transform.DOMoveX(originalPosX + hoverMoveX, 0.2f);
        }
        else
        {
            graphic.transform.DOMoveX(originalPosX, 0.2f);
        }
    }
}
