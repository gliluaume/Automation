using Categorizer.Data;
using Categorizer.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Categorizer
{
    class Program
    {
        static void Main(string[] args)
        {

            string s = @"https://fr-mg42.mail.yahoo.com/neo/launch?.rand=c42sul7bnqomg#3609056201";
            //var toto = s.Split(',', ';', '.', ':', '!', '?', '=', '+', '-', '[',']', '(', ')', '*', '|', '\\', '/', '#');

            CategoryBuilder categoryBuilder = new CategoryBuilder();
            categoryBuilder.ReadKnowledge();
            CategoryEvaluations categoryEvaluations = categoryBuilder.Run(s);
            if(categoryEvaluations.NumCategories == 0)
            {
                Console.WriteLine("nouvelle catégorie :");
                string newCat = Console.ReadLine();
                categoryBuilder.AddCategory(newCat);

                string word = GetWord();
                while (word != @"$quit")
                {
                    if(string.IsNullOrWhiteSpace(word))
                        categoryBuilder.AddWordInCategory(newCat, word);
                    word = GetWord();
                }

            }
            Console.WriteLine("appuyer sur entrée pour quitter");
            Console.ReadLine();

        }
        static private string GetWord()
        {
            Console.WriteLine("nouveau mot :");
            return Console.ReadLine();
        }
    }

}
