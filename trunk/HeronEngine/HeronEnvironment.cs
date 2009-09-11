﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security.Policy;

namespace HeronEngine
{
    /// <summary>
    /// This is the lexical environment of a running program. 
    /// It is organized as a stack of frames. It has a temporary "result" variable
    /// and a list of arguments.
    /// </summary>
    public class Environment 
    {
        #region fields
        /// <summary>
        /// The current result value
        /// </summary>
        private HeronObject result;

        /// <summary>
        /// Used for loading assemblies
        /// </summary>
        private AppDomain domain;

        /// <summary>
        /// A flag that is set to true when a return statement occurs. 
        /// </summary>
        bool bReturning = false;

        /// <summary>
        /// A list of call stack frames 
        /// </summary>
        Stack<Frame> frames = new Stack<Frame>();

        /// <summary>
        /// Top level object representing an executable Heron program.
        /// Contains all of the modules.
        /// </summary>
        HeronProgram program;
        #endregion

        public Environment(HeronProgram p)
        {
            program = p;

            AppDomainSetup domaininfo = new AppDomainSetup();

            //Create evidence for the new appdomain from evidence of the current application domain
            Evidence evidence = AppDomain.CurrentDomain.Evidence;

            // Create domain
            domain = AppDomain.CreateDomain("MyDomain", evidence, domaininfo);
            
            Clear();
        }

        public void Clear()
        {
            frames.Clear();
            result = null;
            bReturning = false;
            Initialize();
        }

        public void Initialize()
        {
            PushNewFrame(null, null);
            PushScope();
        }


        /// <summary>
        /// Throw an exception if condition is not true. However, not an assertion. 
        /// This is used to check for exceptional run-time condition.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="s"></param>
        private void Assure(bool b, string s)
        {
            if (!b)
                throw new Exception("error occured: " + s);
        }

        /// <summary>
        /// Called when a new function execution starts.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="self"></param>
        public void PushNewFrame(HeronFunction f, ClassInstance self)
        {
            Assure(!bReturning, "cannot push a new frame while returning from another");
            frames.Push(new Frame(f, self));
        }

        /// <summary>
        /// Creates a new lexical scope. Roughly corresponds to an open brace ('{') in many languages.
        /// </summary>
        public void PushScope()
        {
            PushScope(new ObjectTable());
        }

        /// <summary>
        /// Creates a new scope, with a predefined set of variable names. Useful for function arguments
        /// or class fields.
        /// </summary>
        /// <param name="scope"></param>
        public void PushScope(ObjectTable scope)
        {
            frames.Peek().AddScope(scope);
        }

        /// <summary>
        /// Creates a new variable name in the current scope.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        public void AddVar(string s, HeronObject o)
        {
            if (frames.Peek().HasVar(s))
                throw new Exception(s + " is already declared in the scope");
            frames.Peek().AddVar(s, o);
        }

        /// <summary>
        /// Assigns a value a variable name in the current environment.
        /// The name must already exist
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        public void SetVar(string s, HeronObject o)
        {
            foreach (Frame f in frames)
                if (f.SetVar(s, o))
                    return;
            throw new Exception("Could not find variable " + s);
        }

        public void SetField(string s, HeronObject o)
        {
            if (frames.Count == 0)
                throw new Exception("No stack frames");
            Frame f = frames.Peek();
            if (f.self == null)
                throw new Exception("Not called from within a class");
            f.self.SetField(s, o);
        }

        /// <summary>
        /// Inidcates the current activation record is finished.
        /// </summary>
        public void PopFrame()
        {
            // Reset the returning flag, to indicate that the returning operation is completed. 
            bReturning = false;
            frames.Pop();
        }

        /// <summary>
        /// Removes the current scope. Correspond roughly to a closing brace ('}').
        /// </summary>
        public void PopScope()
        {
            frames.Peek().PopScope();
        }

        /// <summary>
        /// Returns true if a return statement was executed and we are leaving the current scope.
        /// </summary>
        /// <returns></returns>
        public bool IsReturning()
        {
            return bReturning;
        }

        /// <summary>
        /// This is used by loops over statements to check whether a return statement, a break 
        /// statement, or a throw statement was called. Currently only return statements are supported.
        /// </summary>
        /// <returns></returns>
        public bool ShouldExitScope()
        {
            return IsReturning();
        }

        /// <summary>
        /// Called by a return statement. Sets the function result, and sets a flag to indicate 
        /// to executing statement groups that execution should terminate.
        /// </summary>
        /// <param name="ret"></param>
        public void Return(HeronObject ret)
        {
            Assure(!bReturning, "internal error, returning flag was not reset");
            bReturning = true;
            result = ret;
        }

        /// <summary>
        /// Looks up the value or type associated with the name.
        /// Looks in each scope in the currenst stack frame until a match is found.
        /// If no match is found then the various module scopes are searched.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public HeronObject LookupName(string s)
        {
            if (frames.Count != 0)
            {
                bool bFound = false;
                HeronObject r = frames.Peek().LookupName(s, out bFound);
                if (bFound)
                    return r;
            }
            throw new Exception("Could not find '" + s + "' in the environment");
       }

        /// <summary>
        /// Looks up a name in the local variables in current scope only.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HeronObject LookupVar(string name)
        {
            if (frames.Count == 0)
                return null;
            return frames.Peek().LookupVar(name);
        }

        /// <summary>
        /// Returns true if the name is that of a variable in the local scope
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasVar(string name)
        {
            if (frames.Count == 0)
                return false;
            return frames.Peek().HasVar(name);
        }

        /// <summary>
        /// Looks up a name as a field in the current object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HeronObject LookupField(string name)
        {
            if (frames.Count == 0)
                return null;
            return frames.Peek().LookupField(name);
        }

        /// <summary>
        /// Returns true if the name is a field in the current object scope.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasField(string name)
        {
            if (frames.Count == 0)
                return false;
            return frames.Peek().HasField(name);
        }

        /// <summary>
        /// Gets the value set by the previous "return".
        /// Resets the result as well. So any subsequent calls will
        /// return null, until return is called on it.
        /// </summary>
        /// <returns></returns>
        public HeronObject GetLastResult()
        {
            HeronObject r = result;
            result = null;
            return r;
        }

        /// <summary>
        /// Gets the current activation record.
        /// </summary>
        /// <returns></returns>
        public Frame GetCurrentFrame()
        {
            return frames.Peek();
        }

        /// <summary>
        /// Returns a textual representation of the environment. 
        /// Used primarily for debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Frame f in frames)
                sb.Append(f.ToString());
            return sb.ToString();
        }
    }
}