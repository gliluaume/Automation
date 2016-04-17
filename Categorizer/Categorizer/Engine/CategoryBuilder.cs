using Categorizer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Categorizer.Data.JsonProviders;

namespace Categorizer.Engine
{
    public class CategoryBuilder
    {
        public IDataProvider<Category[]> DataProvider { get; set; }
        private Category[] _knowledge;
        public Category[] Knowledge
        {
            get
            {
                return this._knowledge;
            }
        }
        
        private CategoryEvaluations _categoryEvaluations;

        public CategoryEvaluations Evaluations { get { return _categoryEvaluations; } }

        public CategoryBuilder()
        {
            this.DataProvider = new KnowledgeJsonProvider();
        }
        public CategoryBuilder(Category[] knowledge)
        {
            this.DataProvider = new KnowledgeJsonProvider();
            this._knowledge = knowledge;
        }


        public void AddCategory(string categoryName)
        {
            Category[] newCategory = new Category[1] {new Category() {
                Name = categoryName,
                Words = new HashSet<string>()
            } };
            this._knowledge = this._knowledge.Concat<Category>(newCategory).ToArray();
        }

        public void AddWordInCategory(string categoryName, string word)
        {
            for (int i = 0; i < this._knowledge.Length; i++)
            {
                if (this._knowledge[i].Name == categoryName)
                {
                    this._knowledge[i].Words.Add(word);
                }
            }
        }
        public void SaveKnowledge()
        {
            DataProvider.Save(_knowledge);
        }
        public void ReadKnowledge()
        {
            _knowledge = DataProvider.Read();
        }
        /// <summary>
        /// Lance la catégorisation de la chaîne de caractères fournie sur la base des connaissances positionnées en amont 
        /// </summary>
        /// <param name="text">Chaîne à catégoriser</param>
        /// <returns></returns>
        public CategoryEvaluations Run(string text)
        {
            return Run(new string[1] { text });
        }
        /// <summary>
        /// Lance la catégorisation de la liste de chaîne de caractères sur la base des connaissances positionnées en amont
        /// </summary>
        /// <param name="texts">Tableau de chaînes de caractères à catégoriser</param>
        /// <returns></returns>
        public CategoryEvaluations Run(string[] texts)
        {
            this._categoryEvaluations = new CategoryEvaluations(texts.Length, this._knowledge.Length);

            for (int i = 0; i < texts.Length; i++)
            {
                this._categoryEvaluations.SetByTextIndex(i, EvaluateString(texts[i]));
            }

            return this._categoryEvaluations;
        }

        private CategoryEvaluation[] EvaluateString(string inText)
        {
            CategoryEvaluation[] ret = new CategoryEvaluation[this._knowledge.Length];

            for (int i = 0; i < this._knowledge.Length; i++)
            {
                ret[i] = EvaluateCategory(inText, this._knowledge[i]);
            }

            return ret;
        }

        private CategoryEvaluation EvaluateCategory(string inText, Category category)
        {
            decimal matchedWords = 0;
            CategoryEvaluation eval = new CategoryEvaluation();
            eval.CategoryName = category.Name;

            if (category.Words.Count > 0)
            {
                foreach (string word in category.Words)
                {
                    if (inText.ToLower().Contains(word.ToLower()))
                    {
                        matchedWords++;
                    }
                }
                eval.Evaluation = (int)Math.Round(100 * matchedWords / category.Words.Count);
            }

            return eval;
        }
    }
}
