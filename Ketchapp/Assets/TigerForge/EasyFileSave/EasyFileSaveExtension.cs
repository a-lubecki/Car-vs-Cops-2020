using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EasyFileSaveExtension
{

    // ----------------------------------------
    // Add new extension inside this function.
    // ----------------------------------------

    public void Start()
    {

        // Name of this extension and the callBack function which contains the extension configuration.
        AddExtension("BoxCollider", BoxColliderExtension);

    }

    // ----------------------------------------
    // Add callBack functions here.
    // ----------------------------------------

    // This extension allow Easy File Save to easily save BoxCollider data.
    void BoxColliderExtension()
    {
        // The boxCollider object data received by the AddCustom() method.
        var data = GetData("BoxCollider");

        // Casting of the object data to BoxCollider.
        BoxCollider bc = (BoxCollider)data;
        
        // Configure which BoxCollider values to load/save and their name to be used with GetCustom() method.
        SetParameters(
            "BoxCollider",
            new Par { name = "centerX", value = bc.center.x }, 
            new Par { name = "centerY", value = bc.center.y },
            new Par { name = "centerZ", value = bc.center.z },
            new Par { name = "sizeX", value = bc.size.x },
            new Par { name = "sizeY", value = bc.size.y },
            new Par { name = "sizeZ", value = bc.size.z },
            new Par { name = "enabled", value = bc.enabled },
            new Par { name = "isTrigger", value = bc.isTrigger },
            new Par { name = "contactOffset", value = bc.contactOffset }
            );
    }














    // ******************************************************
    //       DON'T MODIFY ANYTHING UNDER THIS COMMENT:
    // ******************************************************

    #region " EXTENSIONS ENGINE "

    public Dictionary<string, UnityAction> extensions = new Dictionary<string, UnityAction>();
    public Dictionary<string, object> data = new Dictionary<string, object>();
    public Dictionary<string, List<object>> pars = new Dictionary<string, List<object>>();
    public Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>();

    struct Par
    {
        public string name;
        public object value;
    }

    /// <summary>
    /// Add a new extension to the Easy File Save system
    /// </summary>
    private void AddExtension(string name, UnityAction callBack)
    {
        if (!extensions.ContainsKey(name))
        {
            extensions.Add(name, callBack);
            data.Add(name, null);
            pars.Add(name, null);
            mapping.Add(name, null);
        }
        else
        {
            Debug.LogWarning("An extension with name '" + name + "' already exists.");
        }
    }

    /// <summary>
    /// Get the object data sent to this callback.
    /// </summary>
    private object GetData(string extensionName)
    {
        if (data.ContainsKey(extensionName)) return data[extensionName]; else return null;
    }

    /// <summary>
    /// Collect the object data to save.
    /// </summary>
    private void SetParameters(string extensionName, params Par[] parameters)
    {
        List<object> par = new List<object>();
        List<string> map = new List<string>();

        foreach (Par obj in parameters)
        {
            par.Add(obj.value);
            map.Add(obj.name);
        }

        pars[extensionName] = par;
        mapping[extensionName] = map;
    }

    #endregion

}
