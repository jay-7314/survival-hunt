using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScreenshotTool
{
    [MenuItem("Tools/Capture Screenshot %&c")]
    private static void Capture()
    {
        Directory.CreateDirectory("Screenshots");
        string path = $"Screenshots/shot_{System.DateTime.Now:HHmmss}.png";
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log("¿˙¿Â: " + path);
    }
}