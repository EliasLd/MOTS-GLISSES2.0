
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

        public void Partie()
        {
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


            bool win = false;
            int i = 0;
            DateTime débutPartie = DateTime.Now;
            TimeSpan duréeGlobale = TimeSpan.FromSeconds(15);   //test pour une partie qui dure 15 secondes en tout

            while (!win && DateTime.Now - débutPartie < duréeGlobale)
            {
                Console.Clear();
                Console.SetCursorPosition(39, 3);
                Console.WriteLine(this.joueurs[0].toString());
                Console.SetCursorPosition(39, 4);
                Console.WriteLine(this.joueurs[1].toString());
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(plateauCourant.toString());
                Console.WriteLine("entrez un mot " + this.joueurs[i].Nom);

                DateTime début = DateTime.Now;
                TimeSpan durée = TimeSpan.FromSeconds(5);           //test pour le tour d'un joueur qui dure 5 secondes

                string mot = null;



                while (DateTime.Now - début < durée)  //une minute en millisecondes
                {
                    
                    mot = Console.ReadLine().Trim();

                    if (!this.joueurs[i].Contient(mot) && mot.Length >= 2 && DateTime.Now - début < durée
                        && this.plateauCourant.Recherche_mot(mot, this.plateauCourant.nombreApparitionsLettreSurPremiereLignePlateau(mot[0]))
                        && this.Dico.RechDichoRecursif(mot, 0, this.Dico.GetDictionnaire.Length - 1) && !string.IsNullOrEmpty(mot))
                    {
                        this.joueurs[i].Add_Mot(mot);
                        this.joueurs[i].Add_Score(Program.calculScore(mot, this.plateauCourant));

                        Console.Write("Bravo " + this.joueurs[i].Nom + ", le mot " + mot + " était dans le plateau, tu remportes ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(Program.calculScore(mot, this.plateauCourant) + " points ! ");
                        Console.ResetColor();
                        while (DateTime.Now - début < durée)
                        {
                            Console.SetCursorPosition(0, 21);
                            Console.Write("Il reste ");
                            TimeSpan tempsRestant = durée - (DateTime.Now - début);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(tempsRestant);
                            Console.ResetColor();
                            Console.WriteLine(" secondes avant le prochain tour");
                        }
                        break;
                    }
                    else 
                    {
                        Console.SetCursorPosition(39, 0);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Le mot est invalide ou ne figure pas dans le dicionnaire, réessayez.");
                        Console.SetCursorPosition(0, 20);
                        Console.ResetColor();
                    }
                   

                    System.Threading.Thread.Sleep(100);

                    if ((DateTime.Now - début) >= durée)    //pour éviter qu'un mot que le joueur ait écris et validé après son temps de jeu ne soit compté
                        break;

                }

                mot = null;

                i++;
                if (i == 2)
                    i = 0;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 5);
            if (this.joueurs[0].Score > this.joueurs[1].Score)
                Console.WriteLine("Félicitations " + this.joueurs[0].Nom + " tu remportes la partie avec " + this.joueurs[0].Score + " points !");
            else if (this.joueurs[0].Score < this.joueurs[1].Score)
                Console.WriteLine("Félicitations " + this.joueurs[1].Nom + " tu remportes la partie avec " + this.joueurs[1].Score + " points !");
            else Console.WriteLine("Vous êtes ex aequo !");
            Console.ResetColor();
            Console.SetCursorPosition(20, 6);
            Console.WriteLine("Appuyez sur ENTRER pour quitter");
            Console.SetCursorPosition(20, 7);
            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
               
            }
        }
    }
}
