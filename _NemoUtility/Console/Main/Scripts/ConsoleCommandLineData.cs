using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NemoUtility
{
    public abstract class ConsoleCommandLineData : MonoBehaviour
    {
        public bool IsActiveCommands;
        public string LoginMethodName = "login";
        public string LoginPassword = "123456";

        public string[] Datas;

        public static string[] methodNames;

        [SerializeField] protected GameConsole gameConsole;

        protected List<ConsoleCommand> _consoleCommand;
        protected virtual void Awake()
        {
            gameConsole = GetComponent<GameConsole>();
            ConfigureConsoleCommands();
        }
        protected virtual void ConfigureConsoleCommands()
        {
            _consoleCommand = new();
        }

        public void RunCommand(string[] strs)
        {
            DatasCreate(strs);
            InvokeSafely(strs[0], 0);
        }
        public void DatasCreate(string[] strs)
        {
            Datas = new string[0];
            if (strs.Length > 0)
            {
                Datas = new string[strs.Length];
                for (int i = 0; i < strs.Length; i++)
                {
                    Datas[i] = strs[i];
                }
            }
        }

        void InvokeSafely(string methodName, float delay)
        {
            if (!IsActiveCommands && methodName != LoginMethodName)
            {
                gameConsole.Println(ConsoleColors.red + "!Please Login" + ConsoleColors.endColor);
                return;
            }

            MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                Invoke(methodName, delay);
            }
            else
            {
                gameConsole.Println(ConsoleColors.red + "!Command Not Found" + ConsoleColors.endColor);
            }
        }

        public virtual void inputField_ValueChanged(string text)
        {
            string[] strs = text.Split(" ");
            DatasCreate(strs);
            if (methodNames.Length > 0)
            {
                gameConsole.consolePanel.infoThisCommondText.text = "";
                foreach (var item in methodNames)
                {
                    if (item.Length < strs[0].Length) { continue; }
                    if (item.Substring(0, strs[0].Length).ToLower() == strs[0].ToLower())
                    {
                        if (item != strs[0])
                        {
                            gameConsole.consolePanel.infoThisCommondText.text += item + "\n";
                        }
                    }
                }
            }
            else if (strs.Length == 0)
            {
                gameConsole.consolePanel.infoThisCommondText.text = "";
            }
            else
            {
                gameConsole.consolePanel.infoThisCommondText.text = "";
            }

            foreach (var item in _consoleCommand)
            {
                if (item.Name == strs[0])
                {
                    gameConsole.consolePanel.infoThisCommondText.text = "";
                    var l = item.GetValues(strs);
                    if (l != null)
                    {
                        foreach (var val in l)
                        {
                            gameConsole.consolePanel.infoThisCommondText.text += val + "\n";
                        }
                    }
                }
            }

            //Example
            /*
            if (strs[0] == "add_item" && SceneManager.GetActiveScene().name == "Game")
            {
                if (strs.Length < 2) { return; }
                gameConsole.consolePanel.infoThisCommondText.text = "";
                foreach (var item in GameRememberObjectManager.Instance.Items.Itemss)
                {
                    if (item.name.Length < strs[1].Length) { continue; }
                    if (item.name.Substring(0, strs[1].Length).ToLower() == strs[1].ToLower())
                    {
                        if (item.name != strs[1])
                        {
                            gameConsole.consolePanel.infoThisCommondText.text += item.name + "\n";
                        }
                    }
                }
            }
            */

        }

        public void login()
        {
            if (Datas.Length > 2 && Datas.Length < 2) { gameConsole.Println(ConsoleColors.red + "!Invalid Value"); return; }

            if (Datas[1] == LoginPassword)
            {
                IsActiveCommands = true;
                gameConsole.Println(ConsoleColors.blue + "Login Done" + ConsoleColors.endColor);
            }
            else
            {
                gameConsole.Println(ConsoleColors.red + "Password Incorrect" + ConsoleColors.endColor);
            }

        }
    }

}