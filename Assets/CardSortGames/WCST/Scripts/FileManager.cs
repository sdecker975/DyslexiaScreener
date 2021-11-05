using UnityEngine;
using System.Collections;

public class FileManager : MonoBehaviour
{
    static string fileName = Settings.last + "_" + Settings.first + "/CardSortP2/" +Settings.last + "_" + Settings.first + "_CardSortP2.csv";

    void Start()
    {
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
        fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.
        WriteOut("Date:" + System.DateTime.Now + ",");
    }

    public static void WriteOut(string s)
    {
        if (System.IO.File.Exists(fileName))
            System.IO.File.AppendAllText(fileName, s);
        else
            System.IO.File.WriteAllText(fileName, s);
    }
}
