using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TAG
{
    NULL = -1,
    HOME,
    HUMAN,
    MONSTER,
    PLANT,
    WEAPON,
    MAX_NUM,
}

public struct Page
{
    public string graphic;
    public string title;
    public string description;
    public int textSize;
    public float graphicScale;
    public TAG tag;
}

[Serializable]
public struct BGM
{
    public string songName;
    public float volume;
}

public class ContentLoader : Singleton<ContentLoader>
{
    public List<Page> pageContent;
    public List<BGM> BGMList;
    public void Initialization()
    {
        // create content variable
        pageContent = new List<Page>();

        string[] data = ReadCSV("Document/Content");

        const int columnCount = 7;
        int tableSize = data.Length / columnCount - 1; // 7 is column number
        for (int i = 0; i < tableSize; i++)
        {
            int pageNumber = DataToInteger(data[columnCount * (i + 1)]);
            string tag = data[columnCount * (i + 1) + 1];
            Page newPage;
            newPage.graphic = data[columnCount * (i + 1) + 2];
            newPage.title = data[columnCount * (i + 1) + 3];
            newPage.description = data[columnCount * (i + 1) + 4];
            newPage.textSize = DataToInteger(data[columnCount * (i + 1) + 5]);
            newPage.graphicScale = DataToFloat(data[columnCount * (i + 1) + 6]);

            //Compare Tag string
            TAG targetTag = TAG.NULL;
            if (tag.Contains("Home"))
            {
                targetTag = TAG.HOME;
            }
            else if(tag.Contains("Human"))
            {
                targetTag = TAG.HUMAN;
            }
            else if (tag.Contains("Monster"))
            {
                targetTag = TAG.MONSTER;
            }
            else if (tag.Contains("Plant"))
            {
                targetTag = TAG.PLANT;
            }
            else if (tag.Contains("Weapon"))
            {
                targetTag = TAG.WEAPON;
            }

            newPage.tag = targetTag;

            if (targetTag != TAG.NULL)
            {
                pageContent.Add(newPage);
            }
        }
    }

    int DataToInteger(string txt)
    {
        if (txt.Length == 0) return 0;

        int rtn;
        int.TryParse(txt, out rtn);

        return rtn;
    }

    float DataToFloat(string txt)
    {
        if (txt.Length == 0) return 0f;

        float rtn;
        float.TryParse(txt, out rtn);

        return rtn;
    }

    string[] ReadCSV(string file)
    {
        TextAsset textAssetData = Resources.Load<TextAsset>(file);

        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        return data;
    }

    public int GetMaxPageNumber()
    {
        return pageContent.Count;
    }

    public Page GetContent(int page)
    {
        Page rtn;

        rtn = pageContent[page-1];

        return rtn;
    }

    public int GetPageWithTag(TAG tag)
    {
        int rtn = 1;

        for (int i = 0; i < GetMaxPageNumber()-1; i++)
        {
            if (pageContent[i].tag == tag)
            {
                rtn = i+1;
                break;
            }
        }

        return rtn;
    }

    public int GetRandomPage(int exclude)
    {
        int rtn = 0;

        do
        {
            rtn += UnityEngine.Random.Range(1, GetMaxPageNumber());
        } while (rtn == exclude);

        return rtn;
    }



    // Audio
    public List<BGM> LoadAudioDocument()
    {
        // create content variable
        BGMList = new List<BGM>();
        string[] data = ReadCSV("Document/BGM List");

        const int columnCount = 2;
        int tableSize = data.Length / columnCount - 1; // 2 is column number

        for (int i = 0; i < tableSize; i++)
        {
            BGM newEntry;
            newEntry.songName = data[columnCount * (i + 1)];
            newEntry.volume = DataToFloat(data[columnCount * (i + 1) + 1]);
            BGMList.Add(newEntry);
        }

        return BGMList;
    }
}
