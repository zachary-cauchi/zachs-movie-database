using Orleans;

namespace ZMDB.GrainInterfaces
{
    public interface IMovie : IGrainWithIntegerKey
    {
        ValueTask<string> SayHello(string greeting);
    }
}