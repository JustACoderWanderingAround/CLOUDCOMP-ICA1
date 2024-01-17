using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    public static void toggleObject(GameObject obj)
    {
        obj.SetActive(!obj.activeInHierarchy);
    }
}
