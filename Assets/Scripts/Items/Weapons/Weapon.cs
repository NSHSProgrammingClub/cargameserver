using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : Item
{
    [SerializeField] private GameObject _viewmodelPrefab;
    
    //[SerializeField] private GameObject _viewmodelPrefabLocation;

    [SerializeField] private string _itemName;

    [SerializeField] private ItemType _itemType;

    [SerializeField] private int _slot;

    [SerializeField] private WeaponType _weaponType;

    [SerializeField] private FiremodeType[] _firemodeType;

    [SerializeField] private static FiremodeType _currentFireModeType;

    [SerializeField] private WeaponGraphics _graphics;

    [SerializeField] private float _range;

    [SerializeField] private int _firerate;

    [SerializeField] private int _weight;

    [SerializeField] private Ammunition _ammunition;

    [SerializeField] private float _recoilAmount;

    [SerializeField] private string _countryOfOrigin;

    [SerializeField] private string _manufacturer;






    public override GameObject viewmodelPrefab => _viewmodelPrefab;

    //public override GameObject viewmodelPrefabLocation => _viewmodelPrefabLocation/*GameObject.FindGameObjectsWithTag("ViewmodelPrefabLocation")[0]*/;

    public override string itemName => _itemName;

    public override ItemType itemType => ItemType.weapon;

    public override int slot => _slot;

    public override int weight => _weight;
    public WeaponType weaponType => _weaponType;
    public FiremodeType[] firemodeType => _firemodeType;

    public FiremodeType currentFireModeType = _currentFireModeType;

    public WeaponGraphics graphics => _graphics;

    public float range => _range;

    public int firerate => _firerate / 10; // dividing by 10 can be changed

    public Ammunition ammunition => _ammunition;

    public float recoilAmount => _recoilAmount;

    public string countryOfOrigin => _countryOfOrigin;
    public string manufacturer => _manufacturer;
}
