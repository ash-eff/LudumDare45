using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public enum StoreType { Clothing, General, Sporting, Electronic, Children, Pets, Entertainment, Utility }
    public StoreType storeType;
    public string storeName;
    public int numberOfItems = 1;
    public ValuableItem clothingItem;
    public List<ValuableItem> storeItems = new List<ValuableItem>();
    public ItemLocation[] itemLoactions;

    public void Awake()
    {
        itemLoactions = GetComponentsInChildren<ItemLocation>();

        switch (storeType)
        {
            case StoreType.Clothing:
                ManageItems(numberOfItems, clothingItem);
                break;

            case StoreType.General:
                Debug.Log(storeType);
                break;

            case StoreType.Sporting:
                Debug.Log(storeType);
                break;

            case StoreType.Electronic:
                Debug.Log(storeType);
                break;

            case StoreType.Children:
                Debug.Log(storeType);
                break;

            case StoreType.Pets:
                Debug.Log(storeType);
                break;

            case StoreType.Entertainment:
                Debug.Log(storeType);
                break;

            default:
                break;
        }
    }

    void ManageItems(int numOfItemsToAdd, ValuableItem item)
    {
        for(int i = 0; i < numberOfItems; i++)
        {
            Vector3 pos = itemLoactions[Random.Range(0, itemLoactions.Length)].transform.position;
            ValuableItem itemToInst = Instantiate(item, pos, Quaternion.identity);
            storeItems.Add(itemToInst);
        }
    }
}
