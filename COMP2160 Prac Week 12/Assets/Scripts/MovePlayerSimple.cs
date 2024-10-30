using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayerSimple : MonoBehaviour
{
    private PlayerActions playerActions;
    private InputAction mousePosition;
    private InputAction mouseClick;

    [SerializeField] private float speed = 5;
    [SerializeField] private LayerMask layerMask;
    private Vector3 destination;

    void Awake()
    {
        playerActions = new PlayerActions();
        mousePosition = playerActions.Movement.Position;
        mouseClick = playerActions.Movement.Click;
    }

    void OnEnable()
    {
        mousePosition.Enable();
        mouseClick.Enable();
    }

    void OnDisable()
    {
        mousePosition.Disable();
        mouseClick.Disable();
    }

    void Start()
    {
        mouseClick.performed += GetDestination;
        destination = transform.position;
    }

    void Update()
    {
        MoveToDestination();
    }

    private void GetDestination(InputAction.CallbackContext context)
    {
        Vector2 position = mousePosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            destination = hit.point;
        }
    }        

    private void MoveToDestination()
    {
        Vector3 offset = destination - transform.position;
        Vector3 move = offset.normalized * speed * Time.deltaTime;
        if (move.magnitude > offset.magnitude)  // avoid overshoot
        {
            move = offset;
        }
        transform.Translate(move);
    }
}
