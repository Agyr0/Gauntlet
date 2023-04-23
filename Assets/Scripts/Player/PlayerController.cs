using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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
    #region Item and Inventory Stuff
    //[HideInInspector]
    public PlayerInventory m_inventory;
    private ItemEnum itemToUse = ItemEnum.Potion;
    #endregion

    #region Input
    private Vector2 movement;
    private bool hasShot, usedItem = false;
    #endregion

    private void Start()
    {
        gameManager = GameManager.Instance;
        _playerInput = GetComponent<PlayerInput>();
        _playerConfig = PlayerManager.Instance.playerConfigs[_playerInput.playerIndex];

        mainCamTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        player = new Player(classData);
        screenBorder = new ScreenBorder();

        if (classData.CurHealth != 700)
            classData.CurHealth = 700;

        myBullet = "Bullet/" + classData.ClassType.ToString();
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

    #endregion


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
                    //Use potion
                    UsePotion(false);

                    //Remove from inventory
                    RemoveItemFromInventory(itemToUse);
                    break;
                case ItemEnum.Key:
                    //Use key
                    UseKey();

                    //Remove from inventory
                    RemoveItemFromInventory(itemToUse);
                    break;
                default:
                    break;
            }
        }
    }
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
                    Debug.Log("Removed Potion from inventory");
                    return;

                }
            }
        }
    }

    private void UsePotion(bool isShot)
    {
        Debug.Log("Used potion and wasShot: " + isShot);
        RaycastHit[] hits = Physics.BoxCastAll(Camera.main.transform.position, new Vector3(screenBorder.size.x, 20, screenBorder.size.y), Camera.main.transform.forward, Quaternion.identity, 30f, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < hits.Length; i++)
        {
            //Damage all enemies on screen
            //If potion was used do damage based on class magic value
            //if (!isShot)
            //    hits[i].transform.GetComponent<Enemy>().TakeDamage(classData.Magic);
            ////If potion was shot do less damage 
            //else if (isShot)
            //    hits[i].transform.GetComponent<Enemy>().TakeDamage(classData.Magic / 2);

        }
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
        }
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IFloorItem>() != null)
        {
            other.gameObject.GetComponent<IFloorItem>().HandlePickup(this);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<IFloorItem>() != null)
        {
            hit.gameObject.GetComponent<IFloorItem>().HandlePickup(this);
        }
    }
}
