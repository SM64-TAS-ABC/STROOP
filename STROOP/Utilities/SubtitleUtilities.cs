using STROOP.Forms;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STROOP.Utilities
{
    public static class SubtitleUtilities
    {
        public static void FormatSubtitlesFromClipboard()
        {
            string clipboardText = Clipboard.GetText();
            List<string> inputLines = clipboardText.Split('\n').ToList();
            List<string> outputLines = ConvertLines(inputLines);
            string output = string.Join("\r\n", outputLines);
            InfoForm.ShowValue(output);
	    }

        public static List<string> ConvertLines(List<string> inputLines)
        {
            List<string> outputLines = new List<string>();
            bool inSubtitle = false;
            List<string> workingList = new List<string>();
            List<Subtitle> subtitleList = new List<Subtitle>();

            foreach (string inputLineVar in inputLines)
            {
                string inputLine = inputLineVar.Trim();
                if (inputLine.Length == 0) continue;

                if (!inSubtitle)
                {
                    if (isSubtitleStart(inputLine))
                    {
                        inSubtitle = true;
                        workingList.Add(cleanup(inputLine));
                    }
                    else
                    {
                        // do nothing
                    }
                }
                else
                { // inSubtitle
                    if (isSubtitleEnd(inputLine))
                    {
                        inSubtitle = false;
                        Subtitle subtitle = new Subtitle(workingList);
                        if (!subtitleList.Contains(subtitle))
                        {
                            foreach (string line in workingList)
                            {
                                outputLines.Add(line);
                            }
                            outputLines.Add("");
                            subtitleList.Add(subtitle);
                            Config.Print(subtitle + "\n");
                        }
                        workingList.Clear();
                    }
                    else
                    {
                        workingList.Add(cleanup(inputLine));
                    }
                }
            }
            //System.out.println("NUM SUBTITLES: " + subtitleList.size());
            return outputLines;
        }

        public static bool isSubtitleStart(string line)
        {
            return line.Contains("\\viewkind4");
        }

        public static bool isSubtitleEnd(string line)
        {
            return line.Contains("}");
        }

        public static string cleanup(string line)
        {
            line = removePrefix(line);
            line = removeSuffix(line);
            line = removeItalics(line);
            line = removeQuotes(line);
            return line;
        }

        public static string removePrefix(string line)
        {
            if (line.Length >= 10 && line.Substring(0, 10) == "\\viewkind4")
            {
                int firstSpace = line.IndexOf(" ");
                return line.Substring(firstSpace + 1);
            }
            return line;
        }

        public static string removeSuffix(string line)
        {
            if (line.Substring(line.Length - 4) == "\\par")
            {
                return line.Substring(0, line.Length - 4);
            }
            return line;
        }

        public static string removeItalics(string line)
        {
            line = line.Replace("\\i0 ", "");
            line = line.Replace("\\i ", "");
            return line;
        }

        public static string removeQuotes(string line)
        {
            line = line.Replace("\\rquote ", "'");
            return line;
        }

        private class Subtitle
        {
            public List<string> lines;

            public Subtitle(List<string> lines)
            {
                this.lines = new List<string>(lines);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Subtitle))
                {
                    return false;
                }
                Subtitle other = (Subtitle)obj;
                if (this.lines.Count != other.lines.Count) return false;
                for (int i = 0; i < this.lines.Count; i++)
                {
                    if (lines[i] != other.lines[i]) return false;
                }
                return true;
            }

            public override string ToString()
            {
                String output = "";
                foreach (string line in lines)
                {
                    if (output.Length != 0) output += " ";
                    output += line;
                }
                return output;
            }
        }
    }
} 
