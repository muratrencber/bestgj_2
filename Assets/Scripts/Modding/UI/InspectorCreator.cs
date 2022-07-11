using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Automata.Modding{
    public static class InspectorCreator
    {
        public static void Create(Transform container, object obj) => Create(new InspectorCreationInfo(container, obj), true);
        public static void Create(InspectorCreationInfo icinf, bool checkCustom = false){
            if(checkCustom && icinf.Value is ICustomField){
                (icinf.Value as ICustomField).Draw(icinf);
            } else {
                foreach(FieldInfo f in icinf.Fields){
                    if(f.IsNotSerialized) continue;
                    CreateField(icinf.GetFieldInfo(f));
                }
            }
        }

        public static void CreateField(InspectorCreationInfo icinf, string fieldName, object fieldContainer) =>
        CreateField(icinf.GetInstanceInfo(fieldContainer, fieldName));

        public static void CreateField(InspectorCreationInfo icinf){
            Type type = icinf.Type;
            object gotValue = icinf.Value;
            if(gotValue is ICustomField){
                (gotValue as ICustomField).Draw(icinf);
            } else if(gotValue is IList && !type.IsArray) {
                CreateIEnumerable(icinf);
            } else {
                if(type == typeof(string)){
                    CreateStringField(icinf);
                } else if(type == typeof(bool)){
                    CreateBoolField(icinf);
                } else if(type == typeof(int) || type == typeof(float)) {
                    CreateNumericField(icinf);
                } else if(type.IsEnum) {
                    CreateEnumField(icinf);
                } else if((type.IsClass || type.IsValueType) && !type.IsGenericType){
                    Transform column = OptionsMenuCreator.CreateFoldUI(icinf.container, new LocalizedString(icinf.Name))[2];
                    //column.GetComponent<RectTransform>().RedrawCSFUpwards();
                    Create(icinf.GetClassInfo(gotValue, column));
                }
            }
        }

        public static void CreateIEnumerable(InspectorCreationInfo icinf){
            bool isArray = icinf.Type.IsArray;
            object collectionValue = icinf.Value;
            IList asList = collectionValue as IList;

            Transform[] columns = OptionsMenuCreator.CreateFoldUI(icinf.container, new LocalizedString(icinf.Name+"[ ]"));
            Transform column = columns[2];
            Transform buttonContainer = columns[1];
            OptionsMenuCreator.Item<Button> addButton = OptionsMenuCreator.Create<Button>(
                buttonContainer, OptionsMenuCreator.ItemType.BUTTON,
                new LocalizedString("+")
            );
            OptionsMenuCreator.Item<Button> removeButton = OptionsMenuCreator.Create<Button>(
                buttonContainer, OptionsMenuCreator.ItemType.BUTTON,
                new LocalizedString("-")
            );
            addButton.itemClass.onClick.AddListener(() => {
                int index = asList.Count;
                object newObject = Activator.CreateInstance(collectionValue.GetType().GetGenericArguments().Single());
                asList.Add(newObject);
                CreateField(icinf.GetListElementInfo(index, newObject, collectionValue, column));
                column.GetComponent<RectTransform>().RedrawCSFUpwards();
            });
            
            removeButton.itemClass.onClick.AddListener(() => {
                if(asList.Count == 0) return;
                asList.RemoveAt(asList.Count - 1);
                GameObject.DestroyImmediate(column.GetChild(column.childCount - 1).gameObject);
                column.GetComponent<RectTransform>().RedrawCSFUpwards();
            });
            PopulateWithItems(asList, icinf, column);
        }

        public static void PopulateWithItems(IList asList, InspectorCreationInfo icinf, Transform column){
            object o = null;
            for(int i = 0; i < asList.Count; ++i){
                o = asList[i];
                CreateField(icinf.GetListElementInfo(i, o, icinf.Value, column));
            }
        }

        public static void CreateStringField(InspectorCreationInfo icinf){
            string value = icinf.Value as string;
            OptionsMenuCreator.Item<TMP_InputField> inp = OptionsMenuCreator.CreateRow<TMP_InputField>(
                icinf.container, OptionsMenuCreator.ItemType.INPUT, new LocalizedString(icinf.Name)
            );
            inp.itemClass.SetTextWithoutNotify(value);

            inp.itemClass.onDeselect.AddListener((string newValue) => {
                icinf.SetValue(newValue);
            });
        }

        public static void CreateBoolField(InspectorCreationInfo icinf){
            bool value = (bool)icinf.Value;
            OptionsMenuCreator.Item<Toggle> toggle = OptionsMenuCreator.CreateRow<Toggle>(
                icinf.container, OptionsMenuCreator.ItemType.TOGGLE,
                new LocalizedString(icinf.Name)
            );
            toggle.itemClass.isOn = value;

            toggle.itemClass.onValueChanged.AddListener((b)=>{
                icinf.SetValue(b);
            });
        }

        public static void CreateEnumField(InspectorCreationInfo icinf){
            object val = icinf.Value;
            Type fType = icinf.Type;
            string[] names = Enum.GetNames(fType);
            OptionsMenuCreator.Item<TMP_Dropdown> enumField = OptionsMenuCreator.CreateRow<TMP_Dropdown>(
                icinf.container, OptionsMenuCreator.ItemType.DROPDOWN,
                new LocalizedString(icinf.Name), names.Select((n) => new LocalizedString(n)).ToArray()
            );
            string selected = Enum.GetName(fType, val);
            int selectedIndex = names.ToList().IndexOf(selected);
            if(selectedIndex < 0 || selectedIndex >= names.Length) selectedIndex = 0;
            enumField.itemClass.value = selectedIndex;

            enumField.itemClass.onValueChanged.AddListener((index)=>{
                object newSelected = Enum.Parse(fType, names[index]);
                icinf.SetValue(newSelected);
            });
        }

        public static void CreateNumericField(InspectorCreationInfo icinf){
            float value = 0;
            Type fType = icinf.Type;
            object valueAsObject = icinf.Value;
            if(fType == typeof(int)) value = (int)valueAsObject;
            else if(fType == typeof(float)) value = (float)valueAsObject;

            OptionsMenuCreator.Item<TMP_InputField> inp = OptionsMenuCreator.CreateRow<TMP_InputField>(
                icinf.container, OptionsMenuCreator.ItemType.INPUT, new LocalizedString(icinf.Name)
            );

            if(fType == typeof(int)) inp.itemClass.contentType = TMP_InputField.ContentType.IntegerNumber;
            else inp.itemClass.contentType = TMP_InputField.ContentType.DecimalNumber;

            inp.itemClass.SetTextWithoutNotify(value.ToString());

            inp.itemClass.onDeselect.AddListener((string newValue) => {
                object newValueObject = valueAsObject;
                if(fType == typeof(int)) newValueObject = int.Parse(newValue);
                else if(fType == typeof(float)) newValueObject = float.Parse(newValue);

                icinf.SetValue(newValueObject);
            });
        }
    }
}
