using System;
using System.Collections.Generic;
using UnityEngine;

namespace NemoUtility
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private string _startPanelName;

        protected Stack<string> _menuNavigator = new Stack<string>();

        public List<UIPanel> UIPanels = new List<UIPanel>();

        public static PanelManager Instance;

        protected virtual void Awake()
        {
            Instance = this;
        }

        protected virtual void Start()
        {
            if (!String.IsNullOrEmpty(_startPanelName))
            {
                OpenPanel(_startPanelName);
            }
        }

        public virtual void OpenPanel(string panelName)
        {
            Debug.Log($"[PanelManager] Opening Panel: {panelName}");
            foreach (var panel in UIPanels)
            {
                if (panel.Panel == null)
                {
                    Debug.LogError($"[PanelManager] Panel '{panel.Name}' has a NULL GameObject reference!");
                    continue;
                }

                if (panel.Name == panelName)
                {
                    panel.Panel.SetActive(true);
                    Debug.Log($"[PanelManager] Set {panel.Name} to ACTIVE. GameObject: {panel.Panel.name}");
                    if (_menuNavigator.Count == 0 || _menuNavigator.Peek() != panelName)
                    {
                        _menuNavigator.Push(panelName);
                    }
                }
                else
                {
                    panel.Panel.SetActive(false);
                }
            }
        }

        public virtual void OpenBackPanel()
        {
            if (_menuNavigator.Count <= 1) return;
            
            _menuNavigator.Pop();
            OpenPanel(_menuNavigator.Peek());
        }

        public string GetMenuNavigatorPeek()
        {
            if (_menuNavigator.Count == 0) return "";
            return _menuNavigator.Peek();
        }
    }

    [System.Serializable]
    public class UIPanel
    {
        public string Name;
        public GameObject Panel;
    }
}
