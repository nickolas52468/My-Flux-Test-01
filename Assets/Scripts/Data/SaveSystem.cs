using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections;
using System.Collections.Generic;

public static class SaveSystemData {
    
    public static void SaveGame(GameData data)
    {
        var buffer = new MemoryStream();
        var writer = new BinaryWriter(buffer);


        byte[] _data;


        writer.Write(data.CurrentLevel);
        writer.Write(data.Points);
        writer.Write(data.ShootingSelected);
        writer.Write(data.EnemiesDead);
        writer.Write(data.WithShield);

        writer.Close();


        _data = buffer.ToArray();


        string path = $"{Application.persistentDataPath}/GInfos.NSEData";

        File.WriteAllBytes(path, _data);
    }


    public static GameData LoadGame()
    {
        string path = $"{Application.persistentDataPath}/GInfos.NSEData";

        var buffer = new MemoryStream();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            byte[] _datas = File.ReadAllBytes(path);

            GameData data =  new GameData(false,0,0,0,0);

            try
            {

                using (BinaryReader reader = new BinaryReader(stream))
                {
                    //reading Sequential:
                    data.CurrentLevel = reader.ReadInt32();
                    data.Points = reader.ReadInt32();
                    data.ShootingSelected = reader.ReadInt32();
                    data.EnemiesDead = reader.ReadInt32();
                    data.WithShield = reader.ReadBoolean();
                }
            }
            catch {
                Debug.LogError("algo deu errado!");
            }

            //    data = formatter.Deserialize(stream) as GameData;
            //stream.Dispose();

            //Debug.Log("Foi carregado!");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
        
    }


}
