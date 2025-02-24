using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] List<ShopItem> items = new List<ShopItem>();
    [SerializeField] GameObject shopTab;
    [SerializeField] Transform listAnchor, buttonAnchor;
    [SerializeField] GameObject listButtonPrefab, buttonPrefab;
    [SerializeField] Text weaponName, weaponDesc, price;
    [SerializeField] Dropdown slotSelect;
    PlayerWeapons weapons;
    ShopItem setItem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopTab.SetActive(true);
            weapons.DisableClick();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopTab.SetActive(false);
            weapons.EnableClick();
        }
    }
    int tagsLength = 0;
    private void Awake()
    {
        weapons = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapons>();
        Array tmp = Enum.GetValues(typeof(Tags));
        tagsLength = tmp.Length;
        for(int i = 0; i < tagsLength; i++)
        {
            Button button = Instantiate(listButtonPrefab, listAnchor).GetComponent<Button>();
            int a = i;
            button.onClick.AddListener(delegate { GetShopItems(a); });
            button.transform.GetChild(0).GetComponent<Text>().text = tmp.GetValue(i).ToString();
        }
    }
    public void GetShopItems(int tagIndex)
    {
        for(int i = buttonAnchor.childCount-1; i >= 0; i--)
        {
            buttonAnchor.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            buttonAnchor.GetChild(i).gameObject.PoolDestroy();
        }
        foreach(var i in items)
        {
            if(((int)i.tags & 1<<tagIndex) > 0)
            {
                Button tmp = buttonPrefab.PoolCreate(buttonAnchor.position, buttonAnchor.rotation, buttonAnchor).GetComponent<Button>();
                var a = i;
                tmp.onClick.AddListener(delegate { SelectItem(a); });
                tmp.transform.localScale = new Vector2(1, 1);
                tmp.transform.GetChild(0).GetComponent<Text>().text = i.weapon.weaponName;
            }
        }
    }
    public void SelectItem(ShopItem item)
    {
        setItem = item;
        weaponName.text = item.weapon.weaponName;
        weaponDesc.text = item.weapon.Describe();
        price.text = item.price + "$";
    }
    public void Purchase()
    {
        if (setItem == null || GameManager.Instance.money < setItem.price) return;
        GameManager.Instance.UseMoney(setItem.price);
        weapons.Wield(setItem.weapon.gameObject, slotSelect.value);
    }
}
[System.Serializable]
public class ShopItem
{
    [SerializeField] Weapon m_weapon;
    public Weapon weapon { get { return m_weapon; } }
    public int price;
    public Tags tags;
}
[System.Flags][System.Serializable]
public enum Tags
{
    Melee = 1<<0,
    Ranged = 1<<1,
    Tier1 = 1<<2,
    Tier2 = 1<<3,
    Tier3 = 1<<4,
    Speical = 1<<5
}
