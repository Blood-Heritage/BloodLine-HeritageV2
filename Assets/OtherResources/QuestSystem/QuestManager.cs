using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public TextMeshProUGUI affichageQuote;
    public TextMeshProUGUI textNextQuest;


    public Queue<Mission> MissionTurboQueue;
	[Header ("Quest")]
    public GameObject ColliderQuest2;
    public ScriptCon ColliderQuest2ScriptCon;
    public GameObject ColliderQuest8;
    public ScriptCon ColliderQuest8ScriptCon;
    
    public GameObject ColliderQuest15;
    public ScriptCon ColliderQuest15ScriptCon;
    
    public GameObject ColliderQuest20;
    public ScriptCon ColliderQuest20ScriptCon;
    
    
    public GameObject ColliderQuest23;
    public ScriptCon ColliderQuest23ScriptCon;
    
    public GameObject ColliderQuest25;
    public ScriptCon ColliderQuest25ScriptCon;
    public bool GameFinished { get => MissionTurboQueue.Count == 0 && CurrentMission.isFinished && CurrentQuest.isFinished; }
    public Mission CurrentMission;
    public Quest CurrentQuest;
    
    
    
	void NextQuest()
	{
		CurrentQuest.EndQuest(); // Supprimmer les ennemis par exemple.
        if (!GameFinished)
        {
            if(CurrentMission.isFinished)
                CurrentMission = MissionTurboQueue.Dequeue();
            CurrentQuest = CurrentMission.QuestsQueue.Dequeue();
			CurrentQuest.StartQuest(); // Initialisation de la quête.
            PrintQuest();
		}
	}

	void Start()
	{
		ColliderQuest2 = GameObject.Find("EventListenerQuest2");
		ColliderQuest2ScriptCon = ColliderQuest2.GetComponent<ScriptCon>();
			
		ColliderQuest8 = GameObject.Find("EventListenerQuest8");
		ColliderQuest8ScriptCon = ColliderQuest8.GetComponent<ScriptCon>();
		
		
		ColliderQuest15 = GameObject.Find("EventListenerQuest15");
		ColliderQuest15ScriptCon = ColliderQuest15.GetComponent<ScriptCon>();
		
		ColliderQuest20 = GameObject.Find("EventListenerQuest20");
		ColliderQuest20ScriptCon = ColliderQuest20.GetComponent<ScriptCon>();
		
				
		ColliderQuest23 = GameObject.Find("EventListenerQuest23");
		ColliderQuest23ScriptCon = ColliderQuest23.GetComponent<ScriptCon>();
		
						
		ColliderQuest25 = GameObject.Find("EventListenerQuest25");
		ColliderQuest25ScriptCon = ColliderQuest25.GetComponent<ScriptCon>();

		
		
		// Debug.Log(ColliderQuest2ScriptCon == null);
		// Debug.Log("QuestManager Started");
		//Quest quest3 = new Quest(2, 1, "C’est un piège ! Vite, élimine la cible !", 10, GameplayEnum.Quote);
		//quest = quest3;
        
		// Partie Affichage
		affichageQuote = GameObject.Find("QuestManager").GetComponent<TextMeshProUGUI>();
		textNextQuest = GameObject.Find("QuestNextQuest").GetComponent<TextMeshProUGUI>();
		// affichageQuote.fontSize = formule de math
        
        
        
		// Partie Quetes et Objectifs
		MissionTurboQueue = new Queue<Mission>();

		MissionTurboQueue.Enqueue(new Mission(1));
		MissionTurboQueue.Enqueue(new Mission(2));
		MissionTurboQueue.Enqueue(new Mission(3));
		
		
		
        
        
        
        
        
		CurrentMission = MissionTurboQueue.Dequeue();
		CurrentQuest = CurrentMission.QuestsQueue.Dequeue();
		PrintQuest(); // Première Quête : Type Quote
	}

	void PrintQuest()
	{
		if (CurrentQuest.Type == GameplayEnum.Quote)
			affichageQuote.text = $"Mission {CurrentQuest.Mission} | Quête n°{CurrentQuest.Id} \n« {CurrentQuest.Objective} »"; // Affichage de la Nouvelle Quête (Type Quote, en bas de l'écran)
		else
			affichageQuote.text = $"Mission {CurrentQuest.Mission} | Quête n°{CurrentQuest.Id} \n{CurrentQuest.Objective}"; // Affichage de la Nouvelle Quête (Type Order, à droite de l'écran)
		// TODO IF TIME : Avoir une deuxième variable pour l'affichage Order sur le coté
		
		if (CurrentQuest.Type == GameplayEnum.Quote)
			textNextQuest.text = "Appuez sur [N] pour continuer ";  // TODO : Add a text displayer "[N] : Next Quest"
		else
			textNextQuest.text = "";
	}

	// Update is called once per frame
    void Update()
    {
	    if (!GameFinished)
	    {
		    // Debug.Log(ColliderQuest2ScriptCon.IsTriggered);
		    if (CurrentQuest.Id == 2 && ColliderQuest2ScriptCon.IsTriggered) 
		    {
			    CurrentQuest.isFinished = true;
		    }

		    if ((CurrentQuest.Id == 8 || CurrentQuest.Id == 13) &&	ColliderQuest8ScriptCon.IsTriggered)
		    {
			    CurrentQuest.isFinished = true;
		    }

		    if (CurrentQuest.Id == 15 && ColliderQuest15ScriptCon.IsTriggered)
		    {
			    CurrentQuest.isFinished = true;
		    }
		    
		    if (CurrentQuest.Id == 18 && Input.inputString == "b")
			    CurrentQuest.isFinished = true;

		    if (CurrentQuest.Id == 20 && ColliderQuest20ScriptCon.IsTriggered)
		    {
			    CurrentQuest.isFinished = true;
		    }

		    if (CurrentQuest.Id == 23 && ColliderQuest23ScriptCon.IsTriggered)
		    {
			    CurrentQuest.isFinished = true;	
		    }

		    if (CurrentQuest.Id == 25 && ColliderQuest25ScriptCon.IsTriggered)
		    {
			    CurrentQuest.isFinished = true;
		    }
		    
		    // Event Listeners for Ennemy Deaths
		    if (CurrentQuest.Id == 4 && EnemyContainer.Instance.CountEnemies() <= 2)
			    CurrentQuest.isFinished = true;
		    if ((CurrentQuest.Id == 6 || CurrentQuest.Id == 10 || CurrentQuest.Id == 11 || CurrentQuest.Id == 12 || CurrentQuest.Id == 17 || CurrentQuest.Id == 26 || CurrentQuest.Id == 28) && EnemyContainer.Instance.CountEnemies() == 0)
			    CurrentQuest.isFinished = true;
		    
		    
		    // Vérifier si la quête actuelle est finie
		    if (CurrentQuest.isFinished)
			    NextQuest();
		    if (Input.inputString == "m")
		    {
			    // Debug.Log("Touche M pressée ! Passage à la quête suivante.");
			    CurrentQuest.isFinished = true;
		    }
		    else if (Input.inputString == "n" && CurrentQuest.Type == GameplayEnum.Quote)
			    CurrentQuest.isFinished = true; // L'utilisateur a appuyé sur [N] pour marquer la quote comme lue.
	    }
	    else{
            // S'Execute en Boucle à la Fin du Jeu. (Une fois la toute dernière quete terminée)
            affichageQuote.text = "";
            textNextQuest.text = "";
	    }

		// [L] Réinitialiser les Missions (Peut être fait après la fin du jeu, contrairement à [M])
		if (Input.inputString == "l")
       	{
	        // Debug.Log("Touche L pressée ! Réinitialisation des missions et quêtes.");
            CurrentQuest.EndQuest();
			QuestRegistry.Instance.Reset();
			Start();	
       	}


    } 
}
