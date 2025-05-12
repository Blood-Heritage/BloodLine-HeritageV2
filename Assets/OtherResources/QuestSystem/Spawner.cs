using UnityEngine;


namespace QuestSystem
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Prefab;
        public Vector3 spawnPosition;

        public Spawner(GameObject prefab, Vector3 spawnposition)
        {
            Prefab = prefab;
            spawnPosition = spawnposition;
        }

        public void Spawn()
        {
            Instantiate(Prefab, spawnPosition, Quaternion.identity);
        }
        
    }
    
    
    
}