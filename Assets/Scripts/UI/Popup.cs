using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityEngine.UI{
    public class Popup : MonoBehaviour
    {
        public class Button{
            public string text;
            public Action onClick;

            public Button(string text, Action onClick){
                this.text = text;
                this.onClick = onClick;
            }

            public static Button CloseButton(string text, GameObject target){
                Action closer = () => target.SetActive(false);
                return new Button(text, closer);
            }
        }

        [SerializeField] TMPro.TextMeshProUGUI text;
        [SerializeField] Transform buttonRow;

        static Popup instance;
        static bool CheckInstance() {
            if(instance == null){
                Popup popup = GameObject.FindObjectOfType<Popup>(true);
                if(popup != null) instance = popup;
                else return false;
            }
            return true;
        }

        public static void Show(string message, params Button[] buttons){
            if(!CheckInstance()) return;
            instance.Set(message, buttons);
        }

        public void Set(string message, Button[] buttons){
            if(buttons == null || buttons.Length == 0){
                Button[] newArr = {Button.CloseButton("ok_prompt", gameObject)};
                buttons = newArr;
            }

            buttonRow.DestroyChildren();
            foreach(Button b in buttons){
                OptionsMenuCreator.Item<Button> btn = OptionsMenuCreator.Create<Button>(buttonRow,
                OptionsMenuCreator.ItemType.BUTTON, new LocalizedString(b.text));
                btn.itemClass.onClick = b.onClick;
            }
        }
    }
}
