using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ClassData classData;
    public IPlayer player;
    private Vector3 playerVelocity;
    private ScreenBorder screenBorder;

    private CharacterController controller;
    private bool isGrounded;
    private GameManager gameManager;
    private InputManager inputManager;
    private Transform mainCamTransform;
    private Coroutine shootCorutine;
    public bool canMove, canShoot = true;


    private void Start()
    {
        gameManager = GameManager.Instance;
        inputManager = InputManager.Instance;
        mainCamTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        player = new Player(classData);
        screenBorder = new ScreenBorder();
    }

    private void Update()
    {
        if (canMove)
            HandleMovement();
        if (canShoot)
            HandleShoot();

        //Debug.Log(screenBorder.IsOutside(transform, controller.radius, controller.radius));
    }

    private void HandleMovement()
    {
        //Get grounded
        isGrounded = controller.isGrounded;

        //Get input 
        Vector2 movement = inputManager.GetMovement();
        //Get Vector3 from input Vector2
        playerVelocity = new Vector3(movement.x, 0, movement.y);
        //Make sure movement takes in camera rotation
        playerVelocity = mainCamTransform.TransformDirection(movement);
        playerVelocity.y = 0f;

        //Move character from move Vecotr3
        controller.Move(playerVelocity * Time.deltaTime * classData.CurSpeed);


        //Rotate towards movement
        if(playerVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(playerVelocity);
    }

    private void LateUpdate()
    {
        transform.position = screenBorder.ClampToInside(transform, controller.radius, controller.radius);
    }

    


    private void HandleShoot()
    {
        if (inputManager.GetButton1())
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
}
