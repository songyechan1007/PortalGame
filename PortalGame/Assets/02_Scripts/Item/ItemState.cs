using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : MonoBehaviour
{
    public int hotKey = -1;
    public string itemName;

    public int slotIndex = -1;
    public bool isOn = false;

    public SlotState hitObject_SlotState;

}
