using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;

public class WindowSystemManager
{
    private static readonly string currentDirectoryKey = "v1.4.1";

    public static string GetDLLDirectory()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey(Application.companyName).OpenSubKey("AzureKinectDLLs");
        if (key == null) return null;
        if (key.GetValue(currentDirectoryKey) == null) return null;
        return key.GetValue(currentDirectoryKey).ToString();
    }
}