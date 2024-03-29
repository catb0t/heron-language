﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeronEngine
{
    #region user defined type value
    /// <summary>
    /// An instance of a Heron class. A HeronObject is more general in that it includes 
    /// primitive objects and .NET objects which are not part of the ClassDefn 
    /// hierarchy.
    /// </summary>
    public class ClassInstance : HeronValue
    {
        ClassDefn cls;
        ModuleInstance module;
        Scope fields = new Scope();

        public ClassInstance(ClassDefn c, ModuleInstance m)
        {
            cls = c;
            module = m;
        }

        public override bool Equals(object obj)
        {
            return obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Creates a scope in the environment, containing variables that map to the class field names. 
        /// It is the caller's reponsibility to remove the scope. 
        /// </summary>
        /// <param name="env"></param>
        public void PushFieldsAsScope(VM vm)
        {
            vm.PushScope(fields);
        }

        /// <summary>
        /// Mostly for internal purposes. 
        /// </summary>
        /// <param name="name"></param>
        public void AssureFieldDoesntExist(string name)
        {
            if (HasField(name))
                throw new Exception("field " + name + " already exists");
        }

        /// <summary>
        /// Mostly for internal purposes
        /// </summary>
        /// <param name="name"></param>
        public void AssureFieldExists(string name)
        {
            if (!HasField(name))
                throw new Exception("field " + name + " does not exist");
        }

        /// <summary>
        /// Sets the value on the named field. Does not automatically add a field if missing.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public override void SetField(string name, HeronValue val)
        {
            if (fields.HasName(name))
                fields[name] = val;
            else if (GetBase() != null)
                GetBase().SetField(name, val);
            else
                throw new Exception("Field '" + name + "' does not exist");
        }

        /// <summary>
        /// Returns true if field has already been added 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasField(string name)
        {
            if (fields.HasName(name))
                return true;
            if (GetBase() != null)
                return GetBase().HasField(name);
            return false;
        }

        /// <summary>
        /// Returns the field associated with the name. Throws 
        /// an exception if it does not exist.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HeronValue GetField(string name)
        {
            if (fields.HasName(name))
                return fields[name];
            else if (GetBase() != null)
                return GetBase().GetField(name);
            else
                throw new Exception("Field '" + name + "' does not exist");
        }

        /// <summary>
        /// Returns true if any methods are available that have the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasMethod(string name)
        {
            return cls.HasMethod(name);
        }

        /// <summary>
        /// Returns all functions sharing the given name at once
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FunDefnListValue GetMethods(string name)
        {
            return new FunDefnListValue(this, name, cls.GetMethods(name));
        }

        /// <summary>
        /// Adds a field. FieldDefn must not already exist. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void AddField(VarDesc v, HeronValue val)
        {
            AssureFieldDoesntExist(v.name);
            fields.Add(v, val);
        }

        /// <summary>
        /// Adds a field. FieldDefn must not already exist. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void AddField(VarDesc v)
        {
            AssureFieldDoesntExist(v.name);
            fields.Add(v);
        }

        /// <summary>
        /// Gets a field or method associated with the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override HeronValue GetFieldOrMethod(string name)
        {
            if (fields.HasName(name))
                return fields[name];
            if (HasMethod(name))
                return GetMethods(name);
            if (GetBase() != null)
                return GetBase().GetFieldOrMethod(name);
            return null;
        }

        public override string ToString()
        {
            return "{" + cls.name + "}";
        }

        public override HeronType Type
        {
            get { return cls; }
        }

        /// <summary>
        /// Returns the base class that that this class instance derives from,
        /// or NULL if not applicable.
        /// </summary>
        /// <returns></returns>
        public ClassInstance GetBase()
        {
            if (!fields.HasName("base"))
                return null;
            HeronValue r = fields["base"];
            if (!(r is ClassInstance))
                throw new Exception("The 'base' field should always be an instance of a class");
            return r as ClassInstance;
        }

        /// <summary>
        /// Used to cast the class instance to its base class or an interface.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override HeronValue As(HeronType t)
        {
            if (t is ClassDefn)
            {
                ClassDefn c1 = cls;
                ClassDefn c2 = t as ClassDefn;

                if (c2.name == c1.name)
                    return this;

                if (GetBase() == null)
                    return null;

                return GetBase().As(t);
            }
            else if (t is InterfaceDefn)
            {
                if (cls.Implements(t as InterfaceDefn))
                    return new InterfaceInstance(this, t as InterfaceDefn);

                return null;
            }
            else if (t == PrimitiveTypes.AnyType)
            {
                return new AnyValue(this);
            }
            else if (t == PrimitiveTypes.UnknownType)
            {
                return this;
            }
            return null;
        }

        [HeronVisible]
        public string GetClassName()
        {
            return cls == null ? "_null_" : cls.GetName();
        }

        public ModuleInstance GetModuleInstance()
        {
            return module;
        }
    }

    /// <summary>
    /// An instance of a Heron class. A HeronObject is more general in that it includes 
    /// primitive objects and .NET objects which are not part of the ClassDefn 
    /// hierarchy.
    /// </summary>
    public class InterfaceInstance : HeronValue
    {
        public ClassInstance obj;
        public InterfaceDefn hinterface;

        public InterfaceInstance(ClassInstance obj, InterfaceDefn i)
        {
            this.obj = obj;
            hinterface = i;
        }

        public override bool Equals(object obj)
        {
            return obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns all functions sharing the given name at once
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FunDefnListValue GetMethods(string name)
        {
            return new FunDefnListValue(obj, name, hinterface.GetMethods(name));
        }

        public override HeronValue GetFieldOrMethod(string name)
        {
            if (!hinterface.HasMethod(name))
                base.GetFieldOrMethod(name);
            return obj.GetFieldOrMethod(name);
        }

        public override string ToString()
        {
            return "{" + hinterface.name + "}";
        }

        public override HeronType Type
        {
            get { return hinterface; }
        }

        public ClassInstance GetObject()
        {
            return obj;
        }

        public override HeronValue As(HeronType t)
        {
            InterfaceDefn id = t as InterfaceDefn;
            if (id == null)
                return null;
            if (hinterface.Implements(id))
                return new InterfaceInstance(obj, id);
            return null;
        }

        public ModuleInstance GetModuleInstance()
        {
            return obj.GetModuleInstance();
        }
    }

    /// <summary>
    /// An instance of an enumerable value.
    /// </summary>
    public class EnumInstance : HeronValue
    {
        EnumDefn henum;
        string name;

        public EnumInstance(EnumDefn e, string s)
        {
            henum = e;
            name = s;
        }

        public override HeronType Type
        {
            get { return henum; }
        }

        public override bool Equals(object obj)
        {
            EnumInstance that = obj as EnumInstance;
            if (that == null)
                return false;
            return that.henum.Equals(henum) && that.name == name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode() + henum.GetHashCode();
        }
    }

    /// <summary>
    /// A moduleDef instance can contain exposedFields and methods just like a class
    /// </summary>
    public class ModuleInstance : ClassInstance
    {
        public Dictionary<string, ModuleInstance> imports = new Dictionary<string, ModuleInstance>();

        public ModuleInstance(ModuleDefn m)
            : base(m, null)
        {
            if (m == null)
                throw new Exception("Missing module definition");
        }

        public ModuleDefn GetModuleDefn()
        {
            ModuleDefn m = Type as ModuleDefn;
            if (m == null)
                throw new Exception("Missing module");
            return m;
        }

        public HeronValue GetExportedFieldOrMethod(string name)
        {
            return base.GetFieldOrMethod(name);
        }

        /// <summary>
        /// Returns local fields or methods declared, inherited, or 
        /// even those imported. Consumers of a module should only
        /// ever call "GetExportedFieldOrMethod"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override HeronValue GetFieldOrMethod(string name)
        {
            if (imports.ContainsKey(name))
                return imports[name];
            HeronValue r = base.GetFieldOrMethod(name);
            if (r != null)
                return r;

            List<HeronValue> candidates = new List<HeronValue>();
            foreach (ModuleInstance m in imports.Values)
            {
                HeronValue tmp = m.GetExportedFieldOrMethod(name);
                if (tmp != null)
                    candidates.Add(tmp);
            }
            if (candidates.Count > 1)
                throw new Exception("Found mulitple module instances containing definition of " + name);
            if (candidates.Count == 1)
                return candidates[0];
            return null;
        }
    }
    #endregion
}
