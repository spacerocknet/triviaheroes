using UnityEngine;
using System.Collections;


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
}
