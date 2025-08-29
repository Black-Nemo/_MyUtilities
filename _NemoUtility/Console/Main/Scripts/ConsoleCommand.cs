using System;
using System.Collections.Generic;

namespace NemoUtility
{
    public class ConsoleCommand
    {
        public string Name;
        public Parameter FirstParameter;

        public List<string> GetValues(string[] strs)
        {
            Parameter tempParameter = FirstParameter;
            int counter = 1;
            while (tempParameter.NextParameter != null)
            {
                if (counter >= strs.Length) { return null; }
                if (tempParameter.IsTrue(strs[counter]))
                {
                    tempParameter = tempParameter.NextParameter;
                    counter++;
                }
                else
                {
                    break;
                }
            }
            if (counter >= strs.Length)
            {
                return null;
            }
            else
            {
                return tempParameter.GetValues(strs[counter]);
            }
        }
    }
    public class Parameter
    {
        public Parameter NextParameter;
        public Func<string, List<string>> ValuesFunc;
        public Func<string, bool> TrueFunc;

        public bool IsTrue(string str)
        {
            bool? result = TrueFunc?.Invoke(str);
            return result.Value;
        }

        public List<string> GetValues(string str)
        {
            return ValuesFunc?.Invoke(str);
        }
    }
}