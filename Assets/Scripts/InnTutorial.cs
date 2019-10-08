using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.GetInt("InnTutorial", 0) == 1)
            gameObject.SetActive(false);
    }

    public void Close()
    {
        PlayerPrefs.SetInt("InnTutorial", 1);
        gameObject.SetActive(false);
    }
}
