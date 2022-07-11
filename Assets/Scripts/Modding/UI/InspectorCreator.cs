using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Automata.Modding{
    public class InspectorCreationInfo{
        public Transform container;
        public FieldInfo field;
        public bool isFromCollection;
        public int index = 0;
        public object collectionObject;
        public object ownerObject;
        public object root;
        public Action<InspectorCreationInfo> onSave;

        public InspectorCreationInfo GetListElementInfo(int index, object value, object collectionObject, Transform newContainer = null){
            InspectorCreationInfo icinf = new InspectorCreationInfo(this);
            icinf.index = index;
            icinf.field = null;
            icinf.collectionObject = collectionObject;
            icinf.ownerObject = value;
            icinf.isFromCollection = true;
            if(newContainer != null) icinf.container = newContainer;
            return icinf;
        }

        public InspectorCreationInfo GetClassInfo(object newClass, Transform newContainer = null){
            InspectorCreationInfo icinf = new InspectorCreationInfo(this);
            icinf.isFromCollection = false;
            icinf.collectionObject = null;
            icinf.ownerObject = newClass;
            if(newContainer != null) icinf.container = newContainer;
            return icinf;
        }

        public InspectorCreationInfo GetFieldInfo(FieldInfo f, Transform newContainer = null){
            InspectorCreationInfo icinf = new InspectorCreationInfo(this);
            icinf.field = f;
            icinf.isFromCollection = false;
            icinf.collectionObject = null;
            if(newContainer != null) icinf.container = newContainer;
            return icinf;
        }

        public InspectorCreationInfo(Transform container, object obj) => Set(container, null, false, obj, null, null);
        public InspectorCreationInfo(InspectorCreationInfo icinf) => Set(icinf.container, icinf.field,
        icinf.isFromCollection, icinf.ownerObject, icinf.root, icinf.onSave);
        public void Set(Transform c, FieldInfo f, bool ifc, object oo, object r, Action<InspectorCreationInfo> os){
            container = c;
            field = f;
            isFromCollection = ifc;
            ownerObject = oo;
            root = r;
            onSave = os;
        }

        public object Value {get{
            if(field != null) return field.GetValue(ownerObject);
            return ownerObject;
        }}

        public Type Type{
            get{
                if(field != null) return field.FieldType;
                return ownerObject.GetType();
            }
        }

        public bool HasFieldInfo{
            get{ return field != null; }}

        public FieldInfo[] Fields {
            get{ return Type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.FlattenHierarchy);}
        }

        public string Name { get {
            if(!HasFieldInfo) return "Element "+index.ToString();
            return field.Name;
        }}

        public void SetValue(object o){
            if(field != null) field.SetValue(ownerObject, o);
            else if(isFromCollection){
                IList arrOrList = collectionObject as IList;
                arrOrList[index] = o;
            }
            if(root != null){
                ISaveable rootAsSaveable = root as ISaveable;
                if(rootAsSaveable != null)
                    rootAsSaveable.Save(this);
            }
        }
    }
    public static class InspectorCreator
    {
        public static void Create(Transform container, object obj) => Create(new InspectorCreationInfo(container, obj));
        public static void Create(InspectorCreationInfo icinf){
            foreach(FieldInfo f in icinf.Fields){
                if(f.IsNotSerialized) continue;
                CreateField(icinf.GetFieldInfo(f));
            }
        }

        public static void CreateField(InspectorCreationInfo icinf){
            Type type = icinf.Type;
            object gotValue = icinf.Value;
            if(gotValue is IList && !type.IsArray){
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
