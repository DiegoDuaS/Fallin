using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 12f;
    
    [Header("Límites de la Pantalla")]
    [SerializeField] private float xLimit = 8.2f;
    [SerializeField] private float yMin = -4.5f;
    [SerializeField] private float yMax = 4.5f;

    void Update()
    {
        // Leer teclado con el New Input System (Modo rápido sin PlayerInput component)
        float moveX = 0;
        float moveY = 0;

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveX = -1;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveX = 1;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveY = 1;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveY = -1;
        }

        Vector3 moveDir = new Vector3(moveX, moveY, 0).normalized;
        transform.position += moveDir * speed * Time.deltaTime;

        // Clamping (igual que antes)
        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        float clampedY = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector3(clampedX, clampedY, 0);
    }

}