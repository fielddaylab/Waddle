using BeauUtil;
using UnityEngine;

namespace Waddle {
    public struct CollisionCooldowns {
        public struct Entry {
            public RuntimeObjectHandle Id;
            public float Cooldown;
        }

        public RingBuffer<Entry> Entries;

        public CollisionCooldowns(int capacity) {
            Entries = new RingBuffer<Entry>(capacity, RingBufferMode.Expand);
        }

        public void Update(float deltaTime) {
            for(int i = Entries.Count - 1; i >= 0; i--) {
                ref Entry e = ref Entries[i];
                e.Cooldown -= deltaTime;
                if (e.Cooldown <= 0) {
                    Entries.FastRemoveAt(i);
                }
            }
        }

        public bool Contains(RuntimeObjectHandle obj) {
            return Entries.FindIndex(FindById, obj) >= 0;
        }

        public void Add(RuntimeObjectHandle obj, float duration) {
            int idx = Entries.FindIndex(FindById, obj);
            if (idx >= 0) {
                Entries[idx].Cooldown = duration;
            } else {
                Entries.PushBack(new Entry() {
                    Id = obj,
                    Cooldown = duration
                });
            }
        }

        static private Predicate<Entry, RuntimeObjectHandle> FindById = (e, i) => {
            return e.Id == i;
        };
    }
}