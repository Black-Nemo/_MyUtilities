using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NemoUtility
{
    [DefaultExecutionOrder(-5000)]
    public class GameConsole : MonoBehaviour
    {
        public bool canUnityDebug;
        [SerializeField] private LogType[] _logTypes;
        [SerializeField] private LogColors[] _logColors;

        public static GameConsole Instance;

        private ConsoleCommandLineData consoleCommandLineData;

        public GameObject consoleObjectPrefab;
        [SerializeField] private Transform _spawnTransform;

        private GameObject consoleObject;
        public ConsolePanel consolePanel;
        private string beforeString;

        //TabInfo
        private int index = -1;
        private string indexStr;
        private string _strs;

        private ConsoleInputActions _consoleInputActions;
        private void OnEnable()
        {
            if (canUnityDebug)
            {
                Application.logMessageReceived += OnLog;
            }

        }
        private void OnDisable()
        {
            if (canUnityDebug)
            {
                Application.logMessageReceived -= OnLog;
            }
        }


        private void OnLog(string condition, string stackTrace, LogType type)
        {
            foreach (var item in _logTypes)
            {
                if (type == item)
                {
                    var c = _logColors.First(a => a.LogType == item).Color;
                    Println(c + "[UNITY]" + ConsoleColors.endColor + condition);
                }
            }
        }

        private void Awake()
        {
            _consoleInputActions = new ConsoleInputActions();

            _consoleInputActions.Console.Enable();

            _consoleInputActions.Console.OpenConsole.performed += (context) => { if (context.ReadValueAsButton()) { OpenCloseConsole(); } };
            _consoleInputActions.Console.Tab.performed += (context) => { if (context.ReadValueAsButton()) { Tab(); } };
            _consoleInputActions.Console.UpArrrow.performed += (context) => { if (context.ReadValueAsButton()) { UpArrow(); } };
            _consoleInputActions.Console.DownArrow.performed += (context) => { if (context.ReadValueAsButton()) { DownArrow(); } };

            consoleCommandLineData = GetComponent<ConsoleCommandLineData>();
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            if (consoleObject == null)
            {
                CreateConsole();
            }
            else if (consoleObject == null)
            {
                CreateConsole();
            }
        }

        private void CreateConsole()
        {
            consoleObject = Instantiate(consoleObjectPrefab, Vector3.zero, Quaternion.identity);
            consoleObject.transform.SetParent(_spawnTransform);
            consoleObject.transform.localPosition = Vector3.zero;

            RectTransform rectTransform = consoleObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);

            consolePanel = consoleObject.GetComponent<ConsolePanel>();
            consolePanel.beforeText.text = beforeString;
            consolePanel.inputField.onEndEdit.AddListener(EnterInputCommand);

            consolePanel.inputField.onValueChanged.AddListener(consoleCommandLineData.inputField_ValueChanged);
            consolePanel.inputField.onValueChanged.AddListener((a) =>
            {
                var strs = a.Split(" ");
                consoleCommandLineData.DatasCreate(strs);
                indexStr = "";
                index = -1;
                if (String.IsNullOrEmpty(a))
                {
                    consolePanel.infoThisCommondText.text = "";
                }
            });

            consolePanel.transform.localScale = Vector3.one;
            consoleObject.SetActive(false);
        }

        public void OpenCloseConsole()
        {
            if (consoleObject == null) { return; }
            if (consoleObject.activeSelf) { consolePanel.inputField.text = ""; }

            consoleObject.SetActive(!consoleObject.activeSelf);
            consolePanel.inputField.Select();
            consolePanel.inputField.ActivateInputField();
        }

        public void Tab()
        {
            if (!String.IsNullOrEmpty(indexStr))
            {
                if (consoleCommandLineData.Datas.Length > 0)
                {
                    consolePanel.inputField.text = consolePanel.inputField.text.Substring(0, consolePanel.inputField.text.Length - consoleCommandLineData.Datas[consoleCommandLineData.Datas.Length - 1].Length) + indexStr; ;

                    StartCoroutine(inputFieldEnumerator());

                    indexStr = "";
                    index = -1;
                }
            }
        }
        private IEnumerator inputFieldEnumerator()
        {
            yield return 0;
            int textLength = consolePanel.inputField.text.Length;
            consolePanel.inputField.caretPosition = textLength;
            consolePanel.inputField.selectionAnchorPosition = textLength;
            consolePanel.inputField.selectionFocusPosition = textLength;
            yield return 1;
        }


        public void UpArrow()
        {
            RefleshConsoleInfo();
            index--;
            List<string> strs = consolePanel.infoThisCommondText.text.Split("\n").ToList();
            strs.Remove("");
            strs.Remove(" ");
            if (index >= strs.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = strs.Count - 1;
            }
            consolePanel.infoThisCommondText.text = "";
            for (var i = 0; i < strs.Count; i++)
            {
                if (index == i)
                {
                    indexStr = strs[i];
                    strs[i] = ConsoleColors.red + strs[i] + ConsoleColors.endColor;
                }

                if (i != strs.Count - 1)
                {
                    strs[i] += "\n";
                }
                consolePanel.infoThisCommondText.text += strs[i];
            }
        }
        public void DownArrow()
        {
            RefleshConsoleInfo();
            index++;
            List<string> strs = consolePanel.infoThisCommondText.text.Split("\n").ToList();
            strs.Remove("");
            strs.Remove(" ");
            if (index >= strs.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = strs.Count - 1;
            }
            consolePanel.infoThisCommondText.text = "";
            for (var i = 0; i < strs.Count; i++)
            {
                if (index == i)
                {
                    indexStr = strs[i];
                    strs[i] = ConsoleColors.red + strs[i] + ConsoleColors.endColor;
                }
                if (i != strs.Count - 1)
                {
                    strs[i] += "\n";
                }
                consolePanel.infoThisCommondText.text += strs[i];
            }
        }

        private void Update()
        {
            //if (consoleObject == null) { CreateConsole(); return; }
            //else if (consolePanel == null) { CreateConsole(); return; }
        }

        private void RefleshConsoleInfo()
        {
            List<string> strs = consolePanel.infoThisCommondText.text.Split("\n").ToList();
            strs.Remove("");
            strs.Remove(" ");

            consolePanel.infoThisCommondText.text = "";
            for (var i = 0; i < strs.Count; i++)
            {
                if (index == i)
                {
                    if (String.IsNullOrEmpty(indexStr))
                    {
                        indexStr = strs[i];
                    }
                    else
                    {
                        strs[i] = indexStr;
                    }

                }
                if (i != strs.Count - 1)
                {
                    strs[i] += "\n";
                }
                consolePanel.infoThisCommondText.text += strs[i];
            }
        }

        private void EnterInputCommand(string _cmd)
        {
            if (!consoleObject.activeSelf || consolePanel.inputField.text == "") { return; }
            Println("<color=#FFFFFF>" + _cmd);
            consolePanel.inputField.text = "";
            consolePanel.inputField.Select();
            string[] strs = _cmd.Split(" ");
            consoleCommandLineData.RunCommand(strs);
        }

        public void Println(string str)
        {
            beforeString += str + " \n";
            consolePanel.beforeText.text += str + " \n";
            consolePanel.inputField.Select();
            consolePanel.inputField.ActivateInputField();
        }

        public void ClearConsole()
        {
            consolePanel.beforeText.text = "";
        }
    }
}