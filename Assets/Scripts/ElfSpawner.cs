using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfSpawner : MonoBehaviour
{
    [SerializeField] GameObject elfobject;
    List<ElfController> elflist;

    // Start is called before the first frame update
    void Start()
    {
        elflist = new List<ElfController>();

        int num = Random.RandomRange(8, 12);
        for (int i = 0; i < num; i++)
        {
            GameObject newElf = Instantiate(elfobject);
            elflist.Add(newElf.GetComponent<ElfController>());
        }
    }
}
