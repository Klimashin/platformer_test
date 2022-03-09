using System;
using UnityEngine;

public class GameManager
{
    public static GameManager Instance { get; private set; }

    public readonly InputActions InputActions;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
       new GameManager();
    }

    private GameManager()
    {
        if (Instance != null)
        {
            throw new Exception("Singleton pattern violation and blah");
        }
        
        Instance = this;
        InputActions = new InputActions();
        Cursor.visible = false;
    }
}