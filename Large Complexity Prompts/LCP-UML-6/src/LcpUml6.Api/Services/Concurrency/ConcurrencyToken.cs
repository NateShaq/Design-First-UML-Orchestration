using System.Text;

namespace LcpUml6.Api.Services.Concurrency;

public static class ConcurrencyToken
{
    public static string Encode(byte[] rowVersion) => Convert.ToBase64String(rowVersion);

    public static byte[] Decode(string token) => Convert.FromBase64String(token);
}
