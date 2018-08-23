using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        if (GUILayout.Button("AugmentedImage"))
            SceneManager.LoadScene("AugmentedImage");
        if (GUILayout.Button("CloudAnchors"))
            SceneManager.LoadScene("CloudAnchors");
        if (GUILayout.Button("ComputerVision"))
            SceneManager.LoadScene("ComputerVision");
        if (GUILayout.Button("HelloAR"))
            SceneManager.LoadScene("HelloAR");
    }
}
