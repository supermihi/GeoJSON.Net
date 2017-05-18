using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GeoJSON.Net.Tests
{
    public abstract class TestBase
    {
        private static string AssemblyDirectory
        {
            get
            {              
                var codeBase = AppContext.BaseDirectory;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return path;
                
            }
        }

        protected string GetExpectedJson([CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var projectFolder = GetType().Namespace.Split('.').Last();
            var path = Path.Combine(AssemblyDirectory, @".\", projectFolder, type + "_" + name + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }
    }
}