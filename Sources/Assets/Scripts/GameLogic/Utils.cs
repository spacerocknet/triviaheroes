using UnityEngine;
using System.Collections;
using System.IO;


public class Utils  {

    private static string[] m_CategoryName = {"Geographies", "Science", "Art", "History", "Sports", "Entertainment"};


    public static string CategoryIndexToString(int index)
    {
        return m_CategoryName[index];
    }

    public static int CategoryStringToIndex(string cat)
    {
        for (int i = 0; i < 6; i++)
        {
            if (m_CategoryName[i] == cat)
            {
                return i;
            }
        }
        return -1;
    }

    public static string pathForDocumentsFile(string filename)
    {
        
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            //Debug.Log(Path.Combine(path, filename));
            return Path.Combine(path, filename);
        }

        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
}
