using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MOTS_GLISSES2._0
{
    public class Plateau
    {
        private static Lettre[] tableauLettres;
        private static Lettre[,] plateauJeu;
        private int[,] indexCrush;
        private static Random aleatoire = new Random();




        public Lettre[,] PlateauJeu
        {
            get { return plateauJeu; }
        }

        public Lettre[] TableauLettres
        {
            get { return tableauLettres; }
        }

        /// <summary>
        /// Cett fonction lit le fichier contenant les informations relatives à chaque lettres (poids et nombre max d'apparitions) et les
        /// sauvgarde dans un tableau de type Lettre en créeant une nouvelle instance de Lettre à chaque fois
        /// </summary>
        /// <param name="filename"></param>
        public void RemplirTabLettresDepuisFichierLettre(string filename)
        {
            int i = 0;

            StreamReader sr = new StreamReader(filename);
            tableauLettres = new Lettre[26];              //26 lettres dans l'alphabet
            try
            {
                while (!sr.EndOfStream)
                {
                    string ligne = sr.ReadLine();
                    string[] info = ligne.Split(",");

                    tableauLettres[i] = new Lettre(char.ToLower(char.Parse(info[0])), int.Parse(info[1]), int.Parse(info[2]), 0);  //Création d'une nouvelle Lettre
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur de lecture du fichier" + e.Message);
            }
            finally { sr.Close(); }
        }

        /// <summary>
        /// Fonction qui est appelée quand le joueur choisit de générer un plateau aléatoirement. Nous avons choisit un plateau de 8 par 8 pour respecter
        /// le nombre d'apparitions maximale de chaque lettres. Dans cette fonction, on instancie le plateau de jeu puis on le parcours en tirant un nombre
        /// aléatoire à chaque fois. Ce nombre correspond à un indice d'une lettre du tableau de lettres (donc compris entre 0 et 25). On veille à ce que 
        /// le nombre d'apparitions maximale de chaque lettre soit respecté. Le cas échéant, on tire un nouveau nombre aléatoire. Enfin on ajoute la lettre (de type Lettre)
        /// aux coordonnées correspondantes dans le plateau.
        /// </summary>
        public void RemplirPlateauDeJeu8par8()
        {

            plateauJeu = new Lettre[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int nombreAleatoire;
                    do
                    {
                        nombreAleatoire = aleatoire.Next(0, 26);
                    } while (tableauLettres[nombreAleatoire].NombreApparitionsActuel >= tableauLettres[nombreAleatoire].NombreApparitions);

                    plateauJeu[i, j] = tableauLettres[nombreAleatoire];
                    tableauLettres[nombreAleatoire].NombreApparitionsActuel++;
                }
            }

        }


        /// <summary>
        /// Fonction qui permet de sauvegarder un plateau dans jeu en l'écrivant dans un fichier .txt ou .csv à l'aide d'un StreamWriter. 
        /// Avant d'écire dans le fichier, le StreamWriter vide le fichier pour éviter des problèmes. Puis on parcours le plateau en écrivant
        /// la lettre correspondante dans le fichier en ajoutant un ';' entre chaque lettre (car c'est comme ça que l'on différencie les lettres).
        /// </summary>
        /// <param name="nomfile"></param>
        public void ToFile(string nomfile)
        {
            RemplirPlateauDeJeu8par8();

            StreamWriter sw = new StreamWriter(nomfile);
            sw.Write(string.Empty);
            try
            {
                for (int i = 0; i < plateauJeu.GetLength(0); i++)
                {
                    for (int j = 0; j < plateauJeu.GetLength(1); j++)
                    {
                        sw.Write(plateauJeu[i, j].Symbole);
                        if (j < plateauJeu.GetLength(1) - 1)
                        {
                            sw.Write(";");
                        }
                    }
                    sw.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur de suavgarde" + e.Message);
            }
            finally { sw.Close(); }
        }

        /// <summary>
        /// Complémetaire à la fonction ToFile, on utilise un StreamReader pour lire une instance de plateau à partir d'un fichier
        /// De cette manière, on créer une nouvelle lettre (de type Lettre) que l'on placeaux coordonnées correspondantes dans le plateau.
        /// </summary>
        /// <param name="nomfile">Le nom du fichier à lire</param>
        /// <returns></returns>
        public bool ToRead(string nomfile)
        {
            
            StreamReader sr = new StreamReader(nomfile);
            try
            {
                string[] contenu = File.ReadAllLines(nomfile);
                int lignes = contenu.Length;
                int colonnes = contenu[0].Split(';').Length;
                plateauJeu = new Lettre[lignes, colonnes];
                for (int i = 0; i < lignes; i++)
                {
                    string[] lettres = contenu[i].Split(";");
                    for (int j = 0; j < colonnes; j++)
                    {
                        for (int k = 0; k < tableauLettres.Length; k++)
                        {
                            if (Convert.ToChar(lettres[j]) == tableauLettres[k].Symbole)
                            {
                                plateauJeu[i, j] = new Lettre(Convert.ToChar(lettres[j]), tableauLettres[k].NombreApparitions, tableauLettres[k].Poids, tableauLettres[k].NombreApparitionsActuel);
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Erreur de lecture du fichier");
                return false;
            }
            finally { sr.Close(); }
            return true;


        }

        /// <summary>
        /// Fonction qui affiche le plateau en le parcourant et en ajoutant des caractères pour l'interfaçage (séparation des lettres)
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string str = null;
            for (int i = 0; i < plateauJeu.GetLength(0); i++)
            {
                for (int j = 0; j < plateauJeu.GetLength(1); j++)
                {
                    str += (plateauJeu[i, j].Symbole);
                    if (j < plateauJeu.GetLength(1) - 1)
                    {
                        str += " | ";
                    }
                }
                str += "\n";
                for (int k = 0; k < plateauJeu.GetLength(1); k++)
                {
                    if (i < plateauJeu.GetLength(0) - 1)
                        str += "---";
                }
                if (i < plateauJeu.GetLength(0) - 1)
                    str += "------";
                str += "\n";
            }
            return str;
        }

        /// <summary>
        /// Simple fonction qui retourne le nombre d'apparitions d'un lettres sur la base du plateau. Cette fonction sert uniquement pour la fonction Rercherche_mot.
        /// </summary>
        /// <param name="c">le caractère dont on veut connaître le nombre d'apparition sur la base de la matrice</param>
        /// <returns></returns>
        public int nombreApparitionsLettreSurPremiereLignePlateau(char c)
        {
            int cpt = 0;
            for (int j = 0; j < plateauJeu.GetLength(1); j++)
            {
                if (c == plateauJeu[plateauJeu.GetLength(0) - 1, j].Symbole)
                    cpt++;
            }
            return cpt;
        }

        public bool Recherche_mot(string mot, int n, int index = 0, bool premiereLettreSurLaBase = false, int i = 0, int j = 0)
        {
            if (!premiereLettreSurLaBase)
            {
                i = plateauJeu.GetLength(0) - 1;
                if (mot[index] == plateauJeu[i, j].Symbole && !plateauJeu[i, j].Found)   //il faudra penser à mettre le mot de l'utilisateur en minuscule avant
                {
                    plateauJeu[i, j].Found = true;
                    this.indexCrush = new int[mot.Length + 1, 2];
                    indexCrush[index, 0] = i;
                    indexCrush[index, 1] = j;
                    return Recherche_mot(mot, n, index + 1, true, i, j);
                }
                else if (j == plateauJeu.GetLength(1) - 1)
                {
                    return false;
                }
                else
                {
                    return Recherche_mot(mot, n, index, false, i, j + 1);
                }
            }
            else
            {
                if (index == mot.Length)     //condition d'arrêt
                {
                    this.indexCrush[index, 0] = i;
                    this.indexCrush[index, 1] = j;
                    StateIndicesCrush();              //ce n'est qu'à ce moment là que l'on valide que les lettres vont être crush pour faire tomber les autres
                    Maj_Plateau();
                    return true;
                }
                if (i != 0 && mot[index] == plateauJeu[i - 1, j].Symbole)     //Recherche verticale
                {
                    this.indexCrush[index, 0] = i;
                    this.indexCrush[index, 1] = j;
                    return Recherche_mot(mot, n, index + 1, true, i - 1, j);
                }
                else if (j != plateauJeu.GetLength(1) - 1 && mot[index] == plateauJeu[i, j + 1].Symbole)    //Recherche horizontale (droite)
                {
                    this.indexCrush[index, 0] = i;
                    this.indexCrush[index, 1] = j;
                    return Recherche_mot(mot, n, index + 1, true, i, j + 1);
                }
                else if (j != 0 && mot[index] == plateauJeu[i, j - 1].Symbole)     //Recherche horizontale (gauche)
                {
                    this.indexCrush[index, 0] = i;
                    this.indexCrush[index, 1] = j;
                    return Recherche_mot(mot, n, index + 1, true, i, j - 1);
                }
                else if (i != 0 && j != 0 && mot[index] == plateauJeu[i - 1, j - 1].Symbole)     //Recherche diagonale (gauche)
                {
                    this.indexCrush[index, 0] = i;
                    this.indexCrush[index, 1] = j;
                    return Recherche_mot(mot, n, index + 1, true, i - 1, j - 1);
                }
                else if (i != 0 && j != plateauJeu.GetLength(1) - 1 && mot[index] == plateauJeu[i - 1, j + 1].Symbole)   //Recherche diagonale (droite)
                {
                    this.indexCrush[index, 0] = i;
                    this.indexCrush[index, 1] = j;
                    return Recherche_mot(mot, n, index + 1, true, i - 1, j + 1);
                }
                else if (n > 1)
                    return Recherche_mot(mot, n - 1, 0, false, 0, 0);
                else
                    return false;
            }
        }

        void StateIndicesCrush()
        {

            for (int i = 0; i < plateauJeu.GetLength(0); i++)
            {
                for (int j = 0; j < plateauJeu.GetLength(1); j++)
                {
                    for (int k = 0; k < this.indexCrush.GetLength(0); k++)
                    {
                        if (this.indexCrush[k, 0] == i && this.indexCrush[k, 1] == j)
                        {
                            plateauJeu[i, j].Crush = true;
                        }
                    }
                }
            }
        }


        public void Maj_Plateau()
        {
            for (int i = 0; i < plateauJeu.GetLength(0); i++)
            {
                for (int j = 0; j < plateauJeu.GetLength(1); j++)
                {
                    if (plateauJeu[i, j].Crush)
                    {
                        plateauJeu[i, j].Symbole = ' ';
                    }
                }
            }
            for (int k = 0; k < plateauJeu.GetLength(0); k++)
            {

                for (int i = 0; i < plateauJeu.GetLength(0); i++)
                {
                    for (int j = 0; j < plateauJeu.GetLength(1); j++)
                    {
                        if (i < plateauJeu.GetLength(0) - 1 && plateauJeu[i + 1, j].Crush)
                        {
                            while (plateauJeu[i + 1, j].Crush && !plateauJeu[i, j].Crush)
                            {
                                Lettre tmp = plateauJeu[i + 1, j];
                                plateauJeu[i + 1, j] = plateauJeu[i, j];
                                plateauJeu[i, j] = tmp;
                            }
                        }

                    }
                }
            }
        }
    }
}
