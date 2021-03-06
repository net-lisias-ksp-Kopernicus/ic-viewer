﻿using System;
using System.IO.IsolatedStorage;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class Logger : MonoBehaviour
{
    /// <summary>
    /// The UI element that is used to display the log
    /// </summary>
    private TMP_InputField _display;

    /// <summary>
    /// Whether to use colored text
    /// </summary>
    public Boolean UseRichText = true;
    
    /// <summary>
    /// The color that is used for debug messages
    /// </summary>
    public Color ColorDebug = Color.green;

    /// <summary>
    /// The color that is used for normal messages
    /// </summary>
    public Color ColorInfo = Color.white;

    /// <summary>
    /// The color that is used for warning messages
    /// </summary>
    public Color ColorWarning = Color.yellow;

    /// <summary>
    /// The color that is used for error messages
    /// </summary>
    public Color ColorError = Color.red;

    /// <summary>
    /// The global instance of the logging system
    /// </summary>
    public static Logger Instance;
    
    /// <summary>
    /// Grab the UI element
    /// </summary>
    void Start()
    {
        _display = GetComponent<TMP_InputField>();
        _display.textComponent.richText = UseRichText;
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        // Say hello
        Info("Initializing ic-viewer.");
    }

    /// <summary>
    /// Logs a debug message
    /// </summary>
    public static void Debug(Object message)
    {
        UnityEngine.Debug.Log(message);
        #if UNITY_EDITOR
        String formatted = "[DBG] " + message;
        if (Instance.UseRichText)
        {
            formatted = "<color=" + ColorToHex(Instance.ColorDebug) + ">" + formatted + "</color>";
        }

        Instance._display.text += formatted + "\n";
        #endif
    }

    /// <summary>
    /// Logs an info message
    /// </summary>
    public static void Info(Object message)
    {
        UnityEngine.Debug.Log(message);
        String formatted = "[LOG] " + message;
        if (Instance.UseRichText)
        {
            formatted = "<color=" + ColorToHex(Instance.ColorInfo) + ">" + formatted + "</color>";
        }

        Instance._display.text += formatted + "\n";
    }

    /// <summary>
    /// Logs a warning message
    /// </summary>
    public static void Warning(Object message)
    {
        UnityEngine.Debug.LogWarning(message);
        String formatted = "[WRN] " + message;
        if (Instance.UseRichText)
        {
            formatted = "<color=" + ColorToHex(Instance.ColorWarning) + ">" + formatted + "</color>";
        }

        Instance._display.text += formatted + "\n";
    }

    /// <summary>
    /// Logs an error message
    /// </summary>
    public static void Error(Object message)
    {
        UnityEngine.Debug.LogError(message);
        String formatted = "[ERR] " + message;
        if (Instance.UseRichText)
        {
            formatted = "<color=" + ColorToHex(Instance.ColorError) + ">" + formatted + "</color>";
        }

        Instance._display.text += formatted + "\n";
    }

    /// <summary>
    /// Converts a Unity color into a html hex color
    /// </summary>
    private static String ColorToHex(Color32 color)
    {
        String red = BitConverter.ToString(new[] {color.r});
        String green = BitConverter.ToString(new[] {color.g});
        String blue = BitConverter.ToString(new[] {color.b});
        String alpha = BitConverter.ToString(new[] {color.a});
        return "#" + red + green + blue + alpha;
    }
}