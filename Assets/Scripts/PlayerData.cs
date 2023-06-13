using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public string playerName;
    public int enemiesKilled;
    public int remainingCoins;
    public int survivedTurns;
    public int placedTurrets;

    public void PlayerDataInitialization(string playerName, int enemiesKilled, int remainingCoins, int survivedTurns, int placedTurrets)
    {
        this.playerName = playerName;
        this.enemiesKilled = enemiesKilled;
        this.remainingCoins = remainingCoins;
        this.survivedTurns = survivedTurns;
        this.placedTurrets = placedTurrets;
    }

    public IEnumerator SaveData(string jsonData)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        string url = "https://tower-defense-vr-default-rtdb.europe-west1.firebasedatabase.app/playerData.json";

        using (UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al guardar los datos: " + www.error);
            }
            else
            {
                Debug.Log("Datos guardados exitosamente.");
            }
        }
    }
}

