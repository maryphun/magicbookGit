using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCover : MonoBehaviour
{
    [SerializeField] BookController magicbook;
    [SerializeField] Animator BookCoverAnimation;

    Collider2D collider;
    ScreenFadeManager screenFade;
    AudioPlayer audioPlayer;

    SpriteRenderer sprite;

    bool Opened = false;
    private void Awake()
    {
        AudioManager.Instance.SetSEMasterVolume(1f);
        ContentLoader.Instance().Initialization();
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        screenFade = FindObjectOfType<ScreenFadeManager>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Opened) return; 

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            if (collider.bounds.Contains(mousePos2D))
            {
                AudioManager.Instance.PlaySFX("openbook");
                StartCoroutine(PlayAnimationStart());
                Opened = true;
            }
        }
    }

    IEnumerator PlayAnimationStart()
    {
        sprite.color = new Color(1, 1, 1, 0);
        BookCoverAnimation.Play("Open");
        yield return new WaitForSeconds(0.6f);

        magicbook.gameObject.SetActive(true);
        magicbook.Initialization();
        audioPlayer.InitAudio();

        yield return new WaitForSeconds(0.4f);
    }
    IEnumerator PlayAnimationClose()
    {
        BookCoverAnimation.Play("Close");
        magicbook.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.6f);

        audioPlayer.InitAudio();
        sprite.color = new Color(1, 1, 1, 1);
        Opened = false;
    }

    public void CloseBook()
    {
        AudioManager.Instance.PlaySFX("openbook");
        StartCoroutine(PlayAnimationClose());
    }
}
