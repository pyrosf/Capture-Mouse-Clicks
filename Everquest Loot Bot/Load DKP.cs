using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Everquest_Loot_Bot
{
    class Load_DKP
    {
        public class DKP
        {
            public string Name;
            public string Value;


        }

        public static Dictionary<string,string> LoadDKP(String FileName)
        {
            Dictionary<string,string> ReturnList = new Dictionary<string, string>();
            string Line = "";
            char DelChar = ',';
            using (StreamReader reader = new StreamReader(FileName))
            {
                while ((Line = reader.ReadLine()) != null)
                {
                    
                    string[] linesegments = Line.Split(DelChar);
                    if (linesegments[0] == "Name")
                        {continue;  }
                    DKP temp = new DKP();
                    temp.Name = linesegments[0];
                    temp.Value = linesegments[1];
                    ReturnList.Add(temp.Name, temp.Value);
                }
            }
            return ReturnList;
            }

        public static Dictionary<string, string> LoadTier(String FileName)
        {
            Dictionary<string, string> ReturnList = new Dictionary<string, string>();
            string Line = "";
            char DelChar = ',';
            using (StreamReader reader = new StreamReader(FileName))
            {
                while ((Line = reader.ReadLine()) != null)
                {

                    string[] linesegments = Line.Split(DelChar);
                    if (linesegments[0] == "Name")
                    { continue; }

                    ReturnList.Add(linesegments[0], linesegments[2]);
                }
            }
            return ReturnList;

        }
    
    }



}

