using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MOTS_GLISSES2._0
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestRechDicoRecursif()
        {
            Dictionnaire dico = new Dictionnaire("fr");
            dico.RemplirDico("Mots_Français.txt");
            dico.Tri_Rapide(dico.GetDictionnaire, 0, dico.GetDictionnaire.Length - 1);
            dico.InitMatLettres();

            string mot = "bonjour";
            bool result = dico.RechDichoRecursif(mot, 0, dico.GetDictionnaire.Length - 1);
            Assert.IsTrue(result);
        }

        [TestMethod]

        public void TestToRead()
        {
            Plateau plateau = new Plateau();
            plateau.RemplirTabLettresDepuisFichierLettre("Lettres.txt");
            bool result = plateau.ToRead("Test1.csv");
            Assert.IsTrue(result);
        }

        [TestMethod]

        public void TestContient()
        {
            Joueur j = new Joueur("elias", " ", 0);
            j.Add_Mot("bonjour");
            j.Add_Mot("bienvenue");
            bool result = j.Contient("bienvenue");
            Assert.IsTrue(result);
        }

        [TestMethod]

        public void TestRecherche_Mot()
        {
            Plateau plateau = new Plateau();
            plateau.RemplirTabLettresDepuisFichierLettre("Lettres.txt");
            plateau.ToRead("Test1.csv");    // Dans ce fichier, le mot maison est valide car le m est à la base
            string mot = "maison";
            bool result = plateau.Recherche_mot(mot, plateau.nombreApparitionsLettreSurPremiereLignePlateau(mot[0]));
            Assert.IsTrue(result);
        }

        [TestMethod]
        
        public void TestIsInDirectory()
        {
            bool result = Program.IsInDirectory("save.txt");
            Assert.IsTrue(result);
        }
    }
}