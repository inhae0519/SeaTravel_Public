using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Transform _canvas;
    
    private Transform playerUITrm;
    private Image playerHealtlhImage;
    private Image hungerImage;
    private Image thirstImage;
    private Image energyImage;
    private Transform oxygenBar;
    private Image oxygenImage;
    
    private Transform shipUITrm;
    private Image shipHealthImage;

    private Transform inventoryTrm;
    private Transform usePanelTrm;
    private Button currentUseButton;
    private Button currentRemoveButton;
    private Transform lootInventoryTrm;

    private Transform pausePanel;
    private bool isPause = true;
    private Slider _effectSlider;
    private Slider _bgSlider;

    private Transform mapTrm;
    private bool isMapOpen = true;

    private Image blackPanel;
    private Transform gameOverPanel;
    private Transform gameClearPanel;
    
    private List<Transform> uiList = new();
    
    private void Awake()
    {
        playerUITrm = _canvas.Find("PlayerUI");
        oxygenBar = playerUITrm.Find("OxygenBar");
        playerHealtlhImage = playerUITrm.Find("HealthBar/Bar").GetComponent<Image>();
        hungerImage = playerUITrm.Find("HungerBar/Bar").GetComponent<Image>();
        thirstImage = playerUITrm.Find("ThirstBar/Bar").GetComponent<Image>();
        oxygenImage = playerUITrm.Find("OxygenBar/Bar").GetComponent<Image>();
        energyImage = playerUITrm.Find("EnergyBar/Bar").GetComponent<Image>();

        shipUITrm = _canvas.Find("ShipUI");
        shipHealthImage = shipUITrm.Find("HealthBar/Bar").GetComponent<Image>();

        pausePanel = _canvas.Find("PausePanel");
        _bgSlider = pausePanel.Find("SettingPanel/BGSlider").GetComponent<Slider>();
        _effectSlider = pausePanel.Find("SettingPanel/EffectSlider").GetComponent<Slider>();

        inventoryTrm = _canvas.Find("InventoryPanel");
        usePanelTrm = inventoryTrm.Find("UsingPanel");
        currentUseButton = usePanelTrm.Find("UseButton").GetComponent<Button>();
        currentRemoveButton = usePanelTrm.Find("RemoveButton").GetComponent<Button>();

        lootInventoryTrm = _canvas.Find("LootInventoryPanel");
        gameOverPanel = _canvas.Find("GameOverPanel");
        gameClearPanel = _canvas.Find("GameClearPanel");

        mapTrm = _canvas.Find("Map");

        blackPanel = _canvas.Find("BlackPanel").GetComponent<Image>();

        _bgSlider.value = SoundManager.Instance.BgmValue;
        _effectSlider.value = SoundManager.Instance.EffectValue;
        _bgSlider.onValueChanged.AddListener(delegate(float f) { SoundManager.Instance.BgSoundSetting(_bgSlider); });
        _effectSlider.onValueChanged.AddListener(delegate(float f) { SoundManager.Instance.BgSoundSetting(_effectSlider); });
    }

    public void SetShipHealth(float value)
    {
        shipHealthImage.fillAmount = value;
    }

    public void SetPlayerStat(PlayerStatEnum statEnum, float value)
    {
        switch (statEnum)
        {
            case PlayerStatEnum.Health:
                playerHealtlhImage.fillAmount = value;
                break;
            case PlayerStatEnum.Energy:
                energyImage.fillAmount = value;
                break;
            case PlayerStatEnum.Hunger:
                hungerImage.fillAmount = value;
                break;
            case PlayerStatEnum.Thirst:
                thirstImage.fillAmount = value;
                break;
            case PlayerStatEnum.Oxygen:
                oxygenImage.fillAmount = value;
                break;
        }
    }

    public void OnOffInventory(bool isOpen)
    {
        inventoryTrm.gameObject.SetActive(isOpen);
        UIListAdd(inventoryTrm, isOpen);
        CursorEnable(isOpen);
    }

    public void OnUsingPanel(ItemDataSo itemData, Vector2 position)
    {
        usePanelTrm.gameObject.SetActive(true);
        usePanelTrm.position = position;
        if (currentUseButton != null || currentRemoveButton != null)
        {
            currentUseButton.onClick.RemoveAllListeners();
            currentRemoveButton.onClick.RemoveAllListeners();
        }
        
        currentUseButton.onClick.AddListener(() => {
            Inventory.Instance.RemoveItem(itemData);
            itemData.usingAction?.Invoke();
            DisableRightClickPanel();
        });

        // 떨구기 버튼을 눌렀을 때
        currentRemoveButton.onClick.AddListener(() => {
            Inventory.Instance.RemoveItem(itemData);
            //Instantiate(itemData.prefab, transform.position, quaternion.identity);
            DisableRightClickPanel();
        });
    }

    public void DisableRightClickPanel()
    {
        usePanelTrm.gameObject.SetActive(false);
    }

    public void EnableLootPanel(bool isEnable)
    {
        lootInventoryTrm.gameObject.SetActive(isEnable);
        UIListAdd(lootInventoryTrm, isEnable);
        CursorEnable(isEnable);
    }

    public void EnableOxygenBar(bool isActive)
    {
        oxygenBar.gameObject.SetActive(isActive);
    }

    public void EnableMap()
    {
        mapTrm.gameObject.SetActive(isMapOpen);
        UIListAdd(mapTrm, isMapOpen);
        CursorEnable(isMapOpen);
        isMapOpen = !isMapOpen;
    }

    public void EnablePause()
    {
        pausePanel.gameObject.SetActive(isPause);
        UIListAdd(pausePanel, isPause);
        CursorEnable(isPause);
        isPause = !isPause;
    }

    private void CursorEnable(bool isEnable)
    {
        if (isEnable)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;   
        }
        else
        {
            if(uiList.Count!=0)
                return;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;   
        }
        PlayerManager.Instance.StopCamRotate(isEnable);
    }

    private void UIListAdd(Transform ui, bool isEnable)
    {
        if(isEnable)
            uiList.Add(ui);
        else
            uiList.Remove(ui);
    }

    public void Fade(float value, UnityAction action = null)
    {
        blackPanel.gameObject.SetActive(true);
        blackPanel.DOFade(value, 1f).OnComplete(() => action());
    }

    public void EnableGameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        CursorEnable(true);
    }

    public void EnableGameClear()
    {
        gameClearPanel.gameObject.SetActive(true);
        CursorEnable(true);
    }
}
