using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        
    }

	public void LoadSceneByIndex(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

	public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

  public void Quit()
  {
    Application.Quit();
  }

	public void openURL(string url){
		Application.OpenURL (url);

	}

}
