// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/Inputs/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""dd36f12f-5921-4419-a474-ecaf42e7293f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""fb8a6063-5604-46af-9eef-828ba47e4f09"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""fa3b1fb2-47f7-413c-9d72-ee201fbb022d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""a4d4c017-e896-4e11-bc76-9b5bb2b2a5ef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""1fa7816a-cba1-498e-ac3c-6d3525ad0d3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Evade"",
                    ""type"": ""Button"",
                    ""id"": ""e6f40cf6-4033-49ef-a228-5f090f113b3d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""78a57190-5348-43fd-a8c1-afa6c6c835e9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Use Cheat"",
                    ""type"": ""Button"",
                    ""id"": ""31b1f09d-8fea-4076-8525-cad1dc12a88f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Charge, Throw Grenade"",
                    ""type"": ""Button"",
                    ""id"": ""321e1fac-c298-4c4f-b51c-472794207298"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Switch Grenade"",
                    ""type"": ""Button"",
                    ""id"": ""d4dae3b8-6915-4a0b-81b2-85b28f8619bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Melee Attack"",
                    ""type"": ""Button"",
                    ""id"": ""9957ef72-cef1-43b7-b1af-3bd471712ca5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Guard (Opening Shot)"",
                    ""type"": ""Button"",
                    ""id"": ""ea50218f-b733-42c2-bf98-4ed1bd4b13ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""d8298bf6-22e8-4953-b5c8-98e0c634a084"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Open Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""d8e2050d-6bae-4828-b94c-afb2a161cadd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Change Inventory Sort Type"",
                    ""type"": ""Button"",
                    ""id"": ""c955f565-83a4-4f12-9dda-1ff4920c3734"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Close Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""c1b429be-2556-4eaa-880b-47761668b3ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Switch Weapon Right"",
                    ""type"": ""Button"",
                    ""id"": ""bee4d1c4-3aaa-4da5-8b86-7fc4877c18eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Switch Weapon Left"",
                    ""type"": ""Button"",
                    ""id"": ""45b33321-e54b-4814-b838-3cbcd3b2951b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Favorite (Inventory)"",
                    ""type"": ""Button"",
                    ""id"": ""9e4dd330-0f2b-44d7-a836-d1617485e067"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dismantle (Inventory)"",
                    ""type"": ""Button"",
                    ""id"": ""fa89b09d-c638-4bf7-aab3-b5c417065428"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause Game"",
                    ""type"": ""Button"",
                    ""id"": ""ea9e391d-a693-4acf-a40b-1540e0d5ad06"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cursor"",
                    ""type"": ""Button"",
                    ""id"": ""c4935728-39fb-4077-80e4-3386323e8d26"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Add Loot Normal"",
                    ""type"": ""Button"",
                    ""id"": ""63a7e6e7-e88d-4027-b50c-c776fa407c6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Add Loot Favorite"",
                    ""type"": ""Button"",
                    ""id"": ""4ad70dd7-fb2d-4091-bbc7-a6b59d4beeff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""978bfe49-cc26-4a3d-ab7b-7d7a29327403"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""00ca640b-d935-4593-8157-c05846ea39b3"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e2062cb9-1b15-46a2-838c-2f8d72a0bdd9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""320bffee-a40b-4347-ac70-c210eb8bc73a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d2581a9b-1d11-4566-b27d-b92aff5fabbc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcfe95b8-67b9-4526-84b5-5d0bc98d6400"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c1f7a91b-d0fd-4a62-997e-7fb9b69bf235"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c8e490b-c610-4785-884f-f04217b23ca4"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""143bb1cd-cc10-4eca-a2f0-a3664166fe91"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05f6913d-c316-48b2-a6bb-e225f14c7960"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3126d7d4-6597-40bf-b02e-0f0f85efef48"",
                    ""path"": ""<XInputController>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7baa7041-fdc6-48b6-b0a4-292ceab39052"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e93f43e-efe6-4916-a925-fdcc11b6c8e6"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Evade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5549505-048a-4efd-a6d7-0da534774a8a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Evade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39f7a47b-92d8-4b98-9824-69e9aea4e6be"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3848ae50-9edd-4f4d-a8dc-d59e805ce347"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61adb124-ee6f-4f78-906b-1621696ab37c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Use Cheat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8ae2ded-c266-4f76-b47d-95eefccc08e2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Use Cheat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""264f0195-ef82-4b56-90ed-f4cc6958125a"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Charge, Throw Grenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""970a2fdc-0d05-4751-8447-a1ae4cf207e1"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Charge, Throw Grenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56a17720-6504-4c05-ae75-1fb3395d3986"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Switch Grenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""176a74fb-956d-4684-9a07-38dcb5cf6982"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Switch Grenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb9112a0-e000-423a-963e-7aa5cf691c78"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Melee Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bb07243-ab1b-4b6c-8c4a-1ec65fd502b1"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Melee Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab44767f-b938-4d5e-a973-3a3e041f83b6"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Guard (Opening Shot)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62b71c78-7d4d-49bc-99f4-f59e1e5ccf9e"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Guard (Opening Shot)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88b0a67d-c9ba-401f-9515-b479c48ad4c6"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac251926-d63c-4960-b090-8ba8fbc43182"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5607475-b316-4525-8550-c82b51e18659"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Open Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77776862-2768-4c56-bdcc-525bf06961cf"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Open Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e256354-5f21-4afe-9cf8-de66d7a98ce8"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Change Inventory Sort Type"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0cf531b-96d5-414c-828f-95be41f9c852"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Change Inventory Sort Type"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""650f5640-32c0-4fac-94c2-cfc25973a930"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Close Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eba3a2c4-f415-4bba-8f3e-c56408d69417"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Close Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df061098-f2e5-4d81-9046-fab6867d76ee"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Switch Weapon Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a70de1b-93fd-4291-8d32-100c993f8e83"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Switch Weapon Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""426d0f41-c43b-4013-a9e5-b6aa087e8385"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Switch Weapon Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad52c65c-ebc8-429c-9064-73045296f0a0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Switch Weapon Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""546c9af2-d895-4956-9b23-7679622041b6"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Favorite (Inventory)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23c59f4f-e75d-420b-b618-c544705efd31"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Favorite (Inventory)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e0cf2c7-819d-4c87-9582-386dc976cb4b"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dismantle (Inventory)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12bcd722-0864-418e-ace1-2f2c0a8d2bd3"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Dismantle (Inventory)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2acb8be-0f15-4e69-a2be-f85241b1ae70"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Pause Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16b1ab64-1919-431b-9301-57f8eaebe200"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d6cb6e0-23fb-40d3-8fb2-ff19adf2bc7b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Cursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c2fb0e4-6b76-490b-96d5-e082d3dce6db"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c3ed5ad-fadd-4f0a-9a53-bd5e46fade78"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Add Loot Normal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e87651b-c845-4bb3-98db-a1ace2c88797"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Add Loot Normal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38c16437-3f9a-4607-9b9a-4cc867f1e96b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Add Loot Favorite"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de509aaf-dc55-46e8-a4cc-130eee4c37c9"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Add Loot Favorite"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""aee3d6c6-fe08-484f-9038-c9a95242c817"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""0d401001-b662-4344-a0de-1ad50b76af6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cursor"",
                    ""type"": ""Button"",
                    ""id"": ""583b4d18-2dc6-4c8d-ac12-28932e478b25"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cursor Move"",
                    ""type"": ""Value"",
                    ""id"": ""0cd77c02-a216-4f58-8355-170a68956a56"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cursor Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""85b1626e-3427-4e30-9a88-c51fff8a275d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""L. Click"",
                    ""type"": ""Button"",
                    ""id"": ""e82bd30f-5eae-4a96-a151-33c47e47b305"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c353993a-ca61-488a-8b52-d845ba59cc14"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d0acee5-0daf-4ebc-a418-8960ffd47e16"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26182fd6-3720-4c29-ad82-08e47b2feace"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Cursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c68eb9a2-7ac8-4fb8-a328-e28c339c60ad"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ecebb62-5842-4ff5-9ba7-12ee034621a2"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cursor Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd5019d1-93a4-4af6-8980-a308a5d9184e"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cursor Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e15b9bf-3750-4cbc-a9d1-63df8934a0f6"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""L. Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
        m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        m_Player_Evade = m_Player.FindAction("Evade", throwIfNotFound: true);
        m_Player_Reload = m_Player.FindAction("Reload", throwIfNotFound: true);
        m_Player_UseCheat = m_Player.FindAction("Use Cheat", throwIfNotFound: true);
        m_Player_ChargeThrowGrenade = m_Player.FindAction("Charge, Throw Grenade", throwIfNotFound: true);
        m_Player_SwitchGrenade = m_Player.FindAction("Switch Grenade", throwIfNotFound: true);
        m_Player_MeleeAttack = m_Player.FindAction("Melee Attack", throwIfNotFound: true);
        m_Player_GuardOpeningShot = m_Player.FindAction("Guard (Opening Shot)", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_OpenInventory = m_Player.FindAction("Open Inventory", throwIfNotFound: true);
        m_Player_ChangeInventorySortType = m_Player.FindAction("Change Inventory Sort Type", throwIfNotFound: true);
        m_Player_CloseInventory = m_Player.FindAction("Close Inventory", throwIfNotFound: true);
        m_Player_SwitchWeaponRight = m_Player.FindAction("Switch Weapon Right", throwIfNotFound: true);
        m_Player_SwitchWeaponLeft = m_Player.FindAction("Switch Weapon Left", throwIfNotFound: true);
        m_Player_FavoriteInventory = m_Player.FindAction("Favorite (Inventory)", throwIfNotFound: true);
        m_Player_DismantleInventory = m_Player.FindAction("Dismantle (Inventory)", throwIfNotFound: true);
        m_Player_PauseGame = m_Player.FindAction("Pause Game", throwIfNotFound: true);
        m_Player_Cursor = m_Player.FindAction("Cursor", throwIfNotFound: true);
        m_Player_AddLootNormal = m_Player.FindAction("Add Loot Normal", throwIfNotFound: true);
        m_Player_AddLootFavorite = m_Player.FindAction("Add Loot Favorite", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Cursor = m_UI.FindAction("Cursor", throwIfNotFound: true);
        m_UI_CursorMove = m_UI.FindAction("Cursor Move", throwIfNotFound: true);
        m_UI_CursorScroll = m_UI.FindAction("Cursor Scroll", throwIfNotFound: true);
        m_UI_LClick = m_UI.FindAction("L. Click", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Fire;
    private readonly InputAction m_Player_Sprint;
    private readonly InputAction m_Player_Evade;
    private readonly InputAction m_Player_Reload;
    private readonly InputAction m_Player_UseCheat;
    private readonly InputAction m_Player_ChargeThrowGrenade;
    private readonly InputAction m_Player_SwitchGrenade;
    private readonly InputAction m_Player_MeleeAttack;
    private readonly InputAction m_Player_GuardOpeningShot;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_OpenInventory;
    private readonly InputAction m_Player_ChangeInventorySortType;
    private readonly InputAction m_Player_CloseInventory;
    private readonly InputAction m_Player_SwitchWeaponRight;
    private readonly InputAction m_Player_SwitchWeaponLeft;
    private readonly InputAction m_Player_FavoriteInventory;
    private readonly InputAction m_Player_DismantleInventory;
    private readonly InputAction m_Player_PauseGame;
    private readonly InputAction m_Player_Cursor;
    private readonly InputAction m_Player_AddLootNormal;
    private readonly InputAction m_Player_AddLootFavorite;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Fire => m_Wrapper.m_Player_Fire;
        public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
        public InputAction @Evade => m_Wrapper.m_Player_Evade;
        public InputAction @Reload => m_Wrapper.m_Player_Reload;
        public InputAction @UseCheat => m_Wrapper.m_Player_UseCheat;
        public InputAction @ChargeThrowGrenade => m_Wrapper.m_Player_ChargeThrowGrenade;
        public InputAction @SwitchGrenade => m_Wrapper.m_Player_SwitchGrenade;
        public InputAction @MeleeAttack => m_Wrapper.m_Player_MeleeAttack;
        public InputAction @GuardOpeningShot => m_Wrapper.m_Player_GuardOpeningShot;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @OpenInventory => m_Wrapper.m_Player_OpenInventory;
        public InputAction @ChangeInventorySortType => m_Wrapper.m_Player_ChangeInventorySortType;
        public InputAction @CloseInventory => m_Wrapper.m_Player_CloseInventory;
        public InputAction @SwitchWeaponRight => m_Wrapper.m_Player_SwitchWeaponRight;
        public InputAction @SwitchWeaponLeft => m_Wrapper.m_Player_SwitchWeaponLeft;
        public InputAction @FavoriteInventory => m_Wrapper.m_Player_FavoriteInventory;
        public InputAction @DismantleInventory => m_Wrapper.m_Player_DismantleInventory;
        public InputAction @PauseGame => m_Wrapper.m_Player_PauseGame;
        public InputAction @Cursor => m_Wrapper.m_Player_Cursor;
        public InputAction @AddLootNormal => m_Wrapper.m_Player_AddLootNormal;
        public InputAction @AddLootFavorite => m_Wrapper.m_Player_AddLootFavorite;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Fire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Sprint.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @Evade.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEvade;
                @Evade.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEvade;
                @Evade.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEvade;
                @Reload.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @UseCheat.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseCheat;
                @UseCheat.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseCheat;
                @UseCheat.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseCheat;
                @ChargeThrowGrenade.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChargeThrowGrenade;
                @ChargeThrowGrenade.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChargeThrowGrenade;
                @ChargeThrowGrenade.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChargeThrowGrenade;
                @SwitchGrenade.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchGrenade;
                @SwitchGrenade.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchGrenade;
                @SwitchGrenade.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchGrenade;
                @MeleeAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMeleeAttack;
                @MeleeAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMeleeAttack;
                @MeleeAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMeleeAttack;
                @GuardOpeningShot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGuardOpeningShot;
                @GuardOpeningShot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGuardOpeningShot;
                @GuardOpeningShot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGuardOpeningShot;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @OpenInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @ChangeInventorySortType.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeInventorySortType;
                @ChangeInventorySortType.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeInventorySortType;
                @ChangeInventorySortType.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeInventorySortType;
                @CloseInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseInventory;
                @CloseInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseInventory;
                @CloseInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCloseInventory;
                @SwitchWeaponRight.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchWeaponRight;
                @SwitchWeaponRight.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchWeaponRight;
                @SwitchWeaponRight.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchWeaponRight;
                @SwitchWeaponLeft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchWeaponLeft;
                @SwitchWeaponLeft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchWeaponLeft;
                @SwitchWeaponLeft.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchWeaponLeft;
                @FavoriteInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFavoriteInventory;
                @FavoriteInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFavoriteInventory;
                @FavoriteInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFavoriteInventory;
                @DismantleInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDismantleInventory;
                @DismantleInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDismantleInventory;
                @DismantleInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDismantleInventory;
                @PauseGame.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @Cursor.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCursor;
                @Cursor.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCursor;
                @Cursor.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCursor;
                @AddLootNormal.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddLootNormal;
                @AddLootNormal.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddLootNormal;
                @AddLootNormal.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddLootNormal;
                @AddLootFavorite.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddLootFavorite;
                @AddLootFavorite.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddLootFavorite;
                @AddLootFavorite.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddLootFavorite;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Evade.started += instance.OnEvade;
                @Evade.performed += instance.OnEvade;
                @Evade.canceled += instance.OnEvade;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @UseCheat.started += instance.OnUseCheat;
                @UseCheat.performed += instance.OnUseCheat;
                @UseCheat.canceled += instance.OnUseCheat;
                @ChargeThrowGrenade.started += instance.OnChargeThrowGrenade;
                @ChargeThrowGrenade.performed += instance.OnChargeThrowGrenade;
                @ChargeThrowGrenade.canceled += instance.OnChargeThrowGrenade;
                @SwitchGrenade.started += instance.OnSwitchGrenade;
                @SwitchGrenade.performed += instance.OnSwitchGrenade;
                @SwitchGrenade.canceled += instance.OnSwitchGrenade;
                @MeleeAttack.started += instance.OnMeleeAttack;
                @MeleeAttack.performed += instance.OnMeleeAttack;
                @MeleeAttack.canceled += instance.OnMeleeAttack;
                @GuardOpeningShot.started += instance.OnGuardOpeningShot;
                @GuardOpeningShot.performed += instance.OnGuardOpeningShot;
                @GuardOpeningShot.canceled += instance.OnGuardOpeningShot;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @OpenInventory.started += instance.OnOpenInventory;
                @OpenInventory.performed += instance.OnOpenInventory;
                @OpenInventory.canceled += instance.OnOpenInventory;
                @ChangeInventorySortType.started += instance.OnChangeInventorySortType;
                @ChangeInventorySortType.performed += instance.OnChangeInventorySortType;
                @ChangeInventorySortType.canceled += instance.OnChangeInventorySortType;
                @CloseInventory.started += instance.OnCloseInventory;
                @CloseInventory.performed += instance.OnCloseInventory;
                @CloseInventory.canceled += instance.OnCloseInventory;
                @SwitchWeaponRight.started += instance.OnSwitchWeaponRight;
                @SwitchWeaponRight.performed += instance.OnSwitchWeaponRight;
                @SwitchWeaponRight.canceled += instance.OnSwitchWeaponRight;
                @SwitchWeaponLeft.started += instance.OnSwitchWeaponLeft;
                @SwitchWeaponLeft.performed += instance.OnSwitchWeaponLeft;
                @SwitchWeaponLeft.canceled += instance.OnSwitchWeaponLeft;
                @FavoriteInventory.started += instance.OnFavoriteInventory;
                @FavoriteInventory.performed += instance.OnFavoriteInventory;
                @FavoriteInventory.canceled += instance.OnFavoriteInventory;
                @DismantleInventory.started += instance.OnDismantleInventory;
                @DismantleInventory.performed += instance.OnDismantleInventory;
                @DismantleInventory.canceled += instance.OnDismantleInventory;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @Cursor.started += instance.OnCursor;
                @Cursor.performed += instance.OnCursor;
                @Cursor.canceled += instance.OnCursor;
                @AddLootNormal.started += instance.OnAddLootNormal;
                @AddLootNormal.performed += instance.OnAddLootNormal;
                @AddLootNormal.canceled += instance.OnAddLootNormal;
                @AddLootFavorite.started += instance.OnAddLootFavorite;
                @AddLootFavorite.performed += instance.OnAddLootFavorite;
                @AddLootFavorite.canceled += instance.OnAddLootFavorite;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Cursor;
    private readonly InputAction m_UI_CursorMove;
    private readonly InputAction m_UI_CursorScroll;
    private readonly InputAction m_UI_LClick;
    public struct UIActions
    {
        private @Controls m_Wrapper;
        public UIActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Cursor => m_Wrapper.m_UI_Cursor;
        public InputAction @CursorMove => m_Wrapper.m_UI_CursorMove;
        public InputAction @CursorScroll => m_Wrapper.m_UI_CursorScroll;
        public InputAction @LClick => m_Wrapper.m_UI_LClick;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Cursor.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCursor;
                @Cursor.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCursor;
                @Cursor.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCursor;
                @CursorMove.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCursorMove;
                @CursorMove.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCursorMove;
                @CursorMove.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCursorMove;
                @CursorScroll.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCursorScroll;
                @CursorScroll.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCursorScroll;
                @CursorScroll.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCursorScroll;
                @LClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnLClick;
                @LClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnLClick;
                @LClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnLClick;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Cursor.started += instance.OnCursor;
                @Cursor.performed += instance.OnCursor;
                @Cursor.canceled += instance.OnCursor;
                @CursorMove.started += instance.OnCursorMove;
                @CursorMove.performed += instance.OnCursorMove;
                @CursorMove.canceled += instance.OnCursorMove;
                @CursorScroll.started += instance.OnCursorScroll;
                @CursorScroll.performed += instance.OnCursorScroll;
                @CursorScroll.canceled += instance.OnCursorScroll;
                @LClick.started += instance.OnLClick;
                @LClick.performed += instance.OnLClick;
                @LClick.canceled += instance.OnLClick;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnEvade(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnUseCheat(InputAction.CallbackContext context);
        void OnChargeThrowGrenade(InputAction.CallbackContext context);
        void OnSwitchGrenade(InputAction.CallbackContext context);
        void OnMeleeAttack(InputAction.CallbackContext context);
        void OnGuardOpeningShot(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnOpenInventory(InputAction.CallbackContext context);
        void OnChangeInventorySortType(InputAction.CallbackContext context);
        void OnCloseInventory(InputAction.CallbackContext context);
        void OnSwitchWeaponRight(InputAction.CallbackContext context);
        void OnSwitchWeaponLeft(InputAction.CallbackContext context);
        void OnFavoriteInventory(InputAction.CallbackContext context);
        void OnDismantleInventory(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnCursor(InputAction.CallbackContext context);
        void OnAddLootNormal(InputAction.CallbackContext context);
        void OnAddLootFavorite(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnCursor(InputAction.CallbackContext context);
        void OnCursorMove(InputAction.CallbackContext context);
        void OnCursorScroll(InputAction.CallbackContext context);
        void OnLClick(InputAction.CallbackContext context);
    }
}
