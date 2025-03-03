using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets: MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("ItemAssets.Instance initialized.");
        }
        else
        {
            Debug.LogError("ItemAssets instance already exists. Something is wrong.");
        }
    }

    public Transform pfItemWorld;

    public Sprite paperSprite;
    public Sprite plasticcanSprite;
    public Sprite garbageSprite;
    public Sprite lpaperSprite;
    public Sprite bagSprite;
    public Sprite scissorsSprite;
    
}


