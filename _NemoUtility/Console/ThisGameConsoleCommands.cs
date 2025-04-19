public class ThisGameConsoleCommands : ConsoleCommandLineData
{
    protected override void Awake()
    {
        base.Awake();
        methodNames = new string[]{
            "help"
        };
    }

    public void help()
    {
        if (Datas.Length > 1 && Datas.Length < 1) { gameConsole.Println(red + "!Invalid Value"); return; }

        gameConsole.Println(gold + "help");
    }

    public override void inputField_ValueChanged(string text)
    {
        base.inputField_ValueChanged(text);
        string[] strs = text.Split(" ");

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
}
