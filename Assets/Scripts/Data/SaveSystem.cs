using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystemData {
    
    public static void SaveGame(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Dispose();
        //Debug.Log("Foi salvo!");
    }


    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/game.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Dispose();

            //Debug.Log("Foi carregado!");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
        
    }

    /*
    public static bool loadGame = false;
    public static int count;

    public static void startLoading()
    {
        loadGame = true;
    }

    public static bool hasToLoad()
    {
        return loadGame;
    }

    public static void SaveGame (ControlGame game)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(game);

        formatter.Serialize(stream, data);
        stream.Dispose();
    }

    public static void SaveShip (ControlShip controlShip) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ship.main";
        FileStream stream = new FileStream(path, FileMode.Create);

        ShipData data = new ShipData(controlShip);

        formatter.Serialize(stream, data);
        stream.Dispose();
    }

    public static ShipData LoadShip () {
        string path = Application.persistentDataPath + "/ship.main";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ShipData data = formatter.Deserialize(stream) as ShipData;
            stream.Dispose();

            return data;

        } else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    */

}
