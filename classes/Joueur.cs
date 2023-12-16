using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MOTS_GLISSES2._0
{
    public class Joueur
    {
        private string nom;
        private string motTrouves;
        private int score;
        private bool ff = false;

        public Joueur(string nom, string motTrouves, int score)
        {
            this.nom = nom;
            this.motTrouves = motTrouves;
            this.score = score;
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public string MotTrouves
        {
            get { return motTrouves; }
            set { motTrouves = value; }
        }

        public bool Ff
        {
            get { return ff; }
            set { ff = value; } 
        }

        public void Add_Mot(string mot)
        {
            this.motTrouves += mot + " ";        //On ajoute un espace, ceci a de l'importance pour la fonction Contient
        }

        /// <summary>
        /// Simple méthode qui retourne une chaine de caractères contenant des informations sur le joueur actuel
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return (this.nom + " a un score de " + this.score + " et a trouvé les mots suivant : " + this.motTrouves);
        }

        public void Add_Score(int val)
        {
            this.score += val;              //Simple ajout
        }

        /// <summary>
        /// étant donne que les mots trouvés par l'utilisateur sont stockés par dna sune chaine de caractères et séparés par des espaces, on
        /// créé un tableau de chaines de caractères dans lequel on résupère chaque mot puis on effectue un simple parcour de tableau avec un 
        /// test d'égalité à chaque fois
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public bool Contient(string mot)
        {
            string[] tab_mots = this.motTrouves.Split(' ');
            for (int i = 0; i < tab_mots.Length; i++)
            {
                if (tab_mots[i] == mot)
                    return true;
            }
            return false;
        }
    }
}
