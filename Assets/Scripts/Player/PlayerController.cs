using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public ClassData classData;
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
    public bool canMove, canShoot = true;


    #region Input
    private Vector2 movement;
    private bool hasShot = false;
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

        StartCoroutine(ReduceHealthOverTime());

    }

    private void Update()
    {
        if (canMove)
            HandleMovement();
        if (canShoot)
            HandleShoot();

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
        if(playerVelocity != Vector3.zero && lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void HandleShoot()
    {
        if (hasShot)
        {
            canShoot = !canShoot;
            canMove = !canMove;
            StartCoroutine(ShootOnce());
        }
    }

    private IEnumerator ShootOnce()
    {
        GameObject bullet = Instantiate(classData.ProjectilePrefab, new Vector3( transform.position.x, transform.position.y + (controller.height / 2), transform.position.z), Quaternion.LookRotation(transform.forward));
        yield return new WaitForSeconds(classData.ShootTime);
        canShoot = !canShoot;
        canMove = !canMove;
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
}
