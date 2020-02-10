using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSurroundings : MonoBehaviour
{
    public List<IInteractable> interactableList = new List<IInteractable>();
    public IInteractable currentlyTouching;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Raycast Values")]
    [SerializeField] private int numberOfRays = 3;
    [SerializeField] private float raysWidth = .5f;
    [SerializeField] private float raysHeight = .5f;
    [SerializeField] private float xRayCastLength = .4f;
    [SerializeField] private float yRayCastLength = .05f;
    [SerializeField] private float yRayOffsetFromGround = .8f;
    [Space(2)]

    public TextMeshProUGUI interactText;

    private PlayerManager playerManager;

    private Vector2[] directionsVertical = { Vector2.up, Vector2.down };
    private Vector2[] directionsHorizontal = { Vector2.right, Vector2.left };

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        StartCoroutine(CheckForObjects());
    }

    IEnumerator CheckForObjects()
    {
        while (true)
        {
            interactableList = new List<IInteractable>();
            interactableList = CheckForObjectsUpAndDown();

            if (interactableList.Count == 0)
            {
                interactableList = CheckForObjectsLeftAndRight();
            }

            if (interactableList.Count > 0)
            {
                WhatIsThePlayerTouching(interactableList);
            }
            else
            {
                currentlyTouching = null;
            }

            yield return null;
        }
    }

    private void WhatIsThePlayerTouching(List<IInteractable> theList)
    {
        IInteractable currentInteractable = theList[0];
        int numberOfTimes = 0;
        if (theList.Count > 1)
        {
            foreach (IInteractable interactable in theList)
            {
                currentInteractable = interactable;
                numberOfTimes = 0;
                for (int i = 0; i < theList.Count; i++)
                {
                    if (currentInteractable == theList[i])
                    {
                        numberOfTimes++;
                    }
                }
                if (numberOfTimes > (theList.Count / 2))
                {
                    break;
                }
            }
        }

        currentlyTouching = currentInteractable;
    }

    private List<IInteractable> CheckForObjectsUpAndDown()
    {
        List<IInteractable> hitUpDown = new List<IInteractable>();
        float widthByRays = raysWidth / (numberOfRays - 1);
        foreach (Vector2 dir in directionsVertical)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                Vector2 originPosition = (Vector2)transform.position - new Vector2(raysWidth / 2, yRayOffsetFromGround) + (new Vector2(widthByRays, 0) * i);
                Debug.DrawRay(originPosition, dir * yRayCastLength, Color.red);
                RaycastHit2D hit = Physics2D.Raycast(originPosition, dir, yRayCastLength, obstacleLayer);
                if (hit)
                {
                    if (hit.collider.transform.GetComponentInParent<IInteractable>() != null)
                    {
                        hitUpDown.Add(hit.collider.transform.GetComponentInParent<IInteractable>());
                    }             
                }
            }
        }

        return hitUpDown;
    }

    private List<IInteractable> CheckForObjectsLeftAndRight()
    {
        List<IInteractable> hitLeftRight = new List<IInteractable>();
        float heightByRays = raysHeight / (numberOfRays - 1);
        foreach (Vector2 dir in directionsHorizontal)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                Vector2 originPosition = (Vector2)transform.position - new Vector2(0, raysHeight / 2 + yRayOffsetFromGround) + (new Vector2(0, heightByRays) * i);
                Debug.DrawRay(originPosition, dir * xRayCastLength, Color.green);
                RaycastHit2D hit = Physics2D.Raycast(originPosition, dir, xRayCastLength, obstacleLayer);
                if (hit)
                {
                    if (hit.collider.transform.GetComponentInParent<IInteractable>() != null)
                    {
                        hitLeftRight.Add(hit.collider.transform.GetComponentInParent<IInteractable>());
                    }
                }
            }
        }

        return hitLeftRight;
    }
}
