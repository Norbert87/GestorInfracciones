using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GestorInfracciones.DAL
{
    public class JsonIORepository: IOInterface
    {
        protected  string filePath;

        public void create() {
            if (!File.Exists(filePath))
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                file.Close();
            }
        }

        public string read(){
            using (StreamReader jsonStream = File.OpenText(filePath))
            {
                return  jsonStream.ReadToEnd();
            }
        }

        public void write(string text) {
            File.WriteAllText(filePath, text);
        }

        protected string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                              .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }

    }
}