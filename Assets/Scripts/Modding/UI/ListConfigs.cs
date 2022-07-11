using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Automata.Modding{
    public class ListConfigs : OptionsMenu
    {
        public static Configurable SelectedConfigs {get{return selected;}}
        static Configurable selected;
        [SerializeField] string editWindow; 

        protected override void CreateMenu(){
            var configs = ResourceManager.GetAll<Configurable>();
            KeyValuePair<string, Type>[] configNames = configs.Select((a) => new KeyValuePair<string, Type>(a.ClassName, a.GetType())).ToArray();  
            container.DestroyChildren();
            for(int i = 0; i < configs.Length; ++i){
                KeyValuePair<string, Type> kvp = configNames[i];
                Configurable current = configs[i];
                OptionsMenuCreator.Item<Button> btn = OptionsMenuCreator.CreateRow<Button>(
                    container, OptionsMenuCreator.ItemType.BUTTON, null,
                    new LocalizedString(kvp.Key)
                );  

                btn.itemClass.onClick.AddListener(() => {
                    selected = current;
                    if(selected != null) WindowChanger.ChangeWindow(editWindow);
                });
            }
        }
    }
}

