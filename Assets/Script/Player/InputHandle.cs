//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Script/Player/InputHandle.inputactions
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

public partial class @InputHandle: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputHandle()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputHandle"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""622f9ccd-c3ca-4999-8c86-14018b587cd1"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""bb5beb06-b673-463a-9022-8a5877bf92d0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""1c66abf8-8eaa-4f3b-8e20-b27c350ad51e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""8628144d-ef17-4f84-92c9-c62b0cccc2e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Parry"",
                    ""type"": ""Button"",
                    ""id"": ""7134b0b8-55b9-492e-8248-04fbc6e30433"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""02c54256-0747-45a0-8300-2f6f47e1600a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Item00"",
                    ""type"": ""Button"",
                    ""id"": ""52dc29bd-7bf7-4d67-89cc-98eaf6020f50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Item01"",
                    ""type"": ""Button"",
                    ""id"": ""c90f7c43-1498-4dc9-af78-7f41d7139774"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenBag"",
                    ""type"": ""Button"",
                    ""id"": ""18b47e49-a793-49fa-b796-04cfdcdfccbb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Setting"",
                    ""type"": ""Button"",
                    ""id"": ""06f04c8e-0e86-4004-83db-e525853992c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Earthshatter"",
                    ""type"": ""Button"",
                    ""id"": ""6f337abe-7ca9-41fd-bf6a-23d77bf9c819"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5a9d8c3f-3496-4fbd-b29d-aa7f00870d16"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c6e109dd-ea44-4d45-b294-e883eee3015f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c36b8f0c-e29a-41b9-94f8-1be2179cf9da"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fe7cd135-ff46-4462-a4f8-b74d2e9b6f0a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1a7e3e08-96e5-4033-8ef6-5308c2d67a3e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8ff7152d-628a-40f8-937f-844371ec958b"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f94d438b-f6fc-47a9-b749-0278c65be467"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ff01d44-7d9f-431f-8770-a7663989d194"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f0c3965-86ed-4c76-b0f0-ae601903eb27"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38555b29-da6a-43e1-9674-2cb85d0d1217"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a4d731b-1e07-46c7-b516-4687b828f0de"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc81ba37-0aec-47ad-aa01-be3910b2c326"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item00"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3822b1c5-6d77-4476-a29b-2e51f81fe243"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenBag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8325cf7-04aa-4654-ae6f-fad8552f784b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Setting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b8fe395-0408-472e-bd42-6956aa4c97ef"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Earthshatter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aaa7188a-6877-4b74-9c85-2ce2a8ae3207"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item01"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SexAction"",
            ""id"": ""8f141a01-5965-4b53-acb7-5ff46b318815"",
            ""actions"": [
                {
                    ""name"": ""ResistHorizontal"",
                    ""type"": ""Value"",
                    ""id"": ""f2f3a9f7-8ad3-4c31-994e-774e71997e88"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""d7c6af16-8126-475a-ba1f-7a25fb57ab09"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResistHorizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""f5106350-7622-4c86-89c3-d1f1e2b33d6f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResistHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""93ea766c-2084-47c2-80c4-b0f45009cbf0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResistHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Dialog"",
            ""id"": ""02605d9c-ca80-4b49-8a48-c10fbac8b119"",
            ""actions"": [
                {
                    ""name"": ""Continue"",
                    ""type"": ""Button"",
                    ""id"": ""1a8f09ad-f512-4abb-947c-c705ca8089c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""613a5b98-775f-40b2-af4e-0cc97eb8368e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_Dash = m_Character.FindAction("Dash", throwIfNotFound: true);
        m_Character_Attack = m_Character.FindAction("Attack", throwIfNotFound: true);
        m_Character_Parry = m_Character.FindAction("Parry", throwIfNotFound: true);
        m_Character_Interact = m_Character.FindAction("Interact", throwIfNotFound: true);
        m_Character_Item00 = m_Character.FindAction("Item00", throwIfNotFound: true);
        m_Character_Item01 = m_Character.FindAction("Item01", throwIfNotFound: true);
        m_Character_OpenBag = m_Character.FindAction("OpenBag", throwIfNotFound: true);
        m_Character_Setting = m_Character.FindAction("Setting", throwIfNotFound: true);
        m_Character_Earthshatter = m_Character.FindAction("Earthshatter", throwIfNotFound: true);
        // SexAction
        m_SexAction = asset.FindActionMap("SexAction", throwIfNotFound: true);
        m_SexAction_ResistHorizontal = m_SexAction.FindAction("ResistHorizontal", throwIfNotFound: true);
        // Dialog
        m_Dialog = asset.FindActionMap("Dialog", throwIfNotFound: true);
        m_Dialog_Continue = m_Dialog.FindAction("Continue", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Character
    private readonly InputActionMap m_Character;
    private List<ICharacterActions> m_CharacterActionsCallbackInterfaces = new List<ICharacterActions>();
    private readonly InputAction m_Character_Movement;
    private readonly InputAction m_Character_Dash;
    private readonly InputAction m_Character_Attack;
    private readonly InputAction m_Character_Parry;
    private readonly InputAction m_Character_Interact;
    private readonly InputAction m_Character_Item00;
    private readonly InputAction m_Character_Item01;
    private readonly InputAction m_Character_OpenBag;
    private readonly InputAction m_Character_Setting;
    private readonly InputAction m_Character_Earthshatter;
    public struct CharacterActions
    {
        private @InputHandle m_Wrapper;
        public CharacterActions(@InputHandle wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @Dash => m_Wrapper.m_Character_Dash;
        public InputAction @Attack => m_Wrapper.m_Character_Attack;
        public InputAction @Parry => m_Wrapper.m_Character_Parry;
        public InputAction @Interact => m_Wrapper.m_Character_Interact;
        public InputAction @Item00 => m_Wrapper.m_Character_Item00;
        public InputAction @Item01 => m_Wrapper.m_Character_Item01;
        public InputAction @OpenBag => m_Wrapper.m_Character_OpenBag;
        public InputAction @Setting => m_Wrapper.m_Character_Setting;
        public InputAction @Earthshatter => m_Wrapper.m_Character_Earthshatter;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void AddCallbacks(ICharacterActions instance)
        {
            if (instance == null || m_Wrapper.m_CharacterActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Parry.started += instance.OnParry;
            @Parry.performed += instance.OnParry;
            @Parry.canceled += instance.OnParry;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @Item00.started += instance.OnItem00;
            @Item00.performed += instance.OnItem00;
            @Item00.canceled += instance.OnItem00;
            @Item01.started += instance.OnItem01;
            @Item01.performed += instance.OnItem01;
            @Item01.canceled += instance.OnItem01;
            @OpenBag.started += instance.OnOpenBag;
            @OpenBag.performed += instance.OnOpenBag;
            @OpenBag.canceled += instance.OnOpenBag;
            @Setting.started += instance.OnSetting;
            @Setting.performed += instance.OnSetting;
            @Setting.canceled += instance.OnSetting;
            @Earthshatter.started += instance.OnEarthshatter;
            @Earthshatter.performed += instance.OnEarthshatter;
            @Earthshatter.canceled += instance.OnEarthshatter;
        }

        private void UnregisterCallbacks(ICharacterActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Parry.started -= instance.OnParry;
            @Parry.performed -= instance.OnParry;
            @Parry.canceled -= instance.OnParry;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @Item00.started -= instance.OnItem00;
            @Item00.performed -= instance.OnItem00;
            @Item00.canceled -= instance.OnItem00;
            @Item01.started -= instance.OnItem01;
            @Item01.performed -= instance.OnItem01;
            @Item01.canceled -= instance.OnItem01;
            @OpenBag.started -= instance.OnOpenBag;
            @OpenBag.performed -= instance.OnOpenBag;
            @OpenBag.canceled -= instance.OnOpenBag;
            @Setting.started -= instance.OnSetting;
            @Setting.performed -= instance.OnSetting;
            @Setting.canceled -= instance.OnSetting;
            @Earthshatter.started -= instance.OnEarthshatter;
            @Earthshatter.performed -= instance.OnEarthshatter;
            @Earthshatter.canceled -= instance.OnEarthshatter;
        }

        public void RemoveCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICharacterActions instance)
        {
            foreach (var item in m_Wrapper.m_CharacterActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CharacterActions @Character => new CharacterActions(this);

    // SexAction
    private readonly InputActionMap m_SexAction;
    private List<ISexActionActions> m_SexActionActionsCallbackInterfaces = new List<ISexActionActions>();
    private readonly InputAction m_SexAction_ResistHorizontal;
    public struct SexActionActions
    {
        private @InputHandle m_Wrapper;
        public SexActionActions(@InputHandle wrapper) { m_Wrapper = wrapper; }
        public InputAction @ResistHorizontal => m_Wrapper.m_SexAction_ResistHorizontal;
        public InputActionMap Get() { return m_Wrapper.m_SexAction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SexActionActions set) { return set.Get(); }
        public void AddCallbacks(ISexActionActions instance)
        {
            if (instance == null || m_Wrapper.m_SexActionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_SexActionActionsCallbackInterfaces.Add(instance);
            @ResistHorizontal.started += instance.OnResistHorizontal;
            @ResistHorizontal.performed += instance.OnResistHorizontal;
            @ResistHorizontal.canceled += instance.OnResistHorizontal;
        }

        private void UnregisterCallbacks(ISexActionActions instance)
        {
            @ResistHorizontal.started -= instance.OnResistHorizontal;
            @ResistHorizontal.performed -= instance.OnResistHorizontal;
            @ResistHorizontal.canceled -= instance.OnResistHorizontal;
        }

        public void RemoveCallbacks(ISexActionActions instance)
        {
            if (m_Wrapper.m_SexActionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ISexActionActions instance)
        {
            foreach (var item in m_Wrapper.m_SexActionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_SexActionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public SexActionActions @SexAction => new SexActionActions(this);

    // Dialog
    private readonly InputActionMap m_Dialog;
    private List<IDialogActions> m_DialogActionsCallbackInterfaces = new List<IDialogActions>();
    private readonly InputAction m_Dialog_Continue;
    public struct DialogActions
    {
        private @InputHandle m_Wrapper;
        public DialogActions(@InputHandle wrapper) { m_Wrapper = wrapper; }
        public InputAction @Continue => m_Wrapper.m_Dialog_Continue;
        public InputActionMap Get() { return m_Wrapper.m_Dialog; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DialogActions set) { return set.Get(); }
        public void AddCallbacks(IDialogActions instance)
        {
            if (instance == null || m_Wrapper.m_DialogActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DialogActionsCallbackInterfaces.Add(instance);
            @Continue.started += instance.OnContinue;
            @Continue.performed += instance.OnContinue;
            @Continue.canceled += instance.OnContinue;
        }

        private void UnregisterCallbacks(IDialogActions instance)
        {
            @Continue.started -= instance.OnContinue;
            @Continue.performed -= instance.OnContinue;
            @Continue.canceled -= instance.OnContinue;
        }

        public void RemoveCallbacks(IDialogActions instance)
        {
            if (m_Wrapper.m_DialogActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDialogActions instance)
        {
            foreach (var item in m_Wrapper.m_DialogActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DialogActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DialogActions @Dialog => new DialogActions(this);
    public interface ICharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnParry(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnItem00(InputAction.CallbackContext context);
        void OnItem01(InputAction.CallbackContext context);
        void OnOpenBag(InputAction.CallbackContext context);
        void OnSetting(InputAction.CallbackContext context);
        void OnEarthshatter(InputAction.CallbackContext context);
    }
    public interface ISexActionActions
    {
        void OnResistHorizontal(InputAction.CallbackContext context);
    }
    public interface IDialogActions
    {
        void OnContinue(InputAction.CallbackContext context);
    }
}
