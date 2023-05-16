using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.TextCore;

public class PlayerController : MonoBehaviour
{
    public ClassData classData;
    public string myBullet;
    public IPlayer player;
    private Vector3 playerVelocity;
    private ScreenBorder screenBorder;

    private CharacterController controller;
    private bool isGrounded;
    private GameManager gameManager;

    private PlayerConfiguration _playerConfig;
    private PlayerInput _playerInput;
    private InputManager inputManager;

    private Transform mainCamTransform;
    private Coroutine shootCorutine;
    public bool canMove, canShoot, canUseItem = true;

    private float tempHealth, tempScore;

    private string deathEffect = "DeathEffect";


    #region Item and Inventory Stuff
    //[HideInInspector]
    public PlayerInventory m_inventory;
    private ItemEnum itemToUse = ItemEnum.Potion;
    #endregion

    #region Input
    private PlayerInput _input;
    private Vector2 movement;
    private bool hasShot, usedItem = false;
    public MultiplayerEventSystem m_eventSystem;
    #endregion

    private void OnEnable()
    {
        myBullet = "Bullet/" + classData.ClassType.ToString();
    }
    private void Start()
    {
        player = new Player(classData);
        gameManager = GameManager.Instance;
        _playerInput = GetComponent<PlayerInput>();
        _playerConfig = PlayerManager.Instance.playerConfigs[_playerInput.playerIndex];
        m_eventSystem = GetComponentInChildren<MultiplayerEventSystem>();
        mainCamTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        screenBorder = new ScreenBorder();
        _input = GetComponentInChildren<PlayerInput>();

        classData.ResetValuesToDefault();

        UIManager.Instance.HandleScoreUI(classData);
        UIManager.Instance.HandleHealthUI(classData);

        switch (classData.ClassType)
        {
            case ClassEnum.Warrior:
                classData.MyUIInventory = UIManager.Instance.warriorInventory;
                break;
            case ClassEnum.Valkyrie:
                classData.MyUIInventory = UIManager.Instance.valkyrieInventory;

                break;
            case ClassEnum.Wizard:
                classData.MyUIInventory = UIManager.Instance.wizzardInventory;

                break;
            case ClassEnum.Elf:
                classData.MyUIInventory = UIManager.Instance.elfInventory;

                break;
            default:
                break;
        }
        HandlePlayerGFX();

        StartCoroutine(ReduceHealthOverTime());

    }

    private void Update()
    {
        if (canMove)
            HandleMovement();
        if (canShoot)
            HandleShoot();
        if (canUseItem)
            HandleItem();


        ////Debug.Log(screenBorder.IsOutside(transform, controller.radius, controller.radius));
    }

    private void LateUpdate()
    {
        transform.position = screenBorder.ClampToInside(transform, controller.radius, controller.radius);
    }

    public void HandlePlayerGFX()
    {
        //If there is already a GFX destroy it
        if(transform.childCount > 1)
            for (int i = 1; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        GameObject gfx = Instantiate(classData.PlayerPrefab, transform.position, Quaternion.identity, transform);
        this.gameObject.name = classData.PlayerPrefab.name;
    }

    #region Player Input Functions
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        hasShot = context.action.triggered;
    }
    public void OnItem(InputAction.CallbackContext context)
    {
        usedItem = context.action.triggered;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        UIManager.Instance.isPaused = !UIManager.Instance.isPaused;
        UIManager.Instance._eventSystem = m_eventSystem;
        UIManager.Instance.state = UIManager.Instance.isPaused ? CanvasState.Pause : CanvasState.Level;
        EventBus.Publish(EventType.UI_CHANGED);

        if(UIManager.Instance.isPaused)
            NaratorManager.Instance.audioSource.Pause();
        else
            NaratorManager.Instance.audioSource.UnPause();

    }

    /// <summary>
    /// Sets players <paramref name="canMove"/>,<paramref name="canShoot"/> and, <paramref name="canUseItem"/> to <paramref name="allowInput"/>
    /// </summary>
    /// <param name="allowInput"></param>
    public void AllowInput(bool allowInput)
    {
        canMove= allowInput;
        canShoot= allowInput;
        canUseItem= allowInput;
    }

    #endregion


    public void HandleMyInventory(GameObject inventoryGrp, GameObject _item)
    {
        if(m_inventory.myItems.Count != inventoryGrp.transform.childCount)
        {
            GameObject item = ObjectPooler.Instance.GetPooledObject(_item.tag);
        }
    }

    #region Constant Functions
    private void HandleMovement()
    {
        //Get grounded
        isGrounded = controller.isGrounded;

        //Get input 
        //Get Vector3 from input Vector2
        playerVelocity = new Vector3(movement.x, 0, movement.y);
        //Make sure movement takes in camera rotation
        playerVelocity = mainCamTransform.TransformDirection(movement);
        playerVelocity.y = -1f;

        //Move character from move Vecotr3
        controller.Move(playerVelocity * Time.deltaTime * classData.CurSpeed);

        //Rotate towards movement
        Vector3 lookDirection = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        if (playerVelocity != Vector3.zero && lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void HandleShoot()
    {
        if (hasShot)
        {
            canShoot = false;
            canMove = false;
            StartCoroutine(ShootOnce());
        }
    }

    private IEnumerator ShootOnce()
    {

        //GameObject bullet = Instantiate(classData.ProjectilePrefab, new Vector3( transform.position.x, transform.position.y + (controller.height / 2), transform.position.z), Quaternion.LookRotation(transform.forward));
        GameObject bullet = ObjectPooler.Instance.GetPooledObject(myBullet);
        Debug.Log(myBullet + bullet.name);
        if (bullet != null)
        {
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y + (controller.height / 2), transform.position.z);
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward);
            bullet.GetComponent<BulletController>().m_player = this;
            bullet.SetActive(true);


        }
        yield return new WaitForSeconds(classData.ShootTime);
        canMove = true;

        //Wait till myBullet is off the screen before I can shoot again
        yield return new WaitUntil(() => bullet.gameObject.activeInHierarchy == false);
        canShoot = true;
        yield return null;
    }

    private void HandleItem()
    {
        if (usedItem)
        {
            usedItem = false;
            //Check which item to use
            switch (itemToUse)
            {
                case ItemEnum.Potion:
                    bool hasPotions = m_inventory.myItems.Any(item => item.ItemType == ItemEnum.Potion);
                    if (hasPotions)
                    {
                        //Use potion
                        GameManager.Instance.UsePotion(this);

                        //Remove from inventory
                        GameObject potion = null;
                        for (int i = 0; i < classData.MyUIInventory.transform.childCount; i++)
                        {
                            Transform child = classData.MyUIInventory.transform.GetChild(i);
                            if (child.gameObject.name == "PotionIcon(Clone)")
                            {
                                potion = child.gameObject;
                            }
                        }
                        potion.SetActive(false);
                        potion.transform.SetParent(ObjectPooler.Instance.transform);
                        RemoveItemFromInventory(itemToUse);
                    }

                    break;
                case ItemEnum.Key:
                    bool hasKeys = m_inventory.myItems.Any(item => item.ItemType == ItemEnum.Key);
                    if (hasKeys)
                    {
                        //Use key
                        UseKey();

                        //Remove from inventory
                        GameObject key = null;
                        for (int i = 0; i < classData.MyUIInventory.transform.childCount; i++)
                        {
                            Transform child = classData.MyUIInventory.transform.GetChild(i);
                            if (child.gameObject.name == "KeyIcon(Clone)")
                            {
                                key = child.gameObject;
                            }
                        }
                        key.SetActive(false);
                        key.transform.SetParent(ObjectPooler.Instance.transform);
                        RemoveItemFromInventory(itemToUse);
                        //InventoryManager.Instance.RemoveItemFromInventory(this, key.GetComponent<KeyItem>().data);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public void HandleItem(PlayerController player)
    {

        //Check which item to use
        switch (itemToUse)
        {
            case ItemEnum.Potion:
                bool hasPotions = m_inventory.myItems.Any(item => item.ItemType == ItemEnum.Potion);
                if (hasPotions)
                {
                    //Remove from inventory
                    GameObject potion = null;
                    for (int i = 0; i < classData.MyUIInventory.transform.childCount; i++)
                    {
                        Transform child = classData.MyUIInventory.transform.GetChild(i);
                        if (child.gameObject.name == "PotionIcon(Clone)")
                        {
                            potion = child.gameObject;
                        }
                    }
                    potion.SetActive(false);
                    potion.transform.SetParent(ObjectPooler.Instance.transform);
                    RemoveItemFromInventory(itemToUse);
                }

                break;
            case ItemEnum.Key:
                bool hasKeys = m_inventory.myItems.Any(item => item.ItemType == ItemEnum.Key);
                if (hasKeys)
                {
                    //Remove from inventory
                    GameObject key = null;
                    for (int i = 0; i < classData.MyUIInventory.transform.childCount; i++)
                    {
                        Transform child = classData.MyUIInventory.transform.GetChild(i);
                        if (child.gameObject.name == "KeyIcon(Clone)")
                        {
                            key = child.gameObject;
                        }
                    }
                    key.SetActive(false);
                    key.transform.SetParent(ObjectPooler.Instance.transform);
                    RemoveItemFromInventory(itemToUse);
                    //InventoryManager.Instance.RemoveItemFromInventory(this, key.GetComponent<KeyItem>().data);
                }
                break;
            default:
                break;
        }
        
    }
    /// <summary>
    /// Used for removing a specific item from inventory
    /// </summary>
    /// <param name="_item"></param>
    private void RemoveItemFromInventory(ItemEnum _item)
    {
        if (m_inventory.myItems.Count > 0)
        {
            for (int i = 0; i < m_inventory.myItems.Count; i++)
            {
                //If not the right item type go to next
                if (m_inventory.myItems[i].ItemType != _item)
                    continue;
                //If is right item type
                else if (m_inventory.myItems[i].ItemType == _item)
                {
                    
                    //Remove it from my inventory
                    m_inventory.myItems.RemoveAt(i);
                    m_inventory.uiItems.RemoveAt(i);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// Used for removing all items from inventory
    /// </summary>
    public void RemoveItemFromInventory()
    {
        if (m_inventory.myItems.Count > 0)
        {
            for (int i = 0; i < m_inventory.myItems.Count; i++)
            {
                //Remove it from my inventory
                m_inventory.myItems.RemoveAt(i);
                m_inventory.uiItems.RemoveAt(i);
            }
            //Clears inventory manager
        }
            InventoryManager.Instance.ClearPlayerInventory(this);
    }


    private void UseKey()
    {

    }

    private IEnumerator ReduceHealthOverTime()
    {
        while (classData.CurHealth > 0)
        {
            yield return new WaitForSeconds(1f);
            classData.CurHealth--;
            UIManager.Instance.HandleHealthUI(classData);

        }
        yield return null;
        if(classData.CurHealth <= 0)
        {
            //Spawn Particle
            GameObject effect = ObjectPooler.Instance.GetPooledObject(deathEffect);
            if (effect != null)
            {
                effect.transform.position = this.transform.position;
                effect.transform.rotation = Quaternion.identity;
                effect.SetActive(true);
            }
            //Resets players stats to base and then destorys player
            classData.ResetValuesToDefault();
            RemoveItemFromInventory();
            for (int i = 0; i < PlayerManager.Instance.playerConfigs.Count; i++)
            {
                if (PlayerManager.Instance.playerConfigs[i].PlayerParent.GetComponent<PlayerController>() == this)
                {
                    //Clears player lists
                    PlayerManager.Instance.playerConfigs.RemoveAt(i);
                    //PlayerManager.Instance.usedClasses.RemoveAt(i);
                    m_inventory = null;
                    Destroy(gameObject);
                }
            }
            EventBus.Publish(EventType.PLAYER_LEFT);
            UIManager.Instance.HandleHealthUI(classData);
        }
        yield return null;

    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        IFloorItem floorItem = other.gameObject.GetComponent<IFloorItem>();
        if (floorItem != null)
        {
            floorItem.HandlePickup(this);
            UIManager.Instance.HandleScoreUI(classData);
            UIManager.Instance.HandleHealthUI(classData);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IFloorItem floorItem = hit.gameObject.GetComponent<IFloorItem>();
        if (floorItem != null)
        {
            floorItem.HandlePickup(this);
            UIManager.Instance.HandleScoreUI(classData);
            UIManager.Instance.HandleHealthUI(classData);
        }
    }
}
