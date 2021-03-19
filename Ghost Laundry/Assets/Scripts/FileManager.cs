using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileManager
{
    public static bool WriteToFile(string filename, string contents) {
        string fullPath = Path.Combine(Application.persistentDataPath, filename);

        try {
            File.WriteAllText(fullPath, contents);
            return true;
        }
        catch (Exception e) {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
        }
        return false;
    }

    public static bool LoadFromFile(string filename, out string result) {
        string fullPath = Path.Combine(Application.persistentDataPath, filename);

        try {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e) {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
        }
        result = "";
        return false;
    }
}
