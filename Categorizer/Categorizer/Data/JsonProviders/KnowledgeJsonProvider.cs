using System;
using System.Configuration;
using System.IO;

namespace Categorizer.Data.JsonProviders
{
    public class KnowledgeJsonProvider : JsonProvider<Category[]>
    {
        public KnowledgeJsonProvider()
        {
            this.Filepath = ConfigurationManager.AppSettings["KnowledgePath"];
        }

        override public void Save(Category[] category)
        {
            if (File.Exists(this.Filepath))
            {
                string dir = Path.GetDirectoryName(this.Filepath);
                string time = DateTime.Now.ToString("yyyy-MM-dd.HH.mm.ss");
                string filename = Path.GetFileName(this.Filepath);
                string oldFilename = string.Format(@"{0}\{1}_{2}", dir, time, filename);

                File.Move(this.Filepath, oldFilename);
            }
            base.Save(category);
        }

        override public Category[] Read()
        {
            Category[] ret;
            string dir = Path.GetDirectoryName(this.Filepath);
            if (!File.Exists(Filepath))
            {
                ret = new Category[0];

            }
            else if(Directory.Exists(dir))
            {
                ret = base.Read();
            }
            else
            {
                throw new DirectoryNotFoundException(dir);
            }
            return ret;
        }
    }
}
