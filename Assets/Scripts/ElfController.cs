using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElfController : MonoBehaviour
{
    Animator animator;
    Collider2D collider;
    [SerializeField] Vector2 targetMovePoint;
    [SerializeField] bool onFlee;
    [SerializeField] bool onHold;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = Random.Range(0.1f, 0.15f);
        targetMovePoint = transform.position;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        {
            // mouse click
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            if (collider.bounds.Contains(mousePos2D) && Input.GetMouseButton(0))
            {
                transform.DOKill(false);
                transform.position = mousePos2D;
                onHold = true;
            }
        }

        if (onHold)
        {
            if (Input.GetMouseButton(0))
            {
                return;
            }
            onHold = false;
            targetMovePoint = transform.position;
        }

        if (Vector2.Distance(targetMovePoint, transform.position) < 0.25f)
        {
            Vector2 newPointOnScreen = new Vector2(Random.Range( 0f, Screen.width ), Random.Range( 0f, Screen.height));
            
            targetMovePoint = Camera.main.ScreenToWorldPoint(newPointOnScreen);

            transform.DOMove(targetMovePoint, Random.Range(5f, 7f));
            onFlee = false;
        }

        if (onFlee) return;
        Vector2 mousePosition = Input.mousePosition;
        if (Vector2.Distance(Camera.main.ScreenToWorldPoint(mousePosition), transform.position) < 1.0f)
        {
            Vector2 normalizedVector = (transform.position - Camera.main.ScreenToWorldPoint(mousePosition)).normalized;
            targetMovePoint = new Vector2(transform.position.x, transform.position.y ) + (normalizedVector * Random.Range(20.0f, 30.0f));
            transform.DOMove(targetMovePoint, Random.Range(1f, 2f));
            onFlee = true;
        }
    }
}
