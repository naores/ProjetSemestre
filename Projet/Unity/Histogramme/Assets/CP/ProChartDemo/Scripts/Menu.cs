#if UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
	#define PRE_UNITY_5_3
#endif

using UnityEngine;
using System.Collections;

#if !PRE_UNITY_5_3
	using UnityEngine.SceneManagement;
#endif	

///<summary>
/// Menu class to transfer jumping function to load a scene
///</summary>
public class Menu : MonoBehaviour
{
	public void LoadScene(string scene)
	{
#if PRE_UNITY_5_3
		Application.LoadLevel(scene);
#else
		SceneManager.LoadScene(scene);
#endif
	}

}
