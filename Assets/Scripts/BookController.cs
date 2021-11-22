using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BookController : MonoBehaviour
{
    [SerializeField] Animator animation;
    [SerializeField] Collider2D collider;
    [SerializeField] TMP_Text pageText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] SpriteRenderer pageImage;
    [SerializeField] BookCover cover;

    Dictionary<TAG, TagsComponent> tags;
    int currentPage = 1;
    TAG currentTag;

    public void Initialization()
    {
        tags = new Dictionary<TAG, TagsComponent>();
        tags.Add(TAG.HOME, GameObject.Find("tag home").GetComponent<TagsComponent>());
        tags.Add(TAG.HUMAN, GameObject.Find("tag human").GetComponent<TagsComponent>());
        tags.Add(TAG.MONSTER, GameObject.Find("tag monster").GetComponent<TagsComponent>());
        tags.Add(TAG.PLANT, GameObject.Find("tag plant").GetComponent<TagsComponent>());
        tags.Add(TAG.WEAPON, GameObject.Find("tag weapon").GetComponent<TagsComponent>());

        currentTag = TAG.NULL;
        int currentPage = 1;

        ChangePage(currentPage);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            if (collider.bounds.Contains(mousePos2D))
            {
                bool change = false;
                if (mousePos2D.x > collider.bounds.center.x)
                {
                    if (currentPage < ContentLoader.Instance().GetMaxPageNumber())
                    {
                        currentPage++;
                        change = true;
                    }
                }
                else
                {
                    if (currentPage > 1)
                    {
                        currentPage--;
                        change = true;
                    }
                    else
                    {
                        // close book
                        cover.CloseBook();
                        pageText.SetText("");
                        titleText.SetText("");
                        descriptionText.SetText("");
                    }
                }
                if (change) ChangePage(currentPage);
            }
        }
    }

    public void ChangePage(int value, bool playSE = true, bool random = false)
    {
        int oldpage;
        int.TryParse(pageText.text, out oldpage);
        pageText.SetText(value.ToString());

        Page page = ContentLoader.Instance().GetContent(value);
        titleText.SetText(page.title);
        descriptionText.SetText(page.description);
        descriptionText.fontSize = page.textSize;
        pageImage.sprite = Resources.Load<Sprite>("Image/" + page.graphic);
        pageImage.color = Color.white;
        if (pageImage.sprite == null)
        {
            pageImage.DOFade(0, 0);
        }
        pageImage.transform.localScale = new Vector3(page.graphicScale, page.graphicScale, page.graphicScale);

        // change tag
        if (currentTag != page.tag)
        {
            if (currentTag != TAG.NULL) tags[currentTag].SetSelected(false);
            if (page.tag != TAG.NULL) tags[page.tag].SetSelected(true);
            currentTag = page.tag;
        }

        if (random)
        {
            animation.Play("nextpageAlot");

            if (playSE) AudioManager.Instance.PlaySFX("Book02-1(Flip)", 0.2f);
        }
        else if (value > oldpage)
        {
            animation.Play("nextpage");

            if (playSE) AudioManager.Instance.PlaySFX("Book02-1(Flip)", 0.2f);
        }
        else if (value < oldpage)
        {
            animation.Play("previouspage");
            if (playSE) AudioManager.Instance.PlaySFX("Book02-1(Flip)", 0.2f);
        }
    }

    public void ChangeToTag(TAG tag)
    {
        if (tag == TAG.NULL) return;

        AudioManager.Instance.PlaySFX("Book02-4(Flick_Through)");
        int newPage = ContentLoader.Instance().GetPageWithTag(tag);
        currentPage = newPage;
        Debug.Log("Change to page " + newPage.ToString());
        ChangePage(newPage, false);
    }

    public void ChangeToRandomPage()
    {
        AudioManager.Instance.PlaySFX("Book02-4(Flick_Through)");
        int newPage = ContentLoader.Instance().GetRandomPage(currentPage);
        currentPage = newPage;
        ChangePage(newPage, false, true);
    }
}
