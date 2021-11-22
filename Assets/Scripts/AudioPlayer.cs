using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] bool random;
    [SerializeField, Range(0f, 1f)] float volume = 0.75f; 
    [SerializeField] BGM[] bgmList;

    // Reference
    TMP_Text audioPlayerText;
    Collider2D pauseBtn;
    Collider2D startBtn;
    Collider2D nextBtn;
    Collider2D previousBtn;
    int currentMusicIndex;

    private void Awake()
    {
        audioPlayerText = GameObject.Find("AudioNameText").GetComponent<TMP_Text>();
        pauseBtn = GameObject.Find("Pause").GetComponent<Collider2D>();
        startBtn = GameObject.Find("Start").GetComponent<Collider2D>();
        nextBtn = GameObject.Find("Next").GetComponent<Collider2D>();
        previousBtn = GameObject.Find("Previous").GetComponent<Collider2D>();

        List<BGM> list = ContentLoader.Instance().LoadAudioDocument();
        bgmList = new BGM[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            bgmList[i] = list[i];
        }
    }

    // Start is called before the first frame update
    public void InitAudio()
    {
        currentMusicIndex = 0;
        if (bgmList.Length > 0)
        {
            int newndex = 0;
            if (random)
            {
                newndex = RandomMusicIndex();
            }
            AudioManager.Instance.SetMusicVolume(bgmList[newndex].volume);
            AudioManager.Instance.PlayMusicWithFade(bgmList[newndex].songName, 3.0f);
            audioPlayerText.SetText("♪ - " + bgmList[newndex].songName);
        }
    }

    private int RandomMusicIndex(int exception = -1)
    {
        List<int> randomIndex = new List<int>();

        for (int i = 0; i < bgmList.Length; i++)
        {
            if (i != exception)
            {
                randomIndex.Add(i);
            }
        }

        currentMusicIndex = Random.Range(0, randomIndex.Count);
        return randomIndex[currentMusicIndex];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider == pauseBtn)
            {
                AudioManager.Instance.StopMusicWithFade(0.25f, true);
                audioPlayerText.SetText("♪ - " + bgmList[currentMusicIndex].songName);
            }
            if (hit.collider == startBtn)
            {
                AudioManager.Instance.UnpauseMusicWithFade(0.25f);
                audioPlayerText.SetText("♪ - " + bgmList[currentMusicIndex].songName);
            }
            if (hit.collider == nextBtn)
            {
                AudioManager.Instance.PlayMusicWithCrossFade(NextSong(true), 0.25f);
                audioPlayerText.SetText("♪ - " + bgmList[currentMusicIndex].songName);
            }
            if (hit.collider == previousBtn)
            {
                AudioManager.Instance.PlayMusicWithCrossFade(PreviousSong(true), 0.25f);
                audioPlayerText.SetText("♪ - " + bgmList[currentMusicIndex].songName);
            }
        }
    }

    private string NextSong(bool SetMusicVolume)
    {
        int nextSongIndex = currentMusicIndex + 1;
        if (nextSongIndex >= bgmList.Length)
        {
            nextSongIndex = 0;
        }
        //Debug.Log(nextSongIndex);

        if (SetMusicVolume)
        {
            AudioManager.Instance.SetMusicVolume(bgmList[nextSongIndex].volume);
        }

        currentMusicIndex = nextSongIndex;
        return bgmList[nextSongIndex].songName;
    }
    private string PreviousSong(bool SetMusicVolume)
    {
        int previousSongIndex = currentMusicIndex - 1;
        if (previousSongIndex < 0)
        {
            previousSongIndex = bgmList.Length-1;
        }

        //Debug.Log(previousSongIndex);

        if (SetMusicVolume)
        {
            AudioManager.Instance.SetMusicVolume(bgmList[previousSongIndex].volume);
        }

        currentMusicIndex = previousSongIndex;
        return bgmList[previousSongIndex].songName;
    }
}
