using System.Reflection;
using UnityEngine;

public abstract class ConsoleCommandLineData : MonoBehaviour
{
    public bool IsActiveCommands;
    public string LoginMethodName = "login";
    public string LoginPassword = "123456";

    public string[] Datas;

    public static string[] methodNames;

    [SerializeField] protected GameConsole gameConsole;

    public string red = "<color=#FF0000>";
    public string green = "<color=#00FF00>";
    public string blue = "<color=#0000FF>";
    public string yellow = "<color=#FFFF00>";
    public string cyan = "<color=#00FFFF>";
    public string magenta = "<color=#FF00FF>";
    public string black = "<color=#000000>";
    public string white = "<color=#FFFFFF>";
    public string gray = "<color=#808080>";
    public string orange = "<color=#FFA500>";
    public string pink = "<color=#FFC0CB>";
    public string purple = "<color=#800080>";
    public string brown = "<color=#A52A2A>";
    public string lime = "<color=#00FF00>";
    public string olive = "<color=#808000>";
    public string maroon = "<color=#800000>";
    public string navy = "<color=#000080>";
    public string teal = "<color=#008080>";
    public string gold = "<color=#FFD700>";
    public string silver = "<color=#C0C0C0>";
    public string beige = "<color=#F5F5DC>";
    public string lavender = "<color=#E6E6FA>";
    public string salmon = "<color=#FA8072>";
    public string coral = "<color=#FF7F50>";
    public string turquoise = "<color=#40E0D0>";
    public string indigo = "<color=#4B0082>";
    public string violet = "<color=#EE82EE>";
    public string ivory = "<color=#FFFFF0>";
    public string khaki = "<color=#F0E68C>";
    public string endColor = "</color>";

    protected virtual void Awake()
    {
        gameConsole = GetComponent<GameConsole>();
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
            gameConsole.Println(red + "!Please Login");
            return;
        }

        MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (method != null)
        {
            Invoke(methodName, delay);
        }
        else
        {
            gameConsole.Println(red + "!Command Not Found");
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
    }

    public void login()
    {
        if (Datas.Length > 2 && Datas.Length < 2) { gameConsole.Println(red + "!Invalid Value"); return; }

        if (Datas[1] == LoginPassword)
        {
            IsActiveCommands = true;
            gameConsole.Println(blue + "Login Done");
        }
        else
        {
            gameConsole.Println(red + "Password Incorrect");
        }

    }
}
