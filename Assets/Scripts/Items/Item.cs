using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public abstract string itemName { get; }

    public abstract ItemType itemType { get; }

    public abstract int slot { get; }

    public abstract GameObject viewmodelPrefab { get; }

    //public abstract GameObject viewmodelPrefabLocation { get; }

    public abstract int weight { get; }

}
