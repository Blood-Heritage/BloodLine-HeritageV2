using System.Collections.Generic;

namespace QuestSystem{
    public class Mission
    {
        public bool isFinished { get => QuestsQueue.Count == 0; }
        public Queue<Quest> QuestsQueue = new Queue<Quest>();
        public int MissionID;

        public Mission(int id)
        {
            MissionID = id;
            Queue<Quest> quests = null;
            
            switch (id)
            {
                case 1:
                    // Mission 1
                    Quest quest1 = new Quest( 1, "Bonsoir la nouvelle recrue. Maintenant que tu es l’un des nôtres, tu vas nous aider à éliminer un commanditaire rival important avec qui nous avons rendez-vous pour conclure la vente d’une cargaison. Tu n’as pas besoin d’en savoir plus mais rappelles-toi, cette mission est cruciale, si tu échoues, tu le regretteras. Il faut être courageux pour être un lâche dans la mafia.", 0, GameplayEnum.Quote);
                    Quest quest2 = new Quest( 1, "Éliminer la cible", 1, GameplayEnum.Order);
                    Quest quest3 = new Quest( 1, "C’est un piège ! Vite, élimine la cible !", 0, GameplayEnum.Quote);
                    Quest quest4 = new Quest( 1, "Éliminer rapidement la cible !", 1, GameplayEnum.Order);
                    Quest quest5 = new Quest( 1, "Cible éliminée. Abats les agents ennemis, vite !", 0, GameplayEnum.Quote);
                    Quest quest6 = new Quest( 1, "Éliminer les agents du FBI", 0, GameplayEnum.Order);

                    quests = new Queue<Quest>(new [] { quest1, quest2, quest3, quest4, quest5, quest6 });
                    break;
                case 2:

                    // Mission 2
                    Quest quest7 = new Quest( 2, "Tu es tombé dans un piège créé de toute pièce par les autorités et les agents fédéraux. Tout le réseau est tombé, mais il y a peut-être encore moyen de se venger. Va dans notre cache d’armes et récupère ce dont tu as besoin", 0, GameplayEnum.Quote);
                    Quest quest8 = new Quest( 2, "Aller dans la cache d'armes", 1, GameplayEnum.Order);
                    Quest quest9 = new Quest( 2, "J’ai entendu parler de cette opération. Quel bain de sang. Tiens, voilà des informations qui pourraient t’être utiles sur les agents qui t’ont tendu un piège.",  0, GameplayEnum.Quote);
                    Quest quest10 = new Quest( 2, "Éliminez l’agent du FBI à la 501e rue.", 1, GameplayEnum.Order);
                    Quest quest11 = new Quest( 2, "Éliminez l’agent du FBI à la 212e rue.", 1, GameplayEnum.Order);
                    Quest quest12 = new Quest( 2, "Éliminez l’agent du FBI à la 141e rue.", 1, GameplayEnum.Order);
                    Quest quest13 = new Quest( 2, "Retourner voir le contact.", 1, GameplayEnum.Order);
                    Quest quest14 = new Quest( 2, "L’officier du FBI responsable de l’opération est actuellement à la bibliothèque, mais il est escorté par trop d’ennemis pour le moment. Rend toi devant le parking et poses cette bombe pour distraire les agents.", 0, GameplayEnum.Quote);
                    Quest quest15 = new Quest( 2, "Poser une bombe au parking", 0, GameplayEnum.Order);
                    Quest quest16 = new Quest( 2, "“Attention, deux agents ! Élimine les !", 0, GameplayEnum.Quote);
                    Quest quest17 = new Quest( 2, "Éliminer les deux agents.", 2, GameplayEnum.Order);
                    Quest quest18 = new Quest( 2, "Retourner poser une bombe au parking", 2, GameplayEnum.Order);
                    Quest quest19 = new Quest( 2, "Bien. Eloignes-toi.", 0, GameplayEnum.Quote);
                    Quest quest20 = new Quest( 2, "Rendez vous à la bibliothèque et éliminez l’officier", 1, GameplayEnum.Order);
                    Quest quest21 = new Quest( 2, "L’officier n’est plus là. Il va falloir trouver mieux.", 0, GameplayEnum.Quote);

                    quests = new Queue<Quest>(new []
                    {
                        quest7, quest8, quest9, quest10, quest11, quest12, quest13, quest14, quest15, quest16, quest17,
                        quest18, quest19, quest20, quest21
                    });
                    break;
                case 3:

                    // Mission 3
                    Quest quest22 = new Quest( 3, "L’officier n’a pû aller qu’à deux endroits, soit au bureau fédéral, soit au centre-ville. Prie pour que ce soit la deuxième option, et rends-toi devant l'hôtel de ville.", 0, GameplayEnum.Quote);
                    Quest quest23 = new Quest( 3, "Aller à l'hôtel de ville.", 1, GameplayEnum.Order);
                    Quest quest24 = new Quest( 3, "Il n’était pas là. Il va falloir se rendre au bureau fédéral. Rends toi au 26 de la dixième avenue.", 0, GameplayEnum.Quote);
                    Quest quest25 = new Quest( 3, "Aller au bureau fédéral.", 1, GameplayEnum.Order);
                    Quest quest26 = new Quest( 3, "Éliminer les ennemis", 2, GameplayEnum.Order);
                    Quest quest27 = new Quest( 3, "Le voilà ! Le Lieutenant-Colonel Dark Vador ! Elimine-le et venge ta famille mafieuse !",  0, GameplayEnum.Quote);
                    Quest quest28 = new Quest( 3, "Éliminer l'officier du FBI.", 5, GameplayEnum.Order);
                    Quest quest29 = new Quest( 3, "Félicitations, tu as vengé tes camarades. Tu es désormais digne des plus grands mafieux de cette planète.", 10, GameplayEnum.Quote);
                    
                    quests = new Queue<Quest>(new [] { quest22, quest23, quest24, quest25, quest26, quest27, quest28, quest29 });
                    break;
            }
            
            if (quests != null)
                QuestsQueue = quests;
        }

        public void MissionStart()
        {
            
        }
    }
}
