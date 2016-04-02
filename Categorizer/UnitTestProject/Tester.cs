using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Categorizer.Data;
using Newtonsoft.Json;
using System.IO;
using Categorizer.Engine;
using System.Configuration;

namespace UnitTestProject
{
    [TestClass]
    public class Tester
    {
        private const string BASE_PATH = @"..\..\DataSet";
        private const string _mainTestKnowledge = "Knowledge.json";
        private const string _mainTestText = "text.json";

        private string _categoryBuilderWorkDir;
        private string _categoryBuilderKnowledgeFilepath;
        private Category[] _knowledge;
        private string[] _inStrings;

        public Tester()
        {
            _knowledge = DataLoad<Category[]>(_mainTestKnowledge);
            _inStrings = DataLoad<string[]>(_mainTestText);
            _categoryBuilderKnowledgeFilepath = ConfigurationManager.AppSettings["KnowledgePath"];
            _categoryBuilderWorkDir = Path.GetDirectoryName(_categoryBuilderKnowledgeFilepath);

            if (_categoryBuilderWorkDir == Path.GetDirectoryName(BASE_PATH))
                throw new ConfigurationErrorsException("Le chemin configuré pour les fichiers générés par les tests est le même que celui des jeux de données !");
        }

        [TestMethod]
        public void CategoryBuilderMustRecognizeAKnownCategory()
        {
            CategoryBuilder categoryBuilder = new CategoryBuilder(_knowledge);
            CategoryEvaluations categoryEvaluations = categoryBuilder.Run(_inStrings);

            Assert.IsTrue(categoryEvaluations.GetByTextIndex(0)[0].Evaluation > 0);
            Assert.IsTrue(categoryEvaluations.GetByTextIndex(1)[1].Evaluation > 0);
        }

        [TestMethod]
        public void CategoryBuilderMustRunWellOnASingleString()
        {
            CategoryBuilder categoryBuilder = new CategoryBuilder(_knowledge);
            CategoryEvaluations categoryEvaluations = categoryBuilder.Run(_inStrings[0]);

            Assert.IsTrue(categoryEvaluations.GetByTextIndex(0)[0].Evaluation > 0);
        }

        [TestMethod]
        public void CategoryBuilderCanLearnANewCategory()
        {
            CategoryBuilder categoryBuilder = new CategoryBuilder(_knowledge);
            CategoryEvaluations categoryEvaluations = categoryBuilder.Run(_inStrings);

            Assert.IsTrue(categoryEvaluations.GetByTextIndex(0)[0].Evaluation == categoryEvaluations.GetByTextIndex(3)[0].Evaluation);
            categoryBuilder.AddCategory("insectes");
            categoryBuilder.AddWordInCategory("insectes", "fourmi");
            categoryEvaluations = categoryBuilder.Run("fourmi"); 
            Assert.IsTrue(categoryEvaluations.GetByTextIndex(0)[categoryEvaluations.NumCategories - 1].Evaluation == 100);
        }

        [TestMethod]
        public void CategoryBuilderCanLearnSomethingNewAboutACategory()
        {
            CategoryBuilder categoryBuilder = new CategoryBuilder(_knowledge);
            CategoryEvaluations categoryEvaluations = categoryBuilder.Run(_inStrings);

            Assert.IsTrue(categoryEvaluations.GetByTextIndex(0)[0].Evaluation == categoryEvaluations.GetByTextIndex(3)[0].Evaluation);
            categoryBuilder.AddWordInCategory("animaux", "fourmi");
            categoryEvaluations = categoryBuilder.Run(_inStrings);
            Assert.IsTrue(categoryEvaluations.GetByTextIndex(0)[0].Evaluation < categoryEvaluations.GetByTextIndex(3)[0].Evaluation);
        }

        [TestMethod]
        public void CategoryBuilderMustNotRecognizeAnUnknownCategory()
        {
            CategoryBuilder categoryBuilder = new CategoryBuilder(_knowledge);
            CategoryEvaluations categoryEvaluations = categoryBuilder.Run(_inStrings);

            for (int i = 0; i < _knowledge.Length; i++)
            {
                Assert.IsTrue(categoryEvaluations.GetByTextIndex(7)[i].Evaluation == 0);
            }
        }

        [TestMethod]
        public void CategoryBuilderCanReadJsonFiles()
        {
            ResetWorkDir();

            CategoryBuilder categoryBuilder = new CategoryBuilder();
            categoryBuilder.ReadKnowledge();
            Assert.IsTrue(categoryBuilder.Knowledge.Length > 0);
        }

        [TestMethod]
        public void CategoryBuilderCanSaveKnowledgeAsJsonFile()
        {
            ResetWorkDir();

            CategoryBuilder categoryBuilder = new CategoryBuilder();
            categoryBuilder.ReadKnowledge();
            long knowledgeVolume = GetKnowledgeVolume(categoryBuilder);
            categoryBuilder.AddWordInCategory("animaux", "fourmi");
            categoryBuilder.SaveKnowledge();

            CategoryBuilder otherCategoryBuilder = new CategoryBuilder();
            otherCategoryBuilder.ReadKnowledge();
            long otherKnowledgeVolume = GetKnowledgeVolume(otherCategoryBuilder);
            Assert.IsTrue(knowledgeVolume == otherKnowledgeVolume - 1);
        }

        private long GetKnowledgeVolume(CategoryBuilder categoryBuilder)
        {
            long knowledgeVolume = 0;
            for (int i = 0; i < categoryBuilder.Knowledge.Length; i++)
            {
                knowledgeVolume += categoryBuilder.Knowledge[i].Words.Count;
            }
            return knowledgeVolume;
        }

        private void ResetWorkDir()
        {
            DeleteWorkDir();
            CopyKnowledgeToWorkDir();
        }
        private void CopyKnowledgeToWorkDir()
        {
            string path = BASE_PATH + @"\" + _mainTestKnowledge;
            File.Copy(path, _categoryBuilderKnowledgeFilepath);
        }
        private void DeleteWorkDir()
        {
            DirectoryInfo di = new DirectoryInfo(_categoryBuilderWorkDir);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        static private T DataLoad<T>(string filename)
        {
            string json = string.Empty;
            string path = BASE_PATH + @"\" + filename;
            using (StreamReader sr = new StreamReader(path))
            {
                json = sr.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
