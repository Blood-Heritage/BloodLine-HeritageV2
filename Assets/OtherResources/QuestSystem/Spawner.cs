using UnityEngine;
using Photon.Pun; 

namespace QuestSystem
{
    public class Spawner
    {
        public string prefabName;
        public Vector3 spawnPosition;

        public Spawner(string prefabName, Vector3 spawnPosition)
        {
            this.prefabName = prefabName;
            this.spawnPosition = spawnPosition;
        }

        public void Spawn()
        {
            PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);
        }
    }
}


                


