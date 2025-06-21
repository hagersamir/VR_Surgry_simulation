using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerManager
{
    private static float startTime = -1f;
    private static float stopTime = -1f;
    private static bool isRunning = false;

    public static void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public static void StopTimer()
    {
        if (isRunning)
        {
            stopTime = Time.time;
            isRunning = false;
        }
    }

    public static void ResetTimer()
    {
        startTime = -1f;
        stopTime = -1f;
        isRunning = false;
    }

    public static float GetDuration()
    {
        if (startTime < 0) return 0f;
        return isRunning ? Time.time - startTime : stopTime - startTime;
    }

    public static bool IsRunning => isRunning;
}

