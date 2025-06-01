using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootingSpot : Interactble
{
    // 상자에서 나올 아이템 리스트
    [SerializeField] private ItemDataListSO _itemDataListSo;
    // 리스트 중에서 나올 아이템 수
    [SerializeField] private int itemCount;
    // 이 상자의 칸 수
    [SerializeField] private int slotSize;
    // UI창이 비활성화 되는 거리
    [SerializeField] private float disableDistance;
    
    private List<int> noSameItemList = new();
    private List<int> noSameSlotIndex = new();
    private List<int> slotIndex = new ();
    private Dictionary<ItemDataSo, int> itemAndStackDic = new();

    private readonly System.Random _random = new();

    private Transform playerTrm;
    private bool isOpen;
    
    private void Awake()
    {
        if (itemCount > _itemDataListSo.itemDataInfos.Count)
            itemCount = _itemDataListSo.itemDataInfos.Count;
        
        playerTrm = PlayerManager.Instance.PlayerTrm;
        for (int i = 0; i < _itemDataListSo.itemDataInfos.Count; ++i)
        {
            noSameItemList.Add(i);
        }

        for (int i = 0; i < slotSize; ++i)
        {
            noSameSlotIndex.Add(i);
        }

        for (int i = 0; i < itemCount; ++i)
        {
            int randomItem = RandomPick(noSameItemList);
            int randomSlotIndex = RandomPick(noSameSlotIndex);

            slotIndex.Add(randomSlotIndex);
            itemAndStackDic.Add(_itemDataListSo.itemDataInfos[randomItem].itemList,
                Random.Range(_itemDataListSo.itemDataInfos[randomItem].itemMinStack, 
                    _itemDataListSo.itemDataInfos[randomItem].itemMaxStack));
        }
    }

    private void Update()
    {
        if(!isOpen)
            return;
        
        float distance = Vector3.Distance(transform.position, playerTrm.position);
        if (distance > disableDistance || Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = false;
            UIManager.Instance.EnableLootPanel(false);
            UIManager.Instance.DisableRightClickPanel();
        }
    }

    public override void Interact()
    {
        LootInventory.Instance.SetLootStash(itemAndStackDic, slotIndex);
        if(isOpen == false)
        {
            isOpen = true;
            UIManager.Instance.EnableLootPanel(true);
        }
    }

    private int RandomPick(List<int> list)
    {
        int randomIndex = _random.Next(list.Count);
        int randomItem = list[randomIndex];
        list.RemoveAt(randomIndex);

        return randomItem;
    }
}
