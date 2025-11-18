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
            foreach (var panel in UIPanels)
            {
                if (panel.Name == panelName)
                {
                    panel.Panel.SetActive(true);
                    _menuNavigator.Push(panelName);
                }
                else
                {
                    panel.Panel.SetActive(false);
                }
            }
        }

        public virtual void OpenBackPanel()
        {
            _menuNavigator.Pop();
            OpenPanel(_menuNavigator.Peek());
        }
    }

    [System.Serializable]
    public class UIPanel
    {
        public string Name;
        public GameObject Panel;
    }
}
