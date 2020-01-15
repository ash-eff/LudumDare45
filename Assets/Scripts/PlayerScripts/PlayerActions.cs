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
        playerManager.rButtonGUI.SetActive(false);
        Item stealingItem = itemToSteal;
        stealingItem.canBePickedUp = false;      
        playerManager.PlayerOccupied = true;
        StartCoroutine(InstantiateNoise());
        interactBar.SetActive(true);
        float timeOfInteraction = playerManager.stealTime;
        interactBarFill.fillAmount = 0f;
        InvokeRepeating("InstantiateNoise", playerManager.stealTime, .25f);
        while (Input.GetKey(KeyCode.R))
        {
            timeOfInteraction -= Time.deltaTime;
            interactBarFill.fillAmount += Time.deltaTime / playerManager.stealTime;
    
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

    IEnumerator InstantiateNoise()
    {
        playerAudio.PlayAudio(playerAudio.steal);
        while (Input.GetKey(KeyCode.R) && playerManager.PlayerOccupied)
        {
            Instantiate(noisePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(playerManager.stealTime / 4);
        }
        playerAudio.StopAudio();
    }

    public void HackLock(bool b)
    {
        isHacking = b;
        playerManager.lockPad.gameObject.SetActive(b);
        StartCoroutine(HackLocks());
    }

    IEnumerator HackLocks()
    {
        playerManager.PlayerOccupied = true;
        while (isHacking)
        {
            playerManager.PlayerOccupied = true;
            if (playerManager.lockPad.isUnlocked)
            {
                playerManager.lockPad.currentLock.Unlock();
                playerManager.lockPad.gameObject.SetActive(false);
                isHacking = false;
            }

            yield return null;
        }

        playerManager.PlayerOccupied = false;
    }
}
