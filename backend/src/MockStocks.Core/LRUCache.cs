namespace MockStocks.Core;

/// <summary>
/// Represents a least recently used cache.
/// </summary>
/// <typeparam name="TKey">
/// The type of the key.
/// </typeparam>
/// <typeparam name="TValue">
/// The type of the value.
/// </typeparam>
public sealed class LRUCache<TKey, TValue>
    where TKey: notnull
{
    private readonly int _capacity;
    private readonly LinkedList<KeyValuePair<TKey, TValue>> _items = new();
    private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _lookup = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LRUCache{TKey,TValue}"/> class.
    /// </summary>
    /// <param name="capacity">
    /// The maximum capacity of the cache.
    /// </param>
    public LRUCache(int capacity)
    {
        _capacity = capacity;
    }

    /// <summary>
    /// Returns the value associated with the specified key.
    /// </summary>
    /// <param name="key">
    /// The key of the value to retrieve.
    /// </param>
    /// <returns>
    /// The value associated with the key.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// The key does not exist in the cache.
    /// </exception>
    public TValue Get(TKey key)
    {
        if (!_lookup.TryGetValue(key, out var node))
        {
            throw new KeyNotFoundException();
        }

        _items.AddFirst(new KeyValuePair<TKey, TValue>(key, node.Value.Value));
        _items.Remove(node);
        _lookup[key] = _items.First!;
        return node.Value.Value;
    }

    /// <summary>
    /// Updates the value of the key if the key exists; otherwise, adds a new key-value pair
    /// to the cache. If the cache exceeds the capacity then the least recently used key is
    /// evicted.
    /// </summary>
    /// <param name="key">
    /// The key of the key-value pair.
    /// </param>
    /// <param name="value">
    /// The value of the key-value pair.
    /// </param>
    public void Put(TKey key, TValue value)
    {
        if (_lookup.TryGetValue(key, out var node))
        {
            _items.Remove(node);
        }
        
        _items.AddFirst(new KeyValuePair<TKey, TValue>(key, value));
        _lookup[key] = _items.First!;
        
        if (_items.Count > _capacity)
        {
            _lookup.Remove(_items.Last!.Value.Key);
            _items.RemoveLast();
        }
    }
}