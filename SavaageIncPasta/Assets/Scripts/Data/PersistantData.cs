using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PersistantData
{
    [System.Serializable]
    private struct PlayerData
    {
        public float X, Y;
    }

    [System.Serializable]
    private struct ItemData
    {
        public List<BaseItemData> ItemDatabase;

        public ItemData(List<BaseItemData> itemDatabase = null) : this()
        {
            ItemDatabase = new List<BaseItemData>();
        }
    }
    [System.Serializable]
    private struct PartyData
    {
        public Inventory PartyInventory;
        public int Gold;
        //character data
        public List<Character> PartyCharacterData;
        public List<Character> ClanCharacterData;

        public PartyData(List<Character> partyCharacterData = null) : this()
        {
            PartyCharacterData = new List<Character>();
        }
    }

    [System.Serializable]
    private struct ShopData
    {
        public int ID;
        public Dictionary<string,int> Items;
    }

    [System.Serializable]
    private struct SceneData
    {
        public string SceneName;
        public PlayerData PlayerData;
        //Shops
        public List<ShopData> ShopData;

        public SceneData(List<ShopData> shopData = null) : this()
        {
            ShopData = new List<ShopData>();
        }
    }

    public static void SetPlayerPositionInNextScene(Vector2 pos)
    {
        PlayerPrefs.SetFloat("PlayerPosX", pos.x);
        PlayerPrefs.SetFloat("PlayerPosY", pos.y);
        PlayerPrefs.Save();
    }

    public static bool HasPositionInScene()
    {
        return PlayerPrefs.HasKey("PlayerPosX");
    }

    public static Vector2 GetPlayerPositionInScene()
    {
        Vector2 pos = new Vector2(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"));

        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.Save();

        return pos;
    }

    public static bool HasInventory()
    {
        string dataPath = Application.persistentDataPath + "/save/partyData.data";

        return File.Exists(dataPath);
    }

    public static void SaveSceneData(string SceneName, Vector2 PlayerPos, Shop[] shops)
    {
        SceneData sceneData = new SceneData();
        sceneData.SceneName = SceneName;

        PlayerData playerData;
        playerData.X = PlayerPos.x;
        playerData.Y = PlayerPos.y;

        sceneData.PlayerData = playerData;

        //Shops
        sceneData.ShopData = new List<ShopData>();
        foreach (var shop in shops)
        {
            if (shop.Inventory != null)
            {
                ShopData shopData;
                shopData.ID = shop.gameObject.GetComponent<GameObjectGUID>().GameObjectID;
                shopData.Items = new Dictionary<string, int>();
                foreach (var item in shop.Inventory.GetItems())
                {
                    shopData.Items.Add(item.Item.DatabaseName, item.Amount);
                }
                //Add shop to scenedata
                sceneData.ShopData.Add(shopData);
            }
        }


        SaveBytesToFile(Application.persistentDataPath + "/save/", SceneName + ".data", SerializeToBytes(sceneData));
    }

    public static void LoadSceneData(string SceneName ,Transform playerTransform, Shop[] shops)
    {
        var data = ReadBytesFromFile(Application.persistentDataPath + "/save/", SceneName + ".data");
        if (data != null)
        {
            SceneData sceneData = DeserializeToType<SceneData>(data);

            if (sceneData.SceneName.Length > 0)
            {
                playerTransform.position = new Vector2(sceneData.PlayerData.X, sceneData.PlayerData.Y);
            }

            //Shops
            foreach (var shop in shops)
            {
                if (sceneData.ShopData == null || sceneData.ShopData.Count <= 0)
                {
                    break;
                }

                foreach (var shopData in sceneData.ShopData)
                {
                    if (shop.gameObject.GetComponent<GameObjectGUID>().GameObjectID == shopData.ID) //Same shop
                    {
                        //add all items to the shop
                        foreach (var item in shopData.Items)
                        {
                            for (int i = 0; i < item.Value; i++)
                            {
                                shop.Inventory.AddItem(item.Key);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void SavePartyData(PartyInventory partyInventory, PlayerManager playerManager, ClanManager clanManager)
    {
        if (partyInventory == null || playerManager == null)
        {
            return;
        }
        PartyData partyData = new PartyData();
        partyData.PartyInventory = partyInventory.Inventory;
        partyData.Gold = partyInventory.Gold;

        //Party characets
        if (playerManager != null)
        {
            //check if players are dead, if so remove them
            foreach (var character in playerManager.Characters)
            {
                if (!character.Alive)
                {
                    playerManager.RemoveCharacter(character);
                }
            }
            partyData.PartyCharacterData = playerManager.Characters;
        }
        else
        {
            partyData.PartyCharacterData = GetSavedPlayerManager();
        }

        if (clanManager != null)
        {
            partyData.ClanCharacterData = clanManager.SpareCharacterPool;
        }
        else
        {
            partyData.ClanCharacterData = GetSavedClanData();
        }

        SaveBytesToFile(Application.persistentDataPath + "/save/","partyData.data", SerializeToBytes(partyData));
    }

    public static List<Character> GetSavedPlayerManager()
    {
        List<Character> playerManager = new List<Character>();

        var data = ReadBytesFromFile(Application.persistentDataPath + "/save/", "partyData.data");
        if (data != null)
        {
            PartyData partyData = DeserializeToType<PartyData>(data);

            playerManager = partyData.PartyCharacterData;
            foreach (var character in playerManager)
            {
                character.Equipment.Character = character;
            }
        }

        return playerManager;
    }

    public static List<Character> GetSavedClanData()
    {
        List<Character> clan = new List<Character>();

        var data = ReadBytesFromFile(Application.persistentDataPath + "/save/", "partyData.data");
        if (data != null)
        {
            PartyData partyData = DeserializeToType<PartyData>(data);

            clan = partyData.ClanCharacterData;
            foreach (var character in clan)
            {
                character.Equipment.Character = character;
            }
        }

        return clan;
    }

    public static void LoadPartyData(PartyInventory partyInventory, PlayerManager playerManager, ClanManager clanManager)
    {
        var data = ReadBytesFromFile(Application.persistentDataPath + "/save/", "partyData.data");
        if (data != null)
        {
            PartyData partyData = DeserializeToType<PartyData>(data);

            if (partyInventory != null && partyData.PartyInventory != null && partyData.Gold > 0)
            {
                partyInventory.Inventory = partyData.PartyInventory;
                partyInventory.Gold = partyData.Gold;
            }

            if (playerManager != null)
            {
                playerManager.Characters = partyData.PartyCharacterData;
                //set character in the characters equipment
                foreach (var character in playerManager.Characters)
                {
                    character.Equipment.Character = character;
                }
            }

            if (clanManager != null)
            {
                clanManager.SpareCharacterPool = partyData.ClanCharacterData;

                foreach (var character in clanManager.SpareCharacterPool)
                {
                    character.Equipment.Character = character;
                }
            }
        }
    }

    public static void SaveItemDatabase()
    {
        ItemData itemData = new ItemData();
        itemData.ItemDatabase = ItemDatabase.Instance.ToList();

        SaveBytesToFile(Application.persistentDataPath + "/save/", "itemData.data", SerializeToBytes(itemData));
    }

    public static void LoadItemDatabase()
    {
        var data = ReadBytesFromFile(Application.persistentDataPath + "/save/", "itemData.data");
        if (data != null)
        {
            ItemData itemData = DeserializeToType<ItemData>(data);
            ItemDatabase.Instance.FromList(itemData.ItemDatabase);
        }
        else
        {
            ItemDatabase.Instance.CreateItems();
        }
    }

    public static void ClearSaves()
    {
        string dataPath = Application.persistentDataPath + "/save/";
        if (Directory.Exists(dataPath))
        {
            Directory.Delete(dataPath,true);
        }
    }

    public static T DeserializeToType<T>(Byte[] data)
    {
        var stream = new MemoryStream(data);
        var formatter = new BinaryFormatter();
        stream.Seek(0, SeekOrigin.Begin);
        return (T) formatter.Deserialize(stream);
    }

    public static T DeserializeToType<T>(TextAsset asset)
    {
        var stream = new MemoryStream(asset.bytes);
        var formatter = new BinaryFormatter();
        stream.Seek(0, SeekOrigin.Begin);
        return (T)formatter.Deserialize(stream);
    }

    public static Byte[] SerializeToBytes<T>(T data)
    {
        var stream = new MemoryStream();
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream,data);
        stream.Flush();
        stream.Position = 0;
        return stream.ToArray();
    }

    public static void SaveBytesToFile(string path, string filename ,Byte[] data)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string dataPath = path + filename;
        File.WriteAllBytes(dataPath, data);
        Debug.Log("Saved File " + dataPath);
    }

    public static Byte[] ReadBytesFromFile(string path, string filename)
    {
        string dataPath = path + filename;
        if (File.Exists(dataPath))
        {
            return File.ReadAllBytes(dataPath);
        }

        return null;
    }

}
