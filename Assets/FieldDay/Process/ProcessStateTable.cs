using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BeauUtil;
using BeauUtil.Debugger;

namespace FieldDay.Processes {
    /// <summary>
    /// Map of process states.
    /// </summary>
    public sealed class ProcessStateTable {
        private readonly Dictionary<StringHash32, ProcessStateDefinition> m_StateMap;
        
        /// <summary>
        /// Parent state table.
        /// If a key is not present in this table, it'll be looked up in the parent table.
        /// </summary>
        public readonly ProcessStateTable Parent;

        /// <summary>
        /// Default state id.
        /// </summary>
        public StringHash32 DefaultId;

        private ProcessStateTable(int inCapacity, ProcessStateTable parent) {
            m_StateMap = new Dictionary<StringHash32, ProcessStateDefinition>(inCapacity, CompareUtils.DefaultEquals<StringHash32>());
            Parent = parent;
        }

        private ProcessStateTable(Dictionary<StringHash32, ProcessStateDefinition> copyFrom, StringHash32 defaultId) {
            m_StateMap = new Dictionary<StringHash32, ProcessStateDefinition>(copyFrom, CompareUtils.DefaultEquals<StringHash32>());
            DefaultId = defaultId;
        }

        #region Accessors

        /// <summary>
        /// Returns the process state definition for the given id.
        /// </summary>
        public ProcessStateDefinition this[StringHash32 id] {
            get { return Get(id); }
        }

        /// <summary>
        /// Returns if a process state with the given id exists in this table.
        /// </summary>
        public bool Has(StringHash32 id) {
            if (m_StateMap.ContainsKey(id)) {
                return true;
            }

            if (Parent != null) {
                return Parent.Has(id);
            }

            return false;
        }

        /// <summary>
        /// Returns the process state definition for the given id.
        /// </summary>
        public ProcessStateDefinition Get(StringHash32 id) {
            ProcessStateDefinition def;
            if (!m_StateMap.TryGetValue(id, out def)) {
                def = Parent?.Get(id);
            }
            if (def == null) {
                Log.Error("[ProcessStateTable] No state with id '{0}' in table", id.ToDebugString());
            }
            return def;
        }

        #endregion // Accessors

        #region Modifiers

        /// <summary>
        /// Sets the state definition for the given id.
        /// </summary>
        public void Add(ProcessStateDefinition def) {
            Assert.NotNull(def);
            m_StateMap[def.Id] = def;
        }

        /// <summary>
        /// Sets the state definition for the given id.
        /// </summary>
        public void AddFrom(StringHash32 id, IProcessStateCallbacks stateDef, BitSet64 customFlags = default) {
            Assert.NotNull(stateDef);
            Add(ProcessStateDefinition.FromCallbacks(id, stateDef, customFlags));
        }

        /// <summary>
        /// Deletes the state definition for the given id.
        /// </summary>
        public void Delete(StringHash32 id) {
            m_StateMap.Remove(id);
        }

        #endregion // Modifiers

        #region Factory

        /// <summary>
        /// Creates a new process state table with the given capacity.
        /// </summary>
        static public ProcessStateTable Create(int capacity = 8) {
            return new ProcessStateTable(capacity, null);
        }

        /// <summary>
        /// Creates a new process state table that can override the given parent table.
        /// </summary>
        static public ProcessStateTable Override(ProcessStateTable parent, int capacity = 8) {
            return new ProcessStateTable(capacity, parent);
        }

        /// <summary>
        /// Clones the given process state table.
        /// </summary>
        static public ProcessStateTable Clone(ProcessStateTable source) {
            return new ProcessStateTable(source.m_StateMap, source.DefaultId);
        }

        #endregion // Factor
    }
}