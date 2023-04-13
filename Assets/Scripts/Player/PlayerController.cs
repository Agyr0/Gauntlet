using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ClassData classData;
    public IPlayer player;

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
    }

    private void Update()
    {
        if (canMove)
            HandleMovement();
        if (canShoot)
            HandleShoot();
    }

    private void HandleMovement()
    {
        //Get grounded
        isGrounded = controller.isGrounded;

        //Get input 
        Vector2 movement = inputManager.GetMovement();
        //Get Vector3 from input Vector2
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        //Make sure movement takes in camera rotation
        move = mainCamTransform.TransformDirection(movement);
        move.y = 0f;
        //Move character from move Vecotr3
        controller.Move(move * Time.deltaTime * player.CurSpeed);

        //Rotate towards movement
        if(move != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(move);
    }

    private void HandleShoot()
    {
        if (inputManager.GetButton1())
        {
            Debug.Log(player.ProjectilePrefab);
            Debug.Log(classData.ProjectilePrefab);
            ShootOnce();
        }
    }

    private void ShootOnce()
    {
        GameObject bullet = Instantiate(player.ProjectilePrefab, transform.position, Quaternion.LookRotation(transform.forward));

    }
}
