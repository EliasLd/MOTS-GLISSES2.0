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

        /// <summary>
        /// Fonction récursive qui recherche un mot sur le plateau à l'horizontal, la verticale et en diagonale en partant de la base.
        /// La première partie de la fonction permet de tester récursivement si la première lettre du mot entré est sur la base du plateau.
        /// Cette première partie dépend du booléen 'premiereLettreSurLaBase' initialisé à false. Si la lettre est trouvée, alors on rappelle la
        /// fonction récursivement en passant ce booléen à vrai ce qui permet de passer à la seconde partie de la fonction.
        /// Quand on passe à la seconde partie, il ne s'agit que d'une série de test sur la verticale, la diagonale et l'horizontale. A chaque fois,
        /// on sauvegarde les coordonnées du mot trouvés dans la matrice 'indexCrush'. Enfin on a une condition d'arrêt qui vérifie si on est arrivé à la fin du mot,
        /// ce qui signifie que le mot a été entièrement trouvé et qu'on peut appeler Maj_Plateau (expliquée plus loin). Si une lettre n'est pas trouvé lors de la recherche, 
        /// on retourne directement false et tout est annulé. Par ailleurs, il est possible que la première lettre du mot soit présente plusieurs fois sur la base du plateau 
        /// et qu'une seule ne mène au mot, c'est pour cela que l'on test aussi si n (le nb d'apparitions de la première lettre sur la base) est supérieur à 1. Dans ce cas, 
        /// si lors de la recherche on ne trouve pas le mot et que la première lettre apparait plusieurs fois sur la base, alors on rappelle la fonction récursivement pour 
        /// recommencer tout le processus. De plus comme la lettre précédente est balisée par 'Found', celle-ci ne sera pas reconsidérer puisque l'on teste si 'Found' est False.
        /// </summary>
        /// <param name="mot">le mot entré par l'utilisateur</param>
        /// <param name="n">le nombre d'apparitions de la première lettre sur la base de la matrice</param>
        /// <param name="index">l'index actuel sur la chaine de caractère qui décrit le mot</param>
        /// <param name="premiereLettreSurLaBase">booléen qui est false de base mais qui devient true si la première lettre du mot se situe sur la base de la matrice</param>
        /// <param name="i">indice ligne actuelle</param>
        /// <param name="j">indice colonne actuelle</param>
        /// <returns></returns>
        public bool Recherche_mot(string mot, int n, int index = 0, bool premiereLettreSurLaBase = false, int i = 0, int j = 0)
        {
            if (!premiereLettreSurLaBase)
            {
                i = plateauJeu.GetLength(0) - 1;
                if (mot[index] == plateauJeu[i, j].Symbole && !plateauJeu[i, j].Found)  //On a trouvé le mot 
                {
                    plateauJeu[i, j].Found = true;                         //On balise ce mot comme Found, cela sert dans le cas où la première lettre du mot apparaît plusieurs
                                                                           //fois sur la base du plateau. Cela permet de considérer toutes les éventualités. (on le verra par la suite)
                    this.indexCrush = new int[mot.Length + 1, 2];          //On initialise la matrice des indices des lettres à faire diparaitre avec la longueur du mot
                    indexCrush[index, 0] = i;                              //Sauvegarde la ligne du premier mot
                    indexCrush[index, 1] = j;                              //Sauvegarde de la colonne du premier mot
                    return Recherche_mot(mot, n, index + 1, true, i, j);   //Appel récursif en passant à true pour effectuer la recherche plus loin
                }
                else if (j == plateauJeu.GetLength(1) - 1)                 //On est arrivé au bout sans trouver la première lettre du mot
                {
                    return false;
                }
                else
                {
                    return Recherche_mot(mot, n, index, false, i, j + 1);  //La lettre n'a pas encore été trouvé donc on passe à l'index suivant
                }
            }
            else
            {
                if (index == mot.Length)                                   //condition d'arrêt
                { 
                    this.indexCrush[index, 0] = i;
                    this.indexCrush[index, 1] = j;
                    StateIndicesCrush();                                  //ce n'est qu'à ce moment là que l'on valide que les lettres vont être crush pour faire tomber les autres
                    Maj_Plateau();
                    return true;
                }
                if (i != 0 && mot[index] == plateauJeu[i - 1, j].Symbole) //Recherche verticale (vers le haut)
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
                else if (n > 1)                                           //Si le mot n'a pas été trouvé mais que sa première lettre apparaît plusieurs fois sur la base du plateau
                                                                          //on rappelle la fonction et on recommence la recherche depuis le début.
                    return Recherche_mot(mot, n - 1, 0, false, 0, 0);
                else
                    return false;
            }
        }

        /// <summary>
        /// Comme les indices des lettres trouvées ont été sauvegardés, il ne reste plus qu'à parcourir le plateau et la matrice
        /// des indices sauvegardés et de mettre l'attribut crush des lettres à true puisqu'elles vont être "crush" (cassées pour faire tomber les autres)
        /// </summary>
        void StateIndicesCrush()
        {
            for (int i = 0; i < plateauJeu.GetLength(0); i++)
            {
                for (int j = 0; j < plateauJeu.GetLength(1); j++)
                {
                    for (int k = 0; k < this.indexCrush.GetLength(0); k++)
                    {
                        if (this.indexCrush[k, 0] == i && this.indexCrush[k, 1] == j)   //On regarde si les indices correspondent sur la matrice
                        {
                            plateauJeu[i, j].Crush = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tout d'abord cette fonction parcourt le plateau et dès qu'elle trouve une Lettre dont l'attribut "crush" est TRUE, elle la remplace par un espace ' '.
        /// Puis on parcours le plateau k fois et à chaque fois que la lettre en dessous de la lettre actuel est Crush (crush = true), on inverse la lettre actuelle avec le
        /// ' '. Comme tout cela se fait dans un while, les espaces ' ' remontent à la manière d'une bulle jusqu'en haut du plateau.
        /// Remarque : le parcourt du tableau se fait lui même dans une boucle car sinon les espaces ne peuvent pas remonter jusqu'en haut
        /// </summary>
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
            for (int k = 0; k < plateauJeu.GetLength(0); k++)   //On parcour chaque ligne 
            {
                for (int i = 0; i < plateauJeu.GetLength(0); i++) //lignes
                {
                    for (int j = 0; j < plateauJeu.GetLength(1); j++)   //colonnes
                    {
                        if (i < plateauJeu.GetLength(0) - 1 && plateauJeu[i + 1, j].Crush)
                        {
                            while (plateauJeu[i + 1, j].Crush && !plateauJeu[i, j].Crush)    //Si la lettre en dessous est Crush
                            {
                                Lettre tmp = plateauJeu[i + 1, j];                           //On inverse la lettre et l'espace 
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
