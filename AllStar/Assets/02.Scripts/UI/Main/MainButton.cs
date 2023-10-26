using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    public void OnGameStartButton()
    {
        Managers.DataManager.Init(() =>
        {
            SceneManager.LoadSceneAsync(1);
        });
    }
    public void OnGameExitButton()
    {
        Application.Quit();
    }
}
