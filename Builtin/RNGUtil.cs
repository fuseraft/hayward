namespace citrus.Builtin;

public static class RNGUtil
{
    private static readonly char[] s_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    private static readonly ThreadLocal<Random> s_random = new(() => new Random(Guid.NewGuid().GetHashCode()));

    /// <summary>
    /// Generates a random string of the given length from the allowed character set.
    /// </summary>
    /// <param name="length">Length of the random string to generate.</param>
    /// <returns>Randomly generated string.</returns>
    public static string Generate(int length)
    {
        var buffer = new char[length];
        var rnd = s_random.Value ?? new Random(Guid.NewGuid().GetHashCode());

        for (int i = 0; i < length; i++)
        {
            int index = rnd.Next(s_chars.Length);
            buffer[i] = s_chars[index];
        }

        return new string(buffer);
    }
}
