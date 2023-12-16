
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOTS_GLISSES2._0
{
    public class Jeu
    {
        private Dictionnaire dico;
        private Plateau plateauCourant;
        private Joueur[] joueurs = new Joueur[2];   // il n'y a que deux joueurs par définition dans la consigne mais on n'aurait pu en mettre plus

        public Jeu(Dictionnaire dico, Plateau plateauCourant, Joueur[] tab)
        {
            this.dico = dico;
            this.plateauCourant = plateauCourant;
            this.joueurs = tab;
        }

        public Dictionnaire Dico
        {
            get { return dico; }
        }

        public Plateau PlateauCourant
        {
            get { return plateauCourant; }
        }

        public Joueur[] Joueurs
        {
            get { return joueurs; }
        }

        /// <summary>
        /// Fonction de jeu qui est appelé pourles deux types de parties (aléatoire et fichier). Ele est un peu longue mais la plupart des 
        /// commandes correspondent à de l'interfaçage. Par ailleurs il y a quand même la gestion du temps dans une boucle de jeu qui tourne tant 
        /// qu'un joueur n'a pas gagné, que le temps n'est pas écoulé ou que personne n'abandonne. Dans cette fonction, on se sert uniquement de fonctions que l'on a définie 
        /// dans d'autres classes. Les lignes importantes sont commentées.
        /// </summary>
        public void Partie()
        {
            // --------------- CHOIX DUREE D'UNE PARTIE ET DES MANCHES --------------- //

            Console.Clear();
            Console.SetCursorPosition(5, 5);
            Console.WriteLine("Une partie dure 1, 2 ou 3 minutes, tapez le numéro correspondant à votre choix");
            int dureePartie = 0;
            Console.SetCursorPosition(5, 7);
            dureePartie = Convert.ToInt32(Console.ReadLine());
            while (dureePartie != 1 && dureePartie != 2 && dureePartie != 3)
            {
                Console.SetCursorPosition(5, 3);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("INVALIDE");
                Console.ResetColor();
                Console.SetCursorPosition(5, 7);
                dureePartie = Convert.ToInt32(Console.ReadLine());
            } 
            Console.Clear();

            Console.SetCursorPosition(5, 5);
            Console.WriteLine("Une manche dure 10, 15 ou 20 secondes, tapez le numéro correspondant à votre choix");
            int dureeManche = 0;
            Console.SetCursorPosition(5, 7);
            dureeManche = Convert.ToInt32(Console.ReadLine());
            while (dureeManche != 10 && dureeManche != 15 && dureeManche != 20)
            {
                Console.SetCursorPosition(5, 3);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("INVALIDE");
                Console.ResetColor();
                Console.SetCursorPosition(5, 7);
                dureeManche = Convert.ToInt32(Console.ReadLine());
            }

            // --------------- -------------------------------------- --------------- //

            // ------- AFFICHAGE DU TEMPS RESTANT AVNT LE D2BUT DE LA PARTIE -------- //

            DateTime startMenu = DateTime.Now;
            Console.Clear();
            while (DateTime.Now - startMenu < TimeSpan.FromSeconds(3))
            {
                Console.SetCursorPosition(32, 5);
                Console.Write("La partie va commencer dans ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                TimeSpan tempsRestant = TimeSpan.FromSeconds(3) - (DateTime.Now - startMenu);
                Console.Write(tempsRestant);
                Console.ResetColor();
                Console.Write(" secondes ! ");
            }

            // --------------- -------------------------------------- --------------- //

            bool win = false;
            int i = 0;

            DateTime débutPartie = DateTime.Now;                      
            TimeSpan duréeGlobale = TimeSpan.FromMinutes(dureePartie);   //définition de la durée d'une partie 

            //  ------------------------ DEBUT DE LA PARTIE ------------------------  //

            while (!win && DateTime.Now - débutPartie < duréeGlobale && !(this.Joueurs[0].Ff && this.joueurs[1].Ff))
            {
                Console.Clear();
                Console.SetCursorPosition(39, 3);
                Console.WriteLine(this.joueurs[0].toString());
                Console.SetCursorPosition(39, 4);
                Console.WriteLine(this.joueurs[1].toString());
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(plateauCourant.toString());
                Console.SetCursorPosition(43, 8);
                Console.WriteLine("entrez un mot " + this.joueurs[i].Nom + " (ou 'ff' pour arrêter de jouer)");      //affichage de statistiques des joueurs

                DateTime début = DateTime.Now;
                TimeSpan durée = TimeSpan.FromSeconds(dureeManche);           //définition de la durée d'une manche

                string entrée = null;

                // ------- DEBUT MANCHE DU JOUEUR i ------- //

                if (!this.joueurs[i].Ff)
                {

                    while (DateTime.Now - début < durée)
                    {
                        Console.SetCursorPosition(43, 9);
                        entrée = Console.ReadLine().Trim();
                        string mot = entrée.ToLower();

                        if (mot == "ff")    //Si le joueur a bandonné, on passe son tour jusqu'à ce que la partie soit finie ou que l'autre joueur abandonne aussi
                        {
                            this.joueurs[i].Ff = true;
                            break;
                        }

                        if (DateTime.Now - début < durée && mot.Length >= 2 && !this.joueurs[i].Contient(mot)
                            && this.Dico.RechDichoRecursif(mot, 0, this.Dico.GetDictionnaire.Length - 1)
                            && this.plateauCourant.Recherche_mot(mot, this.plateauCourant.nombreApparitionsLettreSurPremiereLignePlateau(mot[0]))
                            && !string.IsNullOrEmpty(mot))             //Toutes les vérifications liées à la validité du mot. On vérfie en premier que le temps de jeu du
                                                                       //joueur n'est pas écoulé. Les condition sont dans l'ordre le plus optimisé possible afin de ne pas
                                                                       //appeler des fonctions pour rien.
                        {
                            this.joueurs[i].Add_Mot(mot);              //Si les conditions sont validés on ajoute le mot dans la liste des mots trouvéspar le joueur
                            this.joueurs[i].Add_Score(Program.calculScore(mot, this.plateauCourant));       //On ajoute à son score le nombre de points obtenues

                            Console.SetCursorPosition(43, 10);
                            Console.Write("Bravo " + this.joueurs[i].Nom + ", le mot " + mot + " était dans le plateau, tu remportes ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(Program.calculScore(mot, this.plateauCourant) + " points ! ");
                            Console.ResetColor();
                            while (DateTime.Now - début < durée)                    // On affiche le temps restant avant le prochain tour pour que ce soit équitable
                            {
                                Console.SetCursorPosition(0, 25);
                                Console.Write("Il reste ");
                                TimeSpan tempsRestant = durée - (DateTime.Now - début);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(tempsRestant);
                                Console.ResetColor();
                                Console.Write(" secondes avant le prochain tour");
                            }
                            break;
                        }
                        else            //Si une des conditions n'est pas valide, on retourne un message d'erreur et le joueur peut retenter d'écrire un moit s'il a le temps
                        {
                            Console.SetCursorPosition(39, 0);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Le mot est invalide ou ne figure pas dans le dicionnaire, réessayez.");
                            Console.SetCursorPosition(0, 20);
                            Console.ResetColor();
                        }


                        System.Threading.Thread.Sleep(100);

                        if ((DateTime.Now - début) >= durée)    //On re-vérifie pour éviter qu'un mot que le joueur ait écris et validé après son temps de jeu ne soit compté
                            break;

                    }
                }
                i++;                 //Fin de la manche du joueur
                if (i == 2)          // il n'y a que deux joueurs donc on itère sur les indices 0 et 1 du tableau de joueurs.
                    i = 0;
            } 

            // ---- FIN DE PARTIE ET RESULTATS ---- //
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 5);
            if (this.joueurs[0].Score > this.joueurs[1].Score)
                Console.WriteLine("Félicitations " + this.joueurs[0].Nom + ", tu remportes la partie avec " + this.joueurs[0].Score + " points !");
            else if (this.joueurs[0].Score < this.joueurs[1].Score)
                Console.WriteLine("Félicitations " + this.joueurs[1].Nom + ", tu remportes la partie avec " + this.joueurs[1].Score + " points !");
            else Console.WriteLine("Vous êtes ex aequo !");
            Console.ResetColor();
            Console.SetCursorPosition(20, 6);
            Console.WriteLine("Appuyez sur ENTRER pour quitter");
            Console.SetCursorPosition(20, 7);
            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
               //On attend que l'utilisateur appuie sur entrée sur continuer
            }
            // ----------------------------------- //
        }
    }
}
