using Categorizer.Data;
using Categorizer.Engine;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Categorizer
{
    class Program
    {
        static void Main(string[] args)
        {
            int evalFLoor = 1;

            //DataFile
            string dataPath = ConfigurationManager.AppSettings["DataFile"];
            string[] lines = File.ReadAllLines(dataPath);

            CategoryBuilder categoryBuilder = new CategoryBuilder();
            categoryBuilder.ReadKnowledge();

            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(lines[i]);
                CategoryEvaluations categoryEvaluations = categoryBuilder.Run(lines[i]);
                CategoryEvaluation firstMatchingEvals = categoryEvaluations.GetByTextIndex(0).Where(a => a.Evaluation > evalFLoor).FirstOrDefault();
                string currCat;
                if (null == firstMatchingEvals)
                {
                    Console.WriteLine("choix catégorie :");
                    int selCat = AskCategoryMenu(categoryBuilder);
                    
                    if (selCat == categoryBuilder.Knowledge.Length)
                    {
                        Console.WriteLine("nom de la nouvelle catégorie :");
                        currCat = Console.ReadLine();
                        categoryBuilder.AddCategory(currCat);
                    }
                    else
                    {
                        currCat = categoryBuilder.Knowledge[selCat].Name;
                    }
                    string word = GetWord();
                    while ((word != @"$quit") && (word != @"qq"))
                    {
                        if (!string.IsNullOrWhiteSpace(word))
                            categoryBuilder.AddWordInCategory(currCat, word);
                        word = GetWord();
                    }
                }
                else
                {
                    currCat = firstMatchingEvals.CategoryName;
                }
                lines[i] += string.Format(";{0}", currCat);
            }
            categoryBuilder.SaveKnowledge();
            File.WriteAllLines(ConfigurationManager.AppSettings["OutFile"], lines);
            Console.WriteLine("appuyer sur entrée pour quitter");
            Console.ReadLine();

        }
        static private string GetWord()
        {
            Console.WriteLine("nouveau mot :");
            return Console.ReadLine();
        }

        static private int AskCategoryMenu(CategoryBuilder categoryBuilder)
        {
            int ret = -1;
            while (ret < 0 || ret > categoryBuilder.Knowledge.Length)
            {
                for (int i = 0; i < categoryBuilder.Knowledge.Length; i++)
                {
                    Console.WriteLine(string.Format("{0}: {1}", i.ToString().PadLeft(2, '0'), categoryBuilder.Knowledge[i].Name));
                }
                Console.WriteLine(string.Format("{0}: Nouvelle", categoryBuilder.Knowledge.Length.ToString().PadLeft(2, '0')));
                string response = Console.ReadLine();
                if (!int.TryParse(response, out ret))
                {
                    ret = -1;
                }
            }
            return ret;
        }
    }

}
