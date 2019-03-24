using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangerSceneCanvas : MonoBehaviour
{
  
    void OnSelect()
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        SceneManager.LoadScene("CanvasDemoBar");

    }

}