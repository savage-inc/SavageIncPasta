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
    private struct PartyData
    {
        public List<string> PartyInventory;
        public int Gold;
        //character data
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
            ShopData shopData;
            shopData.ID = shop.gameObject.GetComponent<GameObjectGUID>().GameObjectID;
            shopData.Items = new Dictionary<string, int>();
            foreach (var item in shop.Inventory.GetItems())
            {
                shopData.Items.Add(item.Item.Name,item.Amount);
            }
            //Add shop to scenedata
            sceneData.ShopData.Add(shopData);
        }


        SaveBytesToFile(SceneName + ".data", SerializeToBytes(sceneData));
    }

    public static void LoadSceneData(string SceneName ,Transform playerTransform, Shop[] shops, ItemDatabase database)
    {
        var data = ReadBytesFromFile(SceneName + ".data");
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
                        shop.Inventory.Clear();
                        //add all items to the shop
                        foreach (var item in shopData.Items)
                        {
                            for (int i = 0; i < item.Value; i++)
                            {
                                shop.Inventory.AddItem(item.Key,database);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void SavePartyData(PartyInventory partyInventory)
    {
        PartyData partyData;
        partyData.PartyInventory = partyInventory.Inventory.SaveToList();
        partyData.Gold = partyInventory.Gold;
        SaveBytesToFile("partyData.data", SerializeToBytes(partyData));
    }

    public static void LoadPartyData(PartyInventory partyInventory, ItemDatabase database)
    {
        var data = ReadBytesFromFile("partyData.data");
        if (data != null)
        {
            PartyData partyData = DeserializeToType<PartyData>(data);

            if (partyData.PartyInventory.Count > 0 && partyData.Gold > 0)
            {
                partyInventory.Inventory.Clear();
                partyInventory.Inventory.LoadFromList(partyData.PartyInventory, database);
                partyInventory.Gold = partyData.Gold;
            }
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

    private static T DeserializeToType<T>(Byte[] data)
    {
        var stream = new MemoryStream(data);
        var formatter = new BinaryFormatter();
        stream.Seek(0, SeekOrigin.Begin);
        return (T) formatter.Deserialize(stream);
    }

    private static Byte[] SerializeToBytes<T>(T data)
    {
        var stream = new MemoryStream();
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream,data);
        stream.Flush();
        stream.Position = 0;
        return stream.ToArray();
    }

    private static void SaveBytesToFile(string filename ,Byte[] data)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/save"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/save");
        }

        string dataPath = Application.persistentDataPath + "/save/" + filename;
        File.WriteAllBytes(dataPath, data);
        Debug.Log("Saved File " + dataPath);
    }

    private static Byte[] ReadBytesFromFile(string filename)
    {
        string dataPath = Application.persistentDataPath + "/save/" + filename;
        if (File.Exists(dataPath))
        {
            return File.ReadAllBytes(dataPath);
        }

        return null;
    }

}
