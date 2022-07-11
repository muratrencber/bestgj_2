using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automata.Modding{
    public class EditConfigurable : OptionsMenu
    {
        protected override void CreateMenu(){
            container.DestroyChildren();
            if(ListConfigs.SelectedConfigs == null) return;
            InspectorCreator.Create(container, ListConfigs.SelectedConfigs);
        }
    }
}
