using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Inventory");
    }
    public void QuitGame()
    {
        Debug.Log("Game closed");
        Application.Quit();
    }
}
