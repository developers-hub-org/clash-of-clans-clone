using System.Collections.Generic;

namespace AStarPathfinding {

    public static class DictionaryExtensions {
        public static void AddUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
            else
                dictionary[key] = value;
        }

        public static void TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
            dictionary.Remove(key);
        }
    }

}