using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        //string name = FindObjectOfType() 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayerNameChanged(string name)
    {

    }


    public void StartButtonPressed()
    {
        SceneManager.LoadScene(1);
        GameManager.instance.playerName = inputField.text;
    }

    public void QuitButtonPressed()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif

    }

}
