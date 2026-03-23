using System.Collections.Concurrent;

namespace LcpUml6.Api.Services.Transactions;

public record TransactionEntry(Guid EntityId, byte[] Token, DateTime CreatedUtc);

public class TransactionRegistry
{
    private readonly ConcurrentDictionary<string, TransactionEntry> _entries = new();
    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(10);

    public string Register(Guid entityId, byte[] token)
    {
        var txId = Guid.NewGuid().ToString("N");
        _entries[txId] = new TransactionEntry(entityId, token, DateTime.UtcNow);
        return txId;
    }

    public bool TryGet(string txId, out TransactionEntry? entry)
    {
        entry = null;
        if (!_entries.TryGetValue(txId, out var value)) return false;
        if (DateTime.UtcNow - value.CreatedUtc > _ttl)
        {
            _entries.TryRemove(txId, out _);
            return false;
        }
        entry = value;
        return true;
    }

    public bool TryRemove(string txId, out TransactionEntry? entry)
    {
        entry = null;
        if (TryGet(txId, out var current))
        {
            _entries.TryRemove(txId, out _);
            entry = current;
            return true;
        }
        return false;
    }
}
