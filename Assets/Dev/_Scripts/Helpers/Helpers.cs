using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds BetterWaitForSeconds(float seconds)
    {
        // Check if WaitForSeconds instance for the specified seconds exists in the dictionary
        if (!WaitDictionary.TryGetValue(seconds, out var wait))
        {
            wait = new WaitForSeconds(seconds);
            WaitDictionary.Add(seconds, wait);
        }

        return wait;
    }
    
    public static int LayerToInt(this LayerMask mask)
    {
        return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
    }

    public static int Sqr(this int value)
    {
        return value * value;
    }

    public static float Sqr(this float value)
    {
        return value * value;
    }
}