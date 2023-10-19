using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{
    private PlayerInventory playerInventory;
    public SlotManager slotManager;
    public GameObject miniMap;
    private List<string> usingItemList = new List<string>();

    public Dictionary<int,string> hotkeyByItemName = new Dictionary<int,string>();
    enum ITEM_NAME_LIST 
    {
        MINIMAP = 0,
        TEST
    }
    public Dictionary<string,Enum> enumByItemName = new Dictionary<string,Enum>();

    private void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        enumByItemName.Add("MiniMap", ITEM_NAME_LIST.MINIMAP);
        enumByItemName.Add("Test", ITEM_NAME_LIST.TEST);
    }

    // 업데이트
    void Update()
    {
        /* 업데이트 내에서 for문 이 돌기때문에 최적화 문제 발생
        foreach (int key in hotkeyByItemName.Keys)
        {
            if(Input.GetKeyDown((KeyCode)key) && key != -1)
            {
                Use(hotkeyByItemName[key]);
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.Alpha0) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha0)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha0]);
        if (Input.GetKeyDown(KeyCode.Alpha1) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha1)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha1]);
        if (Input.GetKeyDown(KeyCode.Alpha2) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha2)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha2]);
        if (Input.GetKeyDown(KeyCode.Alpha3) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha3)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha3]);
        if (Input.GetKeyDown(KeyCode.Alpha4) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha4)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha4]);
        if (Input.GetKeyDown(KeyCode.Alpha5) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha5)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha5]);
        if (Input.GetKeyDown(KeyCode.Alpha6) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha6)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha6]);
        if (Input.GetKeyDown(KeyCode.Alpha7) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha7)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha7]);
        if (Input.GetKeyDown(KeyCode.Alpha8) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha8)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha8]);
        if (Input.GetKeyDown(KeyCode.Alpha9) && hotkeyByItemName.ContainsKey((int)KeyCode.Alpha9)) SlotItemCheck(hotkeyByItemName[(int)KeyCode.Alpha9]);
    }

    void SlotItemCheck(string itemName)
    {
        if (slotManager.slotItemObjectByName.ContainsKey(itemName))
        {
            ItemCountCheck(itemName);
        }
    }

    void ItemCountCheck(string itemName)
    {
        TestMode.DebugLog(IsUsingThisItem(itemName));
        if (!IsUsingThisItem(itemName))
        {
            usingItemList.Add(itemName);
        }
        else
        {
            TestMode.DebugLog("지금 사용중인 아이템 입니다.");
            return;
        }
        ItemUse(itemName);
    }

    void ItemUse(string itemName)
    {
        playerInventory.itemCount[itemName] = playerInventory.itemCount[itemName] - 1;
        if (playerInventory.itemCount[itemName] <= 0)
        {
            TestMode.DebugLog("init 작동");
            ItemInIt(itemName);
        }
        else
        {
            playerInventory.itemObjectByName[itemName].GetComponentInChildren<Text>().text = playerInventory.itemCount[itemName].ToString();
            slotManager.slotItemObjectByName[itemName].GetComponentInChildren<Text>().text = playerInventory.itemCount[itemName].ToString();
        }
        
        if (enumByItemName[itemName].Equals(ITEM_NAME_LIST.MINIMAP))
        {
            miniMap.SetActive(true);
            StartCoroutine(setActive_Time(miniMap, itemName, 10f, true, 4));
        }

        else if (enumByItemName[itemName].Equals(ITEM_NAME_LIST.TEST))
        {
            TestMode.DebugLog("아이템 사용!" + itemName);
        }

    }

    bool IsUsingThisItem(string itemName)
    {
        bool returnType = false;
        for (int i = 0; i < usingItemList.Count; i++)
        {
            if (usingItemList[i].Equals(itemName)) returnType = true;
        }
        return returnType;
    }

    float curTime = 0;
    IEnumerator setActive_Time(GameObject gameObject,string itemName,float time,bool isTwink = false,int repeatCount = 0)
    {
        TestMode.DebugLog("miniMap 호출");
        int count = 0;
        if (isTwink)
        {
            while (curTime < time)
            {
                yield return new WaitForEndOfFrame();
                curTime += Time.deltaTime;
                if (curTime > time - 4f)
                {
                    curTime += Time.deltaTime;
                    count++;
                    yield return new WaitForEndOfFrame();
                    if ((int)curTime % 2 == 0)
                    {
                        gameObject.SetActive(!gameObject.activeSelf);
                    }
                }
            }
            gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
        usingItemList.Remove(itemName);
        curTime = 0;
        
    }


    void ItemInIt(string itemName)
    {
        TestMode.DebugLog("아이템 사용 완료");
        hotkeyByItemName.Remove(slotManager.slotItemObjectByName[itemName].GetComponent<ItemState>().hotKey);
        playerInventory.itemCount.Remove(itemName);
        Destroy(playerInventory.itemObjectByName[itemName]);
        playerInventory.itemObjectByName.Remove(itemName);
        playerInventory.itemPictureList.Remove(itemName);
        playerInventory.hasItemCount = playerInventory.hasItemCount - 1;
        playerInventory.itemCountText.text = playerInventory.hasItemCount + " / " + playerInventory.hasItemMaxCount;
        if (slotManager.slotItemObjectByName.ContainsKey(itemName))
        {
            slotManager.slotItemObjectByName[itemName].GetComponent<ItemState>().hitObject_SlotState.isOnItem = false;
            Destroy(slotManager.slotItemObjectByName[itemName]);
            slotManager.slotItemObjectByName.Remove(itemName);
        }
        
    }
    
}
