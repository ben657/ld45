using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTutorial : MonoBehaviour
{
    public DungeonBuilder builder;
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.GetInt("DungeonTutorial", 0) == 1)
            Close();
    }

    public void Close()
    {
        PlayerPrefs.SetInt("DungeonTutorial", 1);
        gameObject.SetActive(false);
        builder.Build();
    }
}
