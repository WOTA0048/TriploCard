using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingScript : MonoBehaviour
{
    [SerializeField] private float loadProgress;

    
    // Start is called before the first frame update
    void Start()
    {
        /*
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadProgress = loadingOperation.progress;

        if (loadingOperation.isDone)
        {
            // Loading is finished !
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
