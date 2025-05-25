using UnityEngine;

namespace QuestSystem
{
    public class QuestRegistry : MonoBehaviour // Compteur de Quêtes
    {
        private int _currentId = 1;
        public static QuestRegistry Instance;
        
        [Header("Special Object")]
        public GameObject ToyotaBomba;
        public GameObject AnimationExplosion;
        
        public int RequestId() => _currentId++;
        
        public void Reset()
        {
            EnemyContainer.Instance.KillAllEnemies();
            _currentId = 1;
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError("Il existe déjà un quest registry ESTUPIDO");
                Destroy(gameObject);
            }
        }
    }
}