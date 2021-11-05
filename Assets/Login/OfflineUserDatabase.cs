using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OfflineUserDatabase.asset")]
public class OfflineUserDatabase : ScriptableObject {

    [SerializeField] private List<OfflineUserData> offlineUserDataList = new List<OfflineUserData>();

    public bool Matches(string offlineUsername, string offlinePassword) {
        foreach (var offlineUserData in offlineUserDataList) {
            if (offlineUserData.Matches(offlineUsername, offlinePassword)) {
                return true;
            }
        }

        return false;
    }

}

[Serializable]
public struct OfflineUserData {

    [SerializeField] private string username;
    [SerializeField] private string password;

    public bool Matches(string offlineUsername, string offlinePassword) {
        return offlineUsername.Equals(username) && offlinePassword.Equals(password);
    }

}