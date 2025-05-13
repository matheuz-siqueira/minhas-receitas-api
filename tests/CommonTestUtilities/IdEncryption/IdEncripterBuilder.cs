using Sqids;

namespace CommonTestUtilities.IdEncryption;

public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "z5vx9mj8t2ydbi1l7nsfaehw4cog3ukrpq06"
        });
    }
}
