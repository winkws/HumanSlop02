using UnityEngine;

public class ComponentBase : MonoBehaviour
{
    protected Components core;
    protected Player player;

    [HideInInspector] public bool IsActive;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Components>();
        player = FindFirstObjectByType<Player>();

        if (core == null) { Debug.LogError("There is no Core on the parent"); }
        core.AddComponent(this);
    }

    public virtual void ComponentUpdate() { }

}