using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class ScriptCon : MonoBehaviour
    {
        public CapsuleCollider Collider;
        public bool IsTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Player"))
            {;
                Debug.Log("Quête 2 terminée !");
                IsTriggered = true;
            }
            else
            {
                IsTriggered = false;
            }
        }
        
    }

}
