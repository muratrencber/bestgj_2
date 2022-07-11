using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

namespace Automata.Modding{
    public class InspectorCreationInfo{
        public Transform container;
        public FieldInfo field;
        public bool isFromCollection;
        public int index = 0;
        public object fieldObject;
        public string fieldName;
        public object ownerObject;
        public object collectionObject;
        public object root;
        public Action<InspectorCreationInfo> onSave;

        public InspectorCreationInfo Clone() => new InspectorCreationInfo(this);
        public InspectorCreationInfo ChangeContainer(Transform newContainer){
            var inf = Clone();
            inf.container = newContainer;
            return inf;
        }

        public InspectorCreationInfo GetInstanceInfo(object owner, string fieldName, Transform newContainer = null){
            InspectorCreationInfo icinf = new InspectorCreationInfo(this);
            icinf.isFromCollection = false;
            icinf.collectionObject = null;
            icinf.ownerObject = owner;
            icinf.field = owner.GetType().GetField(fieldName);
            icinf.fieldName = fieldName;
            if(newContainer != null) icinf.container = newContainer;
            return icinf;
        }

        public InspectorCreationInfo GetListElementInfo(int index, object value, object collectionObject, Transform newContainer = null){
            InspectorCreationInfo icinf = new InspectorCreationInfo(this);
            icinf.index = index;
            icinf.field = null;
            icinf.ownerObject = value;
            icinf.collectionObject = collectionObject;
            icinf.fieldObject = null;
            icinf.isFromCollection = true;
            if(newContainer != null) icinf.container = newContainer;
            return icinf;
        }

        public InspectorCreationInfo GetClassInfo(object newClass, Transform newContainer = null){
            InspectorCreationInfo icinf = new InspectorCreationInfo(this);
            icinf.isFromCollection = false;
            icinf.fieldObject = null;
            icinf.collectionObject = null;
            icinf.ownerObject = newClass;
            if(newContainer != null) icinf.container = newContainer;
            return icinf;
        }

        public InspectorCreationInfo GetFieldInfo(FieldInfo f, Transform newContainer = null){
            InspectorCreationInfo icinf = new InspectorCreationInfo(this);
            icinf.field = f;
            icinf.isFromCollection = false;
            icinf.fieldObject = null;
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
            if(fieldObject != null) return fieldObject;
            return ownerObject;
        }}

        public Type Type{
            get{
                if(field != null) return field.FieldType;
                if(fieldObject != null) return fieldObject.GetType();
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
            if(!HasFieldInfo){
                if(fieldObject != null) return fieldName;;
                return "Element "+index.ToString();
            } return field.Name;
        }}

        public void SetValue(object o){
            if(field != null) field.SetValue(ownerObject, o);
            else if(isFromCollection){
                IList arrOrList = collectionObject as IList;
                arrOrList[index] = o;
            } else if(fieldObject != null){
                fieldObject = o;
            }
            if(root != null){
                ISaveable rootAsSaveable = root as ISaveable;
                if(rootAsSaveable != null)
                    rootAsSaveable.Save(this);
            }
        }
    }
}

