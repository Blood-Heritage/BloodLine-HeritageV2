using Photon.Pun;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace QuestSystem {
	
    
    
    
    public class Quest : MonoBehaviour
    {
        public int Id;
        public int Mission;
        public string Objective;
        public int Reward;
        public GameplayEnum Type;
        public bool isFinished;
		TextMeshProUGUI textNextMission = GameObject.Find("NextMission").GetComponent<TextMeshProUGUI>();
		TextMeshProUGUI textPoserBomber = GameObject.Find("PoserBombe").GetComponent<TextMeshProUGUI>();
    
        public Quest(int mission, string objective, int reward, GameplayEnum type)
        {
            Id = QuestRegistry.Instance.RequestId(); // Auto-Compteur
            Mission = mission;
            Objective = objective;
            Reward = reward;
            Type = type;
            isFinished = false;
        }

		public void StartQuest() // Initialise la quête (apparition d'ennemis, mini-map, ...)
		{
			
  		  switch(Id)
  		  {
			 // -    =     Nothing to do
			 // |    =     TODO : Mettre le bonne objet à la place de la voiture et mettre les bonnes coordonées
			 


			// -------------------- MISSION 1 --------------------
       		 case 1:  // -
		         break;
       		 case 2: // TODO : Faire spawn la cible
		         var spawner = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(-42, 0, -95));
		         spawner.Spawn();
		         break;  // TODO : |
       		 case 3:   // TODO : Faire spawn les agents du FBI
		         var SpawnFBI = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(-41, 0, -90));
		         SpawnFBI.Spawn();// TODO : |
		         var SpawnFBI5 =  new  QuestSystem.Spawner("FBI_Enemy", new Vector3(-35, 0, -122));
		         SpawnFBI5.Spawn();
		         break;
      		 case 4:  // -
		         break;
      		 case 5:  // -
		         break;
      		 case 6:  // -
		         break;


			// -------------------- MISSION 2 --------------------
      		 case 7:
		         textNextMission.text = "MISSION 2";
		         break;
      		 case 8:  // TODO : Faire un truc avec la cache d'armes
		         break;
             case 9:  // TODO : -
	             break;
             case 10: // TODO : Faire spawn le premier agent
	             var SpawnFBI1 = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(-21, 0, -94));
	             SpawnFBI1.Spawn();// TODO : |
	             break;
             case 11: // TODO : Faire spawn le deuxième agent
	             var SpawnFBI2 = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(190, 0, -5));
	             SpawnFBI2.Spawn();// TODO : |
	             break;
             case 12: // TODO : Faire spawn le troisième agent
	             var SpawnFBI3 = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(10, 0, 147));
	             SpawnFBI3.Spawn();// TODO : |
	             break;
             case 13: // -
	             break;
             case 14: // -
	             break;
             case 15: // TODO : Donner une bombe au joueur (juste sur l'UI?)
	             break;
             case 16: // TODO : Faire spawn deux agents
	             var Agent1 = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(70, 0, 89));
	             var Agent2 = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(64, 0, 94));
	             Agent1.Spawn();// TODO : |
	             Agent2.Spawn();
	             break;
             case 17: // -
	             break;
             case 18: // TODO : Activer le fait de pouvoir placer la bombe
	             textPoserBomber.text = "Appuyez sur [B] pour poser une bombe";
	             break;
             case 19: // -
	             break;
             case 20: // -
	             break;
             
             case 21: // -
	             break;


			// -------------------- MISSION 3 --------------------
             case 22: // TODO : Ecran Noir avant la quete : Mission 3
	             textNextMission.text = "MISSION 3";
	             break;
             case 23: // -
	             break;
             case 24: // -
	             break;
             case 25: // -
	             break;
             case 26: // TODO : Faire spawn deux ennemis
	             var Ennemie1 = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(59, 0, -18));
	             var Ennemie2 = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(63, 0, -18));
	             Ennemie1.Spawn();// TODO : |
	             Ennemie2.Spawn();
	             break;
             case 27: // TODO : Faire spawn le Boss
	             var daBoss = new  QuestSystem.Spawner("FBI_Enemy", new Vector3(60, 0, -20));
	             daBoss.Spawn();// TODO : |
	             break;
             case 28: // -
	             break;
             case 29: // -
             	break;
             default:
                // Should not happen !
                break;
    		}
		}

		public void EndQuest() // Termine la quête (disparition des ennemis, mini-map, ...)
		{
			
			var bomb = GameObject.Find("Bomb").GetComponent<Image>();
			var bomba = GameObject.Find("bomb_reel");
  		  switch(Id)
  		  {
			      
			      
			// Si les cibles mortes ne disparaissent pas, il faudra les
			// faire disparaitre ici.
       		 case 1: // -
		         break;
		         
       		 case 2: // -
		         break;
       		 case 3: //
		         break;
      		 case 4: // - RIEENNNNNNNNNNN
		         break;
      		 case 5: // -
		         break;
      		 case 6: // -
		         break;
      		 case 7: // -
		         textNextMission.text = "";
		         break;
      		 case 8: // -
		         break;
             case 9: // -
	             break;
             case 10: // -
	             break; 
             case 11: // -
	             break;
             case 12: // -
	             break;
             case 13: // -
	             break;
             case 14: // -
	             bomb.enabled = true;
	             
	             break;
             case 15: // -
	             break;
             case 16: // -
	             break;
             case 17: //- 
	             break;
             case 18: // TODO : UI : Faire apparaitre la Bombe posée sur un immeuble
	             bomb.enabled = false; 
	             QuestRegistry.Instance.ToyotaBomba.SetActive(true);
	             textPoserBomber.text = "";
	             break;
             
             case 19: // TODO : Faire exploser la bombe (après s'être éloigné donc)
	             QuestRegistry.Instance.ToyotaBomba.SetActive(false);
	             PhotonNetwork.Instantiate(QuestRegistry.Instance.AnimationExplosion.name, new Vector3(67,0,89),Quaternion.identity);
	             break;
             case 20: //- break;
	             break;
             case 21: // -
	             break;
             case 22: // -
	             textNextMission.text = "";
	             break;
             case 23: // -
	             break;
             case 24: // -
	             break;
             case 25: // -
	             break;
             case 26: // -
	             break;
             case 27: // -
	             break;
             case 28: // TODO : Afficher message ? (Vous avez vaincu le Lieutenant...)
	             break;
             case 29: // -
             	break;
             default:
                // Should not happen !
                break;
    		}
		}

		// TODO : Une fois les fonctions StartQuest() et EndQuest() finies
		// il ne restera plus qu'à créer des "détecteurs" (game objects..)
		// qui mettront l'attribut CurrentQuest.isFinished à true lorsque
		// l'objectif est rempli

		// Exemple : Supposons Quest31 = "Aller au pont)
		// s'approcher du pont lorsque CurrentQuest==41 met l'attribut
		// CurrentQuest.isFinished à true. (C'est tout, le reste automatique déjà fait)

    }
}

