using System;

namespace Categorizer.Data
{
    public class CategoryEvaluations
    {
        private long _numTexts;
        private long _numCategories;

        public CategoryEvaluation[,] CategoryEvals { get; set; }
        public long NumTexts { get { return _numTexts; } }
        public long NumCategories { get { return _numCategories; } }

        public CategoryEvaluations(int numTexts, int numCategories)
        {
            _numTexts = numTexts;
            _numCategories = numCategories;
            CategoryEvals = new CategoryEvaluation[_numTexts, _numCategories];
        }

        public void SetByTextIndex(int textIndex, CategoryEvaluation[] textCatEvals)
        {
            if (textCatEvals.Length != _numCategories)
                throw new IndexOutOfRangeException(string.Format("Should be {0} long. It is {1} long !", _numCategories, textCatEvals.Length));

            for (int i = 0; i < textCatEvals.Length; i++)
            {
                this.CategoryEvals[textIndex, i] = textCatEvals[i];
            }
        }

        public CategoryEvaluation[] GetByTextIndex(int textIndex)
        {
            CategoryEvaluation[] ret = new CategoryEvaluation[_numCategories];

            for (int i = 0; i < this.NumCategories; i++)
            {
                ret[i] = this.CategoryEvals[textIndex, i];
            }

            return ret;
        }
        public void SetByCategoryIndex(int categoryIndex, CategoryEvaluation[] catTextEvals)
        {
            if (catTextEvals.Length != _numTexts)
                throw new IndexOutOfRangeException(string.Format("Should be {0} long. It is {1} long !", _numTexts, catTextEvals.Length));

            for (int i = 0; i < catTextEvals.Length; i++)
            {
                this.CategoryEvals[i, categoryIndex] = catTextEvals[i];
            }
        }
    }
}
