using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;
    public Transform AnalogUI;
    private Rigidbody RB;
    private Vector2 ClickPositionOrigin;
    private Vector2 ClickPositionCurrent;
    private float RotationAngle;
    private AnalogState State;

    [SerializeField]
    private float Speed;
    private const float MinClamp = 0f;
    private const float MaxClamp = 50f;

    private enum AnalogState
    {
        Usable,
        Idle
          
    }

    void Awake()
    {
        State = AnalogState.Idle;
        AnalogUI.gameObject.SetActive(false);
        RB = GetComponent<Rigidbody>();
        if (Instance == null)
            Instance = this;
    }

 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            State = AnalogState.Usable;
            ClickPositionOrigin = Input.mousePosition;
            AnalogUI.position = ClickPositionOrigin;
            AnalogUI.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            State = AnalogState.Idle;
            AnalogUI.gameObject.SetActive(false);
        }
        switch (State)
        {
            case AnalogState.Usable:
                ClickPositionCurrent = Input.mousePosition;
                RotationAngle = Mathf.Atan2(ClickPositionCurrent.x - ClickPositionOrigin.x, ClickPositionCurrent.y - ClickPositionOrigin.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, RotationAngle, 0);

                float distanceBetweenPoints = Mathf.Clamp(Vector2.Distance(ClickPositionCurrent, ClickPositionOrigin), MinClamp, MaxClamp);
                AnalogUI.GetChild(0).position = ClickPositionOrigin +
                    new Vector2(ClickPositionCurrent.x - ClickPositionOrigin.x, ClickPositionCurrent.y - ClickPositionOrigin.y).normalized * distanceBetweenPoints;
                RB.MovePosition(RB.position + transform.forward * Speed * Time.deltaTime);
                break;

            case AnalogState.Idle:
                break;
        }
    }

}
