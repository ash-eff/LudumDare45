using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ash.StateMachine;

namespace Ash.PlayerController
{
    public class PlayerController : MonoBehaviour
    {

        public LayerMask layersToCheck;
        public StateMachine<PlayerController> stateMachine;
        public static PlayerController player;

        #region Exposed Variables and Components
        [Header("Layer Masks")]
        public LayerMask allObstacleLayers;
        public LayerMask ventLayer;
        public LayerMask containerLayer;
        [Space(2)]

        [Header("Raycast Values")]
        [SerializeField] private int numberOfRays = 3;
        [SerializeField] private float raysWidth = .5f;
        [SerializeField] private float raysHeight = .5f;
        [SerializeField] private float xRayCastLength = .4f;
        [SerializeField] private float yRayCastLength = .05f;
        [SerializeField] private float yRayOffsetFromGround = .8f;
        [Space(2)]

        [Header("Player Speeds")]
        [SerializeField] private float runSpeed = 6;
        [SerializeField] private float crawlSpeed = 2;
        [SerializeField] private float dashSpeed = 50;
        [Space(2)]

        [Header("Components")]
        public TextMeshProUGUI interactText;
        public GameObject ventLight;
        public Noise noisePrefab;
        //[SerializeField] private GameObject cursor;
        [SerializeField] private SpriteRenderer playerSprite;
        [Space(2)]
        #endregion

        #region Private Variables and Components
        public List<GameObject> interactableList = new List<GameObject>();
        public GameObject currentlyTouching;

        private Vector3 movement;
        private Vector2 direction;
        private bool canMove;
        private LayerMask currentlyChecking;
        public Animator spriteAnim;
        private Rigidbody2D rb2d;
        #endregion

        public Vector3 Movement { get { return movement; } }
        //public Vector3 GetCursorPos { get { return cursor.transform.position; } }
        public bool CanMove { get { return canMove; } set { canMove = value; } }
        public GameObject CurrentlyTouching { get { return currentlyTouching; } }
        public float RunSpeed { get { return runSpeed; } }
        public float CrawlSpeed { get { return crawlSpeed; } }

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            spriteAnim = GetComponent<Animator>();
            player = this;
            stateMachine = new StateMachine<PlayerController>(this);
            stateMachine.ChangeState(BaseState.Instance);
        }

        private void Update() => stateMachine.Update();
        private void FixedUpdate() => stateMachine.FixedUpdate();

        public void PlayerInput()
        {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (currentlyTouching != null)
                if (Input.GetKeyDown(KeyCode.E))
                    InteractWithObject();
        }

        public void SetPlayerVelocity(float _atSpeed, bool allowMovement)
        {
            rb2d.velocity = allowMovement ? new Vector2(movement.x, movement.y) * _atSpeed : Vector2.zero;
        }

        public void SetSpriteDirection()
        {
            if(movement.x != 0)
            {
                playerSprite.transform.localScale = new Vector2(movement.x, 1f);
            }
        }

        public void SetSpriteAnimation()
        {
            if (movement.x != 0)
            {
                spriteAnim.SetBool("Moving", true);
                spriteAnim.SetBool("RunRightLeft", true);
                spriteAnim.SetBool("RunUp", false);
                spriteAnim.SetBool("RunDown", false);
            }
            else if (movement.y > 0)
            {
                spriteAnim.SetBool("Moving", true);
                spriteAnim.SetBool("RunUp", true);
                spriteAnim.SetBool("RunRightLeft", false);
                spriteAnim.SetBool("RunDown", false);
            }
            else if (movement.y < 0)
            {
                spriteAnim.SetBool("Moving", true);
                spriteAnim.SetBool("RunDown", true);
                spriteAnim.SetBool("RunRightLeft", false);
                spriteAnim.SetBool("RunUp", false);
            }
            else
            {
                spriteAnim.SetBool("Moving", false);
                spriteAnim.SetBool("RunRightLeft", false);
                spriteAnim.SetBool("RunUp", false);
                spriteAnim.SetBool("RunDown", false);
            }
        }

        public void SetPlayerSpriteVisible(bool isVisible)
        {
            playerSprite.enabled = isVisible;
        }

        public void CheckForObjectsOnLayer(LayerMask _layerMask)
        {
            layersToCheck = _layerMask;
            interactableList = new List<GameObject>();
            interactableList = CheckForObjectsUpAndDown(layersToCheck);

            if (interactableList.Count == 0)
            {
                interactableList =CheckForObjectsLeftAndRight(layersToCheck);
            }

            if (interactableList.Count > 0)
            {
                currentlyTouching = WhatIsThePlayerTouching(interactableList);
                interactText.text = currentlyTouching.GetComponentInParent<IInteractable>().BeingTouched();
            }
            else
            {
                currentlyTouching = null;
                interactText.text = "";
            }
        }

        #region helper functions
        private void InteractWithObject()
        {
            if(currentlyTouching.tag == "Vent")
            {
                if (stateMachine.currentState == VentState.Instance)
                    stateMachine.ChangeState(BaseState.Instance);
                else
                    stateMachine.ChangeState(VentState.Instance);
            }
            if (currentlyTouching.tag == "Locker")
            {
                if (stateMachine.currentState == HideState.Instance)
                    stateMachine.ChangeState(BaseState.Instance);
                else
                    stateMachine.ChangeState(HideState.Instance);
            }
            if (currentlyTouching.tag == "Wall")
            {
                if(stateMachine.currentState == BaseState.Instance)
                    stateMachine.ChangeState(KnockState.Instance);
            }
            if (currentlyTouching.tag == "Door")
            {
                // door state
            }
        }

        private Vector2[] directionsVertical = { Vector2.up, Vector2.down };
        private Vector2[] directionsHorizontal = { Vector2.right, Vector2.left };

        private GameObject WhatIsThePlayerTouching(List<GameObject> theList)
        {
            GameObject currentInteractable = theList[0];
            int numberOfTimes = 0;
            if (theList.Count > 1)
            {
                foreach (GameObject interactable in theList)
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

            return currentInteractable;
        }

        private List<GameObject> CheckForObjectsUpAndDown(LayerMask _layersToCheck)
        {
            List<GameObject> hitUpDown = new List<GameObject>();
            float widthByRays = raysWidth / (numberOfRays - 1);
            foreach (Vector2 dir in directionsVertical)
            {
                for (int i = 0; i < numberOfRays; i++)
                {
                    Vector2 originPosition = (Vector2)transform.position - new Vector2(raysWidth / 2, yRayOffsetFromGround) + (new Vector2(widthByRays, 0) * i);
                    Debug.DrawRay(originPosition, dir * yRayCastLength, Color.red);
                    RaycastHit2D hit = Physics2D.Raycast(originPosition, dir, yRayCastLength, _layersToCheck);
                    if (hit)
                    {
                        if (hit.collider.gameObject != null)
                        {
                            hitUpDown.Add(hit.collider.gameObject);
                        }
                    }
                }
            }

            return hitUpDown;
        }

        private List<GameObject> CheckForObjectsLeftAndRight(LayerMask _layersToCheck)
        {
            List<GameObject> hitLeftRight = new List<GameObject>();
            float heightByRays = raysHeight / (numberOfRays - 1);
            foreach (Vector2 dir in directionsHorizontal)
            {
                for (int i = 0; i < numberOfRays; i++)
                {
                    Vector2 originPosition = (Vector2)transform.position - new Vector2(0, raysHeight / 2 + yRayOffsetFromGround) + (new Vector2(0, heightByRays) * i);
                    Debug.DrawRay(originPosition, dir * xRayCastLength, Color.green);
                    RaycastHit2D hit = Physics2D.Raycast(originPosition, dir, xRayCastLength, _layersToCheck);
                    if (hit)
                    {
                        if (hit.collider.gameObject != null)
                        {
                            hitLeftRight.Add(hit.collider.gameObject);
                        }
                    }
                }
            }

            return hitLeftRight;
        }
        #endregion
    }
}