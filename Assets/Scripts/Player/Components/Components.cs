using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Components : MonoBehaviour
{
    private readonly List<ComponentBase> components = new List<ComponentBase>();

    public void Update()
    {
        foreach (ComponentBase component in components)
        {
            if(!component.IsActive) continue;
            component.ComponentUpdate();
        }
    }

    public void AddComponent(ComponentBase component)
    {
        if (!components.Contains(component))
        {
            components.Add(component);
        }
    }

    public T GetCoreComponent<T>() where T : ComponentBase
    {
        var comp = components.OfType<T>().FirstOrDefault();

        if (comp)
            return comp;

        comp = GetComponentInChildren<T>();

        if (comp)
            return comp;

        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T : ComponentBase
    {
        value = GetCoreComponent<T>();
        return value;
    }
}