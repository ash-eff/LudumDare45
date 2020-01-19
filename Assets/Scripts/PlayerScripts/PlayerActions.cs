using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    public GameObject interactBar;
    public Image interactBarFill;
    public GameObject noisePrefab;
    private PlayerManager playerManager;
    private PlayerInventory playerInventory;
    private PlayerAudio playerAudio;

    public bool isHacking;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAudio = GetComponentInChildren<PlayerAudio>();
    }

    public void ThrowItem(Item itemToThrow, Vector2 toPos)
    {
        if (!itemToThrow.spr.enabled)
        {
            itemToThrow.startPos = transform.position;
            itemToThrow.endPos = toPos;
            StartCoroutine(itemToThrow.ThrowItem());
        }
    }

    public IEnumerator StealItem(Item itemToSteal)
    {        
        Item stealingItem = itemToSteal;
        stealingItem.canBePickedUp = false;      
        playerManager.PlayerOccupied = true;
        StartCoroutine(InstantiateNoise());
        interactBar.SetActive(true);
        float timeOfInteraction = playerManager.StealTime;
        interactBarFill.fillAmount = 0f;
        InvokeRepeating("InstantiateNoise", playerManager.StealTime, .25f);
        while (Input.GetKey(KeyCode.Alpha3))
        {
            playerManager.PlayerOccupied = true;
            timeOfInteraction -= Time.deltaTime;
            interactBarFill.fillAmount += Time.deltaTime / playerManager.StealTime;
    
            if (timeOfInteraction <= 0)
            {
                stealingItem.alreadyStolen = true;
                playerInventory.AddItemToInventory(stealingItem);
                break;
            }
          
            yield return null;
        }

        stealingItem = null;
        playerManager.PlayerOccupied = false;      
        interactBar.SetActive(false);
        interactBarFill.fillAmount = 0f;
    }

    public IEnumerator Knock()
    {
        Instantiate(noisePrefab, transform.position, Quaternion.identity);
        playerAudio.PlayAudio(playerAudio.knock);
        yield return new WaitForSeconds(1f);
        playerManager.IsKnocking = false;
    }

    IEnumerator InstantiateNoise()
    {
        playerAudio.PlayAudio(playerAudio.steal);
        while (Input.GetKey(KeyCode.Alpha3) && playerManager.PlayerOccupied)
        {
           
            Instantiate(noisePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(playerManager.StealTime / 4);
        }
        playerAudio.StopAudio();
    }

    public IEnumerator HackLocks()
    {
        isHacking = true;
        playerManager.PlayerOccupied = true;
        float timer = 5;
        while (isHacking)
        {
            timer -= Time.deltaTime;
            playerManager.PlayerOccupied = true;
            if (timer < 0)
            {
                playerManager.currentLock.Unlock();
                isHacking = false;
            }

            yield return null;
        }

        playerManager.PlayerOccupied = false;
    }


}
