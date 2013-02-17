using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryRanking
{
    public class LRUCache<K, V>
    {
        private LinkedList<LRUCacheItem<K, V>> order;
        private Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> cache;
        private int capacity;
        private Object sync = new Object();

        public LRUCache(int c)
        {
            order = new LinkedList<LRUCacheItem<K, V>>();
            cache = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();
            capacity = c;
        }

        public bool Get(K k, out V v)
        {
            lock (sync)
            {
                LinkedListNode<LRUCacheItem<K, V>> node;
                if (cache.TryGetValue(k, out node))
                {
                    V value = node.Value.value;
                    order.Remove(node);
                    order.AddLast(node);
                    v = value;
                    return true;
                }

                v = default(V);
                return false;
            }
        }

        public bool Contains(K k)
        {
            lock (sync)
            {
                return cache.ContainsKey(k);
            }
        }

        public void Add(K k, V v)
        {
            lock (sync)
            {
                LinkedListNode<LRUCacheItem<K, V>> node;
                if (cache.ContainsKey(k) && cache.TryGetValue(k, out node))
                {
                    order.Remove(node);
                    order.AddLast(node);
                    return;
                }

                if (cache.Count >= capacity)
                {
                    cache.Remove(order.First.Value.key);
                    order.RemoveFirst();
                }

                LRUCacheItem<K, V> pair = new LRUCacheItem<K, V>(k, v);
                LinkedListNode<LRUCacheItem<K, V>> newNode = new LinkedListNode<LRUCacheItem<K, V>>(pair);
                order.AddLast(newNode);
                cache.Add(k, newNode);
            }
        }
    }

    internal class LRUCacheItem<K, V>
    {
        public K key;
        public V value;

        public LRUCacheItem(K k, V v)
        {
            key = k;
            value = v;
        }
    }

}
