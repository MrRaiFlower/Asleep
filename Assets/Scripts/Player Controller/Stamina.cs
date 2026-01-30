using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] Movement movementScript;

    [SerializeField] private float maxStamina;

    [SerializeField] private float idleStaminaRegen;
    [SerializeField] private float normalStaminaRegen;
    [SerializeField] private float crouchStaminaRegen;
    [SerializeField] private float sprintStaminaConsumption;
    [SerializeField] private float jumpStaminaConsumption;

    [SerializeField] private float tirednessRatio;

    [HideInInspector] public float stamina;
    [HideInInspector] public bool isTired;

    private void Start()
    {
        stamina = maxStamina;
    }

    private void Update()
    {
        if (movementScript.isGrounded)
        {
            if (movementScript.isMoving)
            {
                if (movementScript.horizontalvelocity.sqrMagnitude > Mathf.Pow(movementScript.normalSpeed, 2.0f))
                {
                    stamina -= sprintStaminaConsumption * Time.deltaTime;
                }
                else if (movementScript.horizontalvelocity.sqrMagnitude > Mathf.Pow(movementScript.crouchSpeed, 2.0f))
                {
                    stamina += normalStaminaRegen * Time.deltaTime;
                }
                else
                {
                    stamina += crouchStaminaRegen * Time.deltaTime;
                }
            }
            else
            {
                stamina += idleStaminaRegen * Time.deltaTime;
            }
        }


        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        else if (stamina <= 0.0f)
        {
            stamina = 0.0f;

            isTired = true;
        }

        if (isTired && stamina >= maxStamina * tirednessRatio)
        {
            isTired = false;
        }

        DebugUI.instance.stamina = (int) stamina;
        DebugUI.instance.isTired = isTired;
    }

    public bool Jump()
    {
        if (stamina >= jumpStaminaConsumption && !isTired)
        {
            stamina -= jumpStaminaConsumption;

            return true;
        }
        else
        {
            return false;
        }
    }
}
