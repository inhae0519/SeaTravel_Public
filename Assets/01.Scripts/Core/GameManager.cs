using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private bool isPause;
    public bool isShip;
    public void Pause()
    {
        if (!isPause)
        {
            Time.timeScale = 0;
            UIManager.Instance.EnablePause();
        }
        else
        {
            Time.timeScale = 1;
            UIManager.Instance.EnablePause();
        }
        PlayerManager.Instance.PlayerInputCheck();
        isPause = !isPause;
    }

    public void GameOver()
    {
        UIManager.Instance.Fade(1, UIManager.Instance.EnableGameOver);
    }

    public void GameClear()
    {
        UIManager.Instance.Fade(1, UIManager.Instance.EnableGameClear);
    }

    public void ReStart()
    {
        UserSceneManager.Instance.SceneLoad(SceneEnum.GameScene);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
