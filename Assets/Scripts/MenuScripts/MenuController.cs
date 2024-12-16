using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public string scenename;
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(scenename);
    }
    public void QuitGame()
    {
        Debug.Log("Game closed");
        Application.Quit();
    }
}
