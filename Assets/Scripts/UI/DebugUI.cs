using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugUI : MonoBehaviour
{
    [SerializeField] TMP_Text textObject;
    [Space(8)]
    [SerializeField] private int fontSize;
    [Space(8)]
    [SerializeField] private bool player;
    [SerializeField] private bool flashlight;
    [SerializeField] private bool timeSystem;
    [SerializeField] private bool powerSystem;
    [SerializeField] private bool roomSystem;
    [SerializeField] private bool sanitySystem;

    private string text;

    // Player

    [HideInInspector] public float stamina;
    [HideInInspector] public bool isTired;

    // Flashlight

    [HideInInspector] public bool hasFlashlight;
    [HideInInspector] public float flashLightCharge;
    [HideInInspector] public float flashLightMaxCharge;

    // Time System
    [HideInInspector] public string currentTime;
    [HideInInspector] public string endTime;
    [HideInInspector] public string timeScale;

    // Power System

    [HideInInspector] public float power;
    [HideInInspector] public float maxPower;
    [HideInInspector] public float powerDrain;
    [HideInInspector] public float powerFailure;

    // Room System

    [HideInInspector] public RoomSystem.roomsEnum room;
    [HideInInspector] public bool isInLight;

    // Sanity System

    [HideInInspector] public int disturbanceLevel;
    [HideInInspector] public int disturbanceMaxLevel;

    [HideInInspector] public float actualDisturbanceAmplifier;

    [HideInInspector] public float sanity;
    [HideInInspector] public float maxSanity;
    [HideInInspector] public float actualSanityDropRate;
    [HideInInspector] public float actualSanityAmplifier;

    [HideInInspector] public bool isIntruderActive;
    [HideInInspector] public string timeTillIntruder;

    [HideInInspector] public float actualDisturbanceChance;
    [HideInInspector] public float actualDangerChance;
    [HideInInspector] public float actualIntruderChance;
    [HideInInspector] public float actualGameOverChance;

    [HideInInspector] public bool canDisturb;
    [HideInInspector] public bool canDanger;
    [HideInInspector] public bool canIntruder;
    [HideInInspector] public bool canGameOver;

    public static DebugUI instance;

    private InputAction debugAction;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        debugAction = InputSystem.actions.FindAction("Debug");
        textObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (debugAction.WasPressedThisFrame())
        {
            textObject.gameObject.SetActive(!textObject.gameObject.activeSelf);
        }
    }

    private void LateUpdate()
    {
        UpdateText();
        textObject.fontSize = fontSize;
    }

    public void UpdateText()
    {
        text = "";

        // Player
        if (player)
        {
            text += GetHeader("Player", "orange");
            text += String.Format("Stamina: {0}\n", stamina);
            text += String.Format("Is tired: {0}\n", isTired);
            text += "\n";
        }
        
        if (flashlight)
        {
            // Flashlight
            text += GetHeader("Flashlight", "blue");
            text += String.Format("Has flashlight: {0}\n", hasFlashlight);
            text += String.Format("Flashlight charge: {0}/{1}\n", flashLightCharge, flashLightMaxCharge);
            text += "\n";
        }

        if (timeSystem)
        {
            // Time System
            text += GetHeader("Time System", "red");
            text += String.Format("Current time: {0}\n", currentTime);
            text += String.Format("End time: {0}\n", endTime);
            text += String.Format("Time scale: {0}\n", timeScale);
            text += "\n";
        }

        if (powerSystem)
        {
            // Power System
            text += GetHeader("Power System", "yellow");
            text += String.Format("Power: {0}%\n", (int) (power / maxPower * 100));
            text += String.Format("Max power: {0}%\n", (int) (100 * powerFailure));
            text += String.Format("Power drain per second: {0:F2}%\n", powerDrain / maxPower * 100f).Replace(',', '.');
            text += "\n";
        }        

        if (roomSystem)
        {
            // Room System
            text += GetHeader("Room System", "green");
            text += String.Format("Room: {0}\n", room);
            text += String.Format("In light: {0}\n", isInLight);
            text += "\n";
        }

        if (sanitySystem)
        {
            // Sanity System
            text += GetHeader("Sanity System", "purple");
            text += String.Format("Disturbance level: {0}%\n", disturbanceMaxLevel != 0f ? (int) (disturbanceLevel / disturbanceMaxLevel * 100f) : 0f);
            // text += String.Format("Disturbance amplifier: {0}\n", actualDisturbanceAmplifier);

            text += String.Format("Sanity: {0}%\n", maxSanity != 0f ? (int) (sanity / maxSanity * 100f) : 0f);
            text += GetSanityDelta();
            text += String.Format("Sanity danger increment: +{0:F2}%\n", actualSanityAmplifier).Replace(',', '.');

            text += String.Format("Intruder active: {0}\n", isIntruderActive).Replace(',', '.');
            text += GetTimeTillIntruder();

            text += String.Format("Disturbance chance: {0:F2}%\n", actualDisturbanceChance).Replace(',', '.');
            text += String.Format("Can disturb: {0}\n", canDisturb);

            text += String.Format("Danger chance: {0:F2}%\n", actualDangerChance).Replace(',', '.');
            text += String.Format("Can endanger: {0}\n", canDanger);

            text += String.Format("Intruder chance: {0:F2}%\n", actualIntruderChance).Replace(',', '.');
            text += String.Format("Can intrude: {0}\n", canIntruder && isIntruderActive);

            text += String.Format("Game over chance: {0:F2}%\n", actualGameOverChance).Replace(',', '.');
            text += String.Format("Can game over: {0}\n", canGameOver);
            text += "\n";
        }

        textObject.text = text;
    }

    private string GetHeader(string headerText, string color)
    {
        // black, blue, green, orange, purple, red, white, and yellow

        return "<color=" + color + ">" + headerText + "</color>\n";
    }

    private string GetSanityDelta()
    {
        if (maxSanity == 0f)
        {
            return "";
        }

        if (-actualSanityDropRate > 0)
        {
            if ((int) sanity == 100)
            {
                return String.Format("Sanity delta per second: 0.0\n");
            }
            else
            {
                return String.Format("Sanity delta per second: {0:F2}", -actualSanityDropRate * 100f / maxSanity).Replace(',', '.') + "% (regen)\n";
            }
        }
        else
        {
            return String.Format("Sanity delta per second: {0:F2}", actualSanityDropRate * 100f / maxSanity).Replace(',', '.') + "% (drain)\n";
        }
    }

    private string GetTimeTillIntruder()
    {
        return String.Format("Time till intruder: {0}\n", timeTillIntruder).Replace(',', '.');
    }
}
