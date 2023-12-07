using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MOTS_GLISSES2._0
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionnaire dico = new Dictionnaire("fr");
            dico.RemplirDico("Mots_Français.txt");
            dico.Tri_Rapide(dico.GetDictionnaire, 0, dico.GetDictionnaire.Length - 1);
            dico.InitMatLettres();

            string mot = "bonjour";
            bool result = dico.RechDichoRecursif(mot, 0, dico.GetDictionnaire.Length - 1);
            Assert.IsTrue(result);
        }
    }
}