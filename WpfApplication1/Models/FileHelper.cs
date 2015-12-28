using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models
{
    public static class FileHelper
    {
        public static List<string> GetInstructionsFromFile(string path, int step)
        {
            List<string> instructionsList = new List<string>();
            StringBuilder currentInstruction = new StringBuilder();

            using ( StreamReader reader = new StreamReader(path) )
            {
                string currentLine = reader.ReadLine();

                if ( currentLine != null && currentLine.StartsWith("# ") )
                {
                    while (currentLine != null)
                    {
                        currentInstruction.AppendLine(currentLine);
                        currentInstruction.Remove(0, 2);

                        currentLine = reader.ReadLine();

                        while (currentLine != null && currentLine.StartsWith("# ") == false)
                        {
                            currentInstruction.AppendLine(currentLine);

                            currentLine = reader.ReadLine();
                        }

                        instructionsList.Add( currentInstruction.ToString() );
                        currentInstruction.Clear();
                    }
                }
                else
                    throw new IOException("Błąd podczas odczytu pliku instrukcji.");
            }

            return instructionsList;
        }
    }
}
