using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum SceneEnum
{
    TitleScene,
    GameScene
}

public class UserSceneManager : MonoSingleton<UserSceneManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SceneLoad(SceneEnum sceneEnum)
    {
        SceneManager.LoadScene((int)sceneEnum);
        Time.timeScale = 1;
    }
}
