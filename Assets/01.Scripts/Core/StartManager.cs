using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Transform blackPanel;
    [SerializeField] private AudioClip bg;

    private void Awake()
    {
        SoundManager.Instance.PlayBGM(bg);
    }

    public void UseStart()
    {
        blackPanel.gameObject.SetActive(true);
        blackPanel.GetComponent<Image>().DOFade(1, 1).OnComplete(() =>
        {
            UserSceneManager.Instance.SceneLoad(SceneEnum.GameScene);
        });
    }

    public void UseExit()
    {
        Application.Quit();
    }
}
