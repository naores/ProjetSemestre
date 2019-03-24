
using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

///<summary>
/// Menu class to transfer jumping function to load a scene
///</summary>
public class MonMenu : MonoBehaviour
{
    public void LoadScene(string scene)
    {

        SceneManager.LoadScene(scene);
    }

}
