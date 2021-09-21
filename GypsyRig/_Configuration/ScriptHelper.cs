using System;
using System.IO;
using System.Threading.Tasks;

namespace GypsyRig
{
    public static class ScriptHelper
    {
        public static async Task<string> GetScriptContents(string scriptName)
        {
            var exeDirectory = AppDomain.CurrentDomain.BaseDirectory
                .TrimEnd(Path.DirectorySeparatorChar);

            scriptName = scriptName
                .TrimStart('\\')
                .Replace('\\', Path.DirectorySeparatorChar);

            var scriptPath = (exeDirectory + Path.DirectorySeparatorChar + scriptName);
            var contents = default(string);

            try
            {
                using TextReader reader = new StreamReader(scriptPath);
                contents = await reader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error attempting to read SQL Script file {scriptPath}", ex);
            }

            return contents;
        }
    }
}
