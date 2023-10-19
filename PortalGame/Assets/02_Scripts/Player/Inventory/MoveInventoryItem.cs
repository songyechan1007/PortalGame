using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveInventoryItem : MonoBehaviour
    , IDragHandler
    , IEndDragHandler

    /*
    , IPointerEnterHandler
    , IPointerExitHandler
    */
{
    private GameObject tempObject;
    private UseItem useItem;

    public Canvas canvas;
    public SlotManager slotManager;
    

    private bool isOnDrag = false;
    private bool hasRaycastExecuted = false;

    public GameObject addedSlotItemList;
    private void Start()
    {
        useItem = GameObject.FindWithTag("Player").GetComponent<UseItem>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isOnDrag)
        {

            tempObject = Instantiate(eventData.pointerDrag, canvas.transform);
            
            TestMode.DebugLog("�۵���");
            isOnDrag = true;
        }
        tempObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TestMode.DebugLog("Ŭ��");
        bool isSlot = false;
        isOnDrag = false;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;


        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;


            // �巡�� end �Ѱ��� Slot�̸�
            if (hitObject.tag.Equals("Slot"))
            {
                if (hitObject.GetComponent<SlotState>().isOnItem)
                {
                    Destroy(tempObject);
                    return;
                }

                if (eventData.pointerDrag.GetComponent<ItemState>().isOn)
                {
                    tempObject.GetComponent<ItemState>().hitObject_SlotState.isOnItem = false;
                    eventData.pointerDrag.GetComponent<ItemState>().hitObject_SlotState.isOnItem = false;
                    if (slotManager.slotItemObjectByName.ContainsKey(tempObject.GetComponent<ItemState>().itemName))
                    {
                        hitObject.GetComponent<SlotState>().isOnItem = false;
                        
                        slotManager.slotItemObjectByName[tempObject.GetComponent<ItemState>().itemName].GetComponent<ItemState>().hitObject_SlotState.isOnItem = false;
                        slotManager.slotItemObjectByName.Remove(tempObject.GetComponent<ItemState>().itemName);
                        useItem.hotkeyByItemName.Remove(tempObject.GetComponent<ItemState>().hotKey);
                    }

                    Destroy(eventData.pointerDrag);
                }
                isSlot = true;
                hitObject.GetComponent<SlotState>().isOnItem = true;

                //�̹� ���Կ� �ִ»��¿��� �κ��丮���ִ� �������� �ٸ� �������� �ű涧 ������ �ִ� ������ isOnItem �� false�� ����
                if (slotManager.slotItemObjectByName.ContainsKey(tempObject.GetComponent<ItemState>().itemName))
                {
                    slotManager.slotItemObjectByName[tempObject.GetComponent<ItemState>().itemName].GetComponent<ItemState>().hitObject_SlotState.isOnItem = false;
                }
                    //hitObject.GetComponent<SlotState>().onItemName = tempObject.GetComponent<ItemState>().itemName;

                    tempObject.GetComponent<ItemState>().slotIndex = hitObject.GetComponent<SlotState>().slotIndex;
                tempObject.GetComponent<ItemState>().isOn = true;

                //0���� 1���� ��Ű ����
                tempObject.GetComponent<ItemState>().hotKey = tempObject.GetComponent<ItemState>().slotIndex != 9 ? tempObject.GetComponent<ItemState>().slotIndex + 49 : 48;

                tempObject.name = "Slot_"+tempObject.GetComponent<ItemState>().itemName;
                if (useItem.hotkeyByItemName.ContainsKey(tempObject.GetComponent<ItemState>().hotKey))
                {
                    useItem.hotkeyByItemName.Remove(tempObject.GetComponent<ItemState>().hotKey);
                }
                else
                {
                    useItem.hotkeyByItemName.Add(tempObject.GetComponent<ItemState>().hotKey, tempObject.GetComponent<ItemState>().itemName);
                }


                tempObject.transform.position = hitObject.transform.position;

                tempObject.transform.SetParent(addedSlotItemList.transform);
                //slot�� ���ں��� depth�� �ڷ� ���ߵ�
                //tempObject.transform.SetAsFirstSibling();

                if (slotManager.slotItemObjectByName.ContainsKey(tempObject.GetComponent<ItemState>().itemName))
                {
                    Destroy(slotManager.slotItemObjectByName[tempObject.GetComponent<ItemState>().itemName]);
                    slotManager.slotItemObjectByName.Remove(tempObject.GetComponent<ItemState>().itemName);
                }

                slotManager.slotItemObjectByName.Add(tempObject.GetComponent<ItemState>().itemName, tempObject);

                tempObject.GetComponent<ItemState>().hitObject_SlotState = hitObject.GetComponent<SlotState>();
                tempObject = null;
                
                
                return;
            }
        }
        if (!isSlot && eventData.pointerDrag.GetComponent<ItemState>().isOn)
        {
            useItem.hotkeyByItemName.Remove(tempObject.GetComponent<ItemState>().hotKey);
            tempObject.GetComponent<ItemState>().hitObject_SlotState.isOnItem = false;
            Destroy(tempObject);
            tempObject = null;
            Destroy(slotManager.slotItemObjectByName[eventData.pointerDrag.GetComponent<ItemState>().itemName]);
            slotManager.slotItemObjectByName.Remove(eventData.pointerDrag.GetComponent<ItemState>().itemName);
        }
        else if (!isSlot)
        {
            
            Destroy(tempObject);
            tempObject = null;
            TestMode.DebugLog("���� ���");
        }
        
        
    }


}
