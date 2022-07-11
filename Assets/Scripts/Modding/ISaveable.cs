using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Automata.Modding{
    public interface ISaveable{
        void Save(InspectorCreationInfo icinf);
    }
    public interface ISaveableString {    
        void Save(string s, FieldInfo fieldInfo, object original);
    }
    public interface ISaveableNumeric {    
        void Save(float f, FieldInfo fieldInfo, object original);
    }
    public interface ISaveableBool {    
        void Save(bool b, FieldInfo fieldInfo, object original);
    }
    public interface ISaveableEnum {
        void Save(object value, FieldInfo fieldInfo, object original);
    }
}
