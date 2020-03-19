using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ash.StateMachine;

namespace Ash.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void OpenTerminal();
        public static event OpenTerminal OnTerminalOpen;
        public static event OpenTerminal OnTerminalClose;

        public Color baseColor;
        public Color stealthColor;
        public bool isStealthed;
        public GameObject stealthIcon;
        public LayerMask layersToCheck;
        public StateMachine<PlayerController> stateMachine;
        public static PlayerController player;
        public TerminalOS terminalOS;

        public Vector3 cameraTarget;

        public bool spotted;
        public int timesSpotted;
        public GameObject warningFlash;
        public GameObject alertFlash;

        #region Exposed Variables and Components
        [Header("Layer Masks")]
        public LayerMask allObstacleLayers;
        public LayerMask ventLayer;
        public LayerMask containerLayer;
        public LayerMask lightsLayer;
        public LayerMask lightVisLayer;
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
        public CanvasGroup terminalGUI;
        public GameObject stealthBar;
        public GameObject feet;
        public GameObject head;
        public GameObject cursor;
        [SerializeField] private SpriteRenderer playerSprite;
        [Space(2)]
        #endregion

        #region Private Variables and Components
        public List<GameObject> interactableList = new List<GameObject>();
        public GameObject currentlyTouching;
        public SingleLight[] lightsInArea;
        public GameController gameController;

        private Vector3 movement;
        private Vector2 direction;
        private bool canMove;
        private bool canAttack = true;
        private LayerMask currentlyChecking;
        public Animator spriteAnim;
        private Rigidbody2D rb2d;
        public LineRenderer circle;
        public int circleSegments;
        #endregion

        public Vector3 Movement { get { return movement; } }
        public Vector3 GetCursorPos { get { return cursor.transform.position; } }
        public float GetSpriteDirection { get { return playerSprite.transform.localScale.x; } }
        public bool CanMove { get { return canMove; } set { canMove = value; } }
        public GameObject CurrentlyTouching { get { return currentlyTouching; } }
        public float RunSpeed { get { return runSpeed; } }
        public float CrawlSpeed { get { return crawlSpeed; } }

        private void Awake()
        {
            gameController = FindObjectOfType<GameController>();
            rb2d = GetComponent<Rigidbody2D>();
            spriteAnim = GetComponent<Animator>();
            terminalOS = FindObjectOfType<TerminalOS>();
            player = this;
            stateMachine = new StateMachine<PlayerController>(player);
            stateMachine.ChangeState(BaseState.Instance);

        }

        private void Start()
        {
            lightsInArea = gameController.currentRoom.GetComponentsInChildren<SingleLight>();
            cameraTarget = transform.position;
            circle.positionCount = circleSegments + 1;
            circle.useWorldSpace = false;
            CreatePoints();
            circle.gameObject.SetActive(false);
        }

        private void Update() => stateMachine.Update();
        private void FixedUpdate() => stateMachine.FixedUpdate();

        public void SetCameraTarget(Vector2 _pos)
        {
            cameraTarget = _pos;
        }

        public void PlayerInput()
        {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (currentlyTouching != null)
                if (Input.GetKeyDown(KeyCode.E))
                    InteractWithObject();
                if (Input.GetKeyDown(KeyCode.R))
                    SecondaryInteractWithObject();

            if (Input.GetKeyDown(KeyCode.T))
                HandTerminal();
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

        public void IdleSprite()
        {
            spriteAnim.SetBool("Moving", false);
            spriteAnim.SetBool("RunRightLeft", false);
            spriteAnim.SetBool("RunUp", false);
            spriteAnim.SetBool("RunDown", false);
        }

        public void SetPlayerSpriteVisible(bool isVisible)
        {
            playerSprite.gameObject.SetActive(isVisible);
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

        public void IgnoreCollisionWithObstacles(bool _ignore)
        {
            Physics2D.IgnoreLayerCollision(8, 9, _ignore);
        }

        public void CheckForStealth()
        {
            List<SingleLight> lights = new List<SingleLight>();
            lights = CheckForLightsHittingPlayer();
            if(lights.Count == 0)
            {
                stealthBar.SetActive(true);
                isStealthed = true;
                stealthIcon.SetActive(true);
                //playerSprite.color = stealthColor;
            }
            else
            {
                stealthBar.SetActive(false);
                isStealthed = false;
                stealthIcon.SetActive(false);
                //playerSprite.color = baseColor;
            }
        }

        public void WarningAlertIndicator()
        {
            if(timesSpotted == 1)
            {
                warningFlash.SetActive(true);
            }
            else if(timesSpotted > 1)
            {
                warningFlash.SetActive(false);
                alertFlash.SetActive(true);
            }             
        }

        public void SwapRooms()
        {
            stateMachine.ChangeState(RoomSwapState.Instance);
        }

        #region helper functions
        private void InteractWithObject()
        {
            if (currentlyTouching.tag == "Vent")
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
                if (stateMachine.currentState == BaseState.Instance)
                    stateMachine.ChangeState(KnockState.Instance);
            }
            //if (currentlyTouching.tag == "Hackable")
            //{
            //    if (stateMachine.currentState == BaseState.Instance)
            //        stateMachine.ChangeState(HackState.Instance);
            //    else
            //        stateMachine.ChangeState(BaseState.Instance);
            //}
            if (currentlyTouching.tag == "Exit")
            {
                if (stateMachine.currentState == BaseState.Instance || stateMachine.currentState == PeakState.Instance)
                {
                    currentlyTouching.GetComponent<RoomExit>().SwapRooms();
                }
            }
        }

        private void SecondaryInteractWithObject()
        {
            if (currentlyTouching.tag == "Exit")
            {
                if (stateMachine.currentState == BaseState.Instance)
                    stateMachine.ChangeState(PeakState.Instance);
                else
                    stateMachine.ChangeState(BaseState.Instance);
            }
        }

        private void HandTerminal()
        {
            if (stateMachine.currentState == HackState.Instance || stateMachine.currentState == TerminalState.Instance)
                stateMachine.ChangeState(BaseState.Instance);
            else if(terminalOS.workingComputer != null)
            {
                if (!terminalOS.workingComputer.accessGranted)
                {
                    stateMachine.ChangeState(HackState.Instance);
                }
                else
                {
                    stateMachine.ChangeState(TerminalState.Instance);
                }
            }
            else
            {
                stateMachine.ChangeState(TerminalState.Instance);
            }

            Debug.Log(stateMachine.currentState);
        }

        public void AccessComputer(Computer computer)
        {
            player.terminalOS.workingComputer = computer;
            HandTerminal();
        }

        public void TargetRobots(bool b)
        {
            circle.gameObject.SetActive(b);
            if (b)
            {
                OnTerminalOpen?.Invoke();
            }
            else
            {
                OnTerminalClose?.Invoke();
            }
        }

        public void CreatePoints()
        {
            float x;
            float y;
            float z = 0f;

            float angle = 20f;

            for (int i = 0; i < (circleSegments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * 10;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * 10;

                circle.SetPosition(i, new Vector3(x, y, z));

                angle += (360f / circleSegments);
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

        private List<SingleLight> CheckForLightsHittingPlayer()
        {
            List<SingleLight> lightsHittingPlayer = new List<SingleLight>();
            foreach (SingleLight light in lightsInArea)
            {
                if (light.isActive)
                {
                    Vector3 dirFeet = light.transform.position - feet.transform.position;
                    Vector3 dirHead = light.transform.position - head.transform.position;
                    float dstFeet = dirFeet.magnitude;
                    float dstHead = dirHead.magnitude;

                    if(dstFeet < light.lightRadius || dstHead < light.lightRadius)
                    {
                        RaycastHit2D lineFromLightToFeet = Physics2D.Raycast(feet.transform.position, dirFeet, dstFeet, lightVisLayer);
                        RaycastHit2D lineFromLightToHead = Physics2D.Raycast(head.transform.position, dirHead, dstHead, lightVisLayer);
                        if (!lineFromLightToFeet || !lineFromLightToHead)
                        {
                            lightsHittingPlayer.Add(light);
                            Debug.DrawLine(feet.transform.position, light.transform.position);
                            Debug.DrawLine(head.transform.position, light.transform.position);
                        }
                    }
                }
            }

            return lightsHittingPlayer;
        }
        #endregion
    }
}