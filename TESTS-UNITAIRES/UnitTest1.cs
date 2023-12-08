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
    }
}