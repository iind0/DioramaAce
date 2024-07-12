using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float gravity = -9.81f; // Valor de la gravedad
    public Animator animator;

    public CharacterController controller;
    public Transform cameraTransform;

    private Vector3 velocity; // Velocidad vertical del jugador

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Ajustar la rotación del personaje
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            float speed = isSprinting ? sprintSpeed : walkSpeed;

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);

            animator.SetBool("Walking", true);
            animator.SetBool("Sprinting", isSprinting);
        }
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Sprinting", false);
        }

        // Aplicar gravedad
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Valor pequeño para mantener el jugador pegado al suelo
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Mover al jugador según la gravedad
        controller.Move(velocity * Time.deltaTime);
    }
}
