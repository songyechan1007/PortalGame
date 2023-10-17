using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{

    public Dictionary<string,int> itemCount = new Dictionary<string,int>();
    public Dictionary<string, GameObject> itemObjectByName = new Dictionary<string, GameObject>();
    public Dictionary<string,Sprite> itemPictureList = new Dictionary<string,Sprite>();
    public List<GameObject> itemSlots;
    public Text itemCountText;

    public int hasItemCount = 0;
    public int hasItemMaxCount = 16;

    private void Start()
    {
        hasItemMaxCount = itemSlots.Count;
    }
}
