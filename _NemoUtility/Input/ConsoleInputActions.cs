//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.14.0
//     from Assets/_MyUtilities/_NemoUtility/Console/Main/Input/ConsoleInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// Provides programmatic access to <see cref="InputActionAsset" />, <see cref="InputActionMap" />, <see cref="InputAction" /> and <see cref="InputControlScheme" /> instances defined in asset "Assets/_MyUtilities/_NemoUtility/Console/Main/Input/ConsoleInputActions.inputactions".
/// </summary>
/// <remarks>
/// This class is source generated and any manual edits will be discarded if the associated asset is reimported or modified.
/// </remarks>
/// <example>
/// <code>
/// using namespace UnityEngine;
/// using UnityEngine.InputSystem;
///
/// // Example of using an InputActionMap named "Player" from a UnityEngine.MonoBehaviour implementing callback interface.
/// public class Example : MonoBehaviour, MyActions.IPlayerActions
/// {
///     private MyActions_Actions m_Actions;                  // Source code representation of asset.
///     private MyActions_Actions.PlayerActions m_Player;     // Source code representation of action map.
///
///     void Awake()
///     {
///         m_Actions = new MyActions_Actions();              // Create asset object.
///         m_Player = m_Actions.Player;                      // Extract action map object.
///         m_Player.AddCallbacks(this);                      // Register callback interface IPlayerActions.
///     }
///
///     void OnDestroy()
///     {
///         m_Actions.Dispose();                              // Destroy asset object.
///     }
///
///     void OnEnable()
///     {
///         m_Player.Enable();                                // Enable all actions within map.
///     }
///
///     void OnDisable()
///     {
///         m_Player.Disable();                               // Disable all actions within map.
///     }
///
///     #region Interface implementation of MyActions.IPlayerActions
///
///     // Invoked when "Move" action is either started, performed or canceled.
///     public void OnMove(InputAction.CallbackContext context)
///     {
///         Debug.Log($"OnMove: {context.ReadValue&lt;Vector2&gt;()}");
///     }
///
///     // Invoked when "Attack" action is either started, performed or canceled.
///     public void OnAttack(InputAction.CallbackContext context)
///     {
///         Debug.Log($"OnAttack: {context.ReadValue&lt;float&gt;()}");
///     }
///
///     #endregion
/// }
/// </code>
/// </example>
public partial class @ConsoleInputActions: IInputActionCollection2, IDisposable
{
    /// <summary>
    /// Provides access to the underlying asset instance.
    /// </summary>
    public InputActionAsset asset { get; }

    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public @ConsoleInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ConsoleInputActions"",
    ""maps"": [
        {
            ""name"": ""Console"",
            ""id"": ""37564996-1bce-4fcf-ad4d-e5d0fbc4b443"",
            ""actions"": [
                {
                    ""name"": ""OpenConsole"",
                    ""type"": ""Button"",
                    ""id"": ""0eed67ac-416f-4778-9bcf-bb2f2e8e335e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""cfff2e2c-5856-4d83-9921-d58f37ae4914"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UpArrrow"",
                    ""type"": ""Button"",
                    ""id"": ""19490d7f-93f5-4932-a735-0d8ccdc6ee0b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DownArrow"",
                    ""type"": ""Button"",
                    ""id"": ""8b6cae79-8513-4c86-afb6-fa6ac03ef121"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""12e70b8c-7853-4d5f-9bbb-86b5b7a8abb8"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenConsole"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f746c38c-f1c8-4126-abea-7bbec19a5979"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae02527b-ac9f-49d2-b9e3-9e40bbc52fab"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpArrrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""221889e7-60ed-4506-9b42-bffa57339e29"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DownArrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Console
        m_Console = asset.FindActionMap("Console", throwIfNotFound: true);
        m_Console_OpenConsole = m_Console.FindAction("OpenConsole", throwIfNotFound: true);
        m_Console_Tab = m_Console.FindAction("Tab", throwIfNotFound: true);
        m_Console_UpArrrow = m_Console.FindAction("UpArrrow", throwIfNotFound: true);
        m_Console_DownArrow = m_Console.FindAction("DownArrow", throwIfNotFound: true);
    }

    ~@ConsoleInputActions()
    {
        UnityEngine.Debug.Assert(!m_Console.enabled, "This will cause a leak and performance issues, ConsoleInputActions.Console.Disable() has not been called.");
    }

    /// <summary>
    /// Destroys this asset and all associated <see cref="InputAction"/> instances.
    /// </summary>
    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindingMask" />
    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.devices" />
    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.controlSchemes" />
    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Contains(InputAction)" />
    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.GetEnumerator()" />
    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable.GetEnumerator()" />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Enable()" />
    public void Enable()
    {
        asset.Enable();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Disable()" />
    public void Disable()
    {
        asset.Disable();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindings" />
    public IEnumerable<InputBinding> bindings => asset.bindings;

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindAction(string, bool)" />
    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindBinding(InputBinding, out InputAction)" />
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Console
    private readonly InputActionMap m_Console;
    private List<IConsoleActions> m_ConsoleActionsCallbackInterfaces = new List<IConsoleActions>();
    private readonly InputAction m_Console_OpenConsole;
    private readonly InputAction m_Console_Tab;
    private readonly InputAction m_Console_UpArrrow;
    private readonly InputAction m_Console_DownArrow;
    /// <summary>
    /// Provides access to input actions defined in input action map "Console".
    /// </summary>
    public struct ConsoleActions
    {
        private @ConsoleInputActions m_Wrapper;

        /// <summary>
        /// Construct a new instance of the input action map wrapper class.
        /// </summary>
        public ConsoleActions(@ConsoleInputActions wrapper) { m_Wrapper = wrapper; }
        /// <summary>
        /// Provides access to the underlying input action "Console/OpenConsole".
        /// </summary>
        public InputAction @OpenConsole => m_Wrapper.m_Console_OpenConsole;
        /// <summary>
        /// Provides access to the underlying input action "Console/Tab".
        /// </summary>
        public InputAction @Tab => m_Wrapper.m_Console_Tab;
        /// <summary>
        /// Provides access to the underlying input action "Console/UpArrrow".
        /// </summary>
        public InputAction @UpArrrow => m_Wrapper.m_Console_UpArrrow;
        /// <summary>
        /// Provides access to the underlying input action "Console/DownArrow".
        /// </summary>
        public InputAction @DownArrow => m_Wrapper.m_Console_DownArrow;
        /// <summary>
        /// Provides access to the underlying input action map instance.
        /// </summary>
        public InputActionMap Get() { return m_Wrapper.m_Console; }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
        public void Enable() { Get().Enable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
        public void Disable() { Get().Disable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
        public bool enabled => Get().enabled;
        /// <summary>
        /// Implicitly converts an <see ref="ConsoleActions" /> to an <see ref="InputActionMap" /> instance.
        /// </summary>
        public static implicit operator InputActionMap(ConsoleActions set) { return set.Get(); }
        /// <summary>
        /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <param name="instance">Callback instance.</param>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
        /// </remarks>
        /// <seealso cref="ConsoleActions" />
        public void AddCallbacks(IConsoleActions instance)
        {
            if (instance == null || m_Wrapper.m_ConsoleActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ConsoleActionsCallbackInterfaces.Add(instance);
            @OpenConsole.started += instance.OnOpenConsole;
            @OpenConsole.performed += instance.OnOpenConsole;
            @OpenConsole.canceled += instance.OnOpenConsole;
            @Tab.started += instance.OnTab;
            @Tab.performed += instance.OnTab;
            @Tab.canceled += instance.OnTab;
            @UpArrrow.started += instance.OnUpArrrow;
            @UpArrrow.performed += instance.OnUpArrrow;
            @UpArrrow.canceled += instance.OnUpArrrow;
            @DownArrow.started += instance.OnDownArrow;
            @DownArrow.performed += instance.OnDownArrow;
            @DownArrow.canceled += instance.OnDownArrow;
        }

        /// <summary>
        /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <remarks>
        /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
        /// </remarks>
        /// <seealso cref="ConsoleActions" />
        private void UnregisterCallbacks(IConsoleActions instance)
        {
            @OpenConsole.started -= instance.OnOpenConsole;
            @OpenConsole.performed -= instance.OnOpenConsole;
            @OpenConsole.canceled -= instance.OnOpenConsole;
            @Tab.started -= instance.OnTab;
            @Tab.performed -= instance.OnTab;
            @Tab.canceled -= instance.OnTab;
            @UpArrrow.started -= instance.OnUpArrrow;
            @UpArrrow.performed -= instance.OnUpArrrow;
            @UpArrrow.canceled -= instance.OnUpArrrow;
            @DownArrow.started -= instance.OnDownArrow;
            @DownArrow.performed -= instance.OnDownArrow;
            @DownArrow.canceled -= instance.OnDownArrow;
        }

        /// <summary>
        /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="ConsoleActions.UnregisterCallbacks(IConsoleActions)" />.
        /// </summary>
        /// <seealso cref="ConsoleActions.UnregisterCallbacks(IConsoleActions)" />
        public void RemoveCallbacks(IConsoleActions instance)
        {
            if (m_Wrapper.m_ConsoleActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        /// <summary>
        /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
        /// </summary>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
        /// </remarks>
        /// <seealso cref="ConsoleActions.AddCallbacks(IConsoleActions)" />
        /// <seealso cref="ConsoleActions.RemoveCallbacks(IConsoleActions)" />
        /// <seealso cref="ConsoleActions.UnregisterCallbacks(IConsoleActions)" />
        public void SetCallbacks(IConsoleActions instance)
        {
            foreach (var item in m_Wrapper.m_ConsoleActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ConsoleActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    /// <summary>
    /// Provides a new <see cref="ConsoleActions" /> instance referencing this action map.
    /// </summary>
    public ConsoleActions @Console => new ConsoleActions(this);
    /// <summary>
    /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "Console" which allows adding and removing callbacks.
    /// </summary>
    /// <seealso cref="ConsoleActions.AddCallbacks(IConsoleActions)" />
    /// <seealso cref="ConsoleActions.RemoveCallbacks(IConsoleActions)" />
    public interface IConsoleActions
    {
        /// <summary>
        /// Method invoked when associated input action "OpenConsole" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnOpenConsole(InputAction.CallbackContext context);
        /// <summary>
        /// Method invoked when associated input action "Tab" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnTab(InputAction.CallbackContext context);
        /// <summary>
        /// Method invoked when associated input action "UpArrrow" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnUpArrrow(InputAction.CallbackContext context);
        /// <summary>
        /// Method invoked when associated input action "DownArrow" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnDownArrow(InputAction.CallbackContext context);
    }
}
