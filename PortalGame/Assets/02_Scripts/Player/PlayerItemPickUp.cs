using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemPickUp : MonoBehaviour
{
    public SlotManager slotManager;
    private PlayerInventory playerInventory;
    private List<GameObject> itemSlots;

    public GameObject addItem;

    // 시작
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        itemSlots = playerInventory.itemSlots;
    }

    //item 충돌체크
    private void OnTriggerEnter(Collider other)
    {
        if (playerInventory.hasItemCount >= playerInventory.hasItemMaxCount)
        {
            TestMode.DebugLog("가방이 꽉찼습니다.");
        }
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Item")) && playerInventory.hasItemCount < playerInventory.hasItemMaxCount)
        {
            if (playerInventory.itemCount.ContainsKey(other.transform.tag))
            {
                UpdateInventory(true, other.gameObject);
            }
            else
            {
                playerInventory.hasItemCount++;
                playerInventory.itemCount.Add(other.transform.tag, 1);
                playerInventory.itemPictureList.Add(other.transform.tag, other.GetComponent<SpriteRenderer>().sprite);
                TestMode.DebugLog(playerInventory.itemCount[other.transform.tag]);
                UpdateInventory(false, other.gameObject);
            }
            Destroy(other.transform.parent.gameObject);
        }
    }
    private void UpdateInventory(bool isHasThisItem,GameObject obj)
    {
        string tagName = obj.tag;
        if (isHasThisItem)
        {
            playerInventory.itemCount[tagName] = playerInventory.itemCount[tagName] + 1;
            playerInventory.itemObjectByName[tagName].transform.GetComponentInChildren<Text>().text = playerInventory.itemCount[tagName].ToString();
            if (slotManager.slotItemObjectByName.ContainsKey(tagName))
            {
                slotManager.slotItemObjectByName[tagName].transform.GetComponentInChildren<Text>().text = playerInventory.itemCount[tagName].ToString();
            }
            
        }
        else
        {
            addItem.GetComponent<Image>().sprite = playerInventory.itemPictureList[tagName];
            addItem.GetComponent<ItemState>().itemName = tagName;
            GameObject newInventoryItem = Instantiate(addItem, itemSlots[playerInventory.itemObjectByName.Count].GetComponent<RectTransform>().transform);
            playerInventory.itemObjectByName.Add(tagName, newInventoryItem);
            newInventoryItem.GetComponent<RectTransform>().position = new Vector2(
                itemSlots[playerInventory.itemObjectByName.Count-1].GetComponent<RectTransform>().position.x,
                itemSlots[playerInventory.itemObjectByName.Count-1].GetComponent<RectTransform>().position.y
            );

            newInventoryItem.transform.Find("CountText").GetComponent<RectTransform>().position = new Vector2(
                itemSlots[playerInventory.itemObjectByName.Count-1].GetComponent<RectTransform>().position.x - itemSlots[playerInventory.itemObjectByName.Count].GetComponent<RectTransform>().rect.width / 2,
                itemSlots[playerInventory.itemObjectByName.Count-1].GetComponent<RectTransform>().position.y - itemSlots[playerInventory.itemObjectByName.Count].GetComponent<RectTransform>().rect.height / 4
            );

            newInventoryItem.SetActive(true);

            playerInventory.itemCountText.text = playerInventory.hasItemCount+" / "+ playerInventory.hasItemMaxCount;
        }

    }
}
