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



    private void Start()
    {
        player = new Player(classData);
        gameManager = GameManager.Instance;
        inputManager = InputManager.Instance;

        controller = GetComponent<CharacterController>();

    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        Vector2 movement = inputManager.GetMovement();
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        controller.Move(move * Time.deltaTime * classData.CurSpeed);

    }


}
