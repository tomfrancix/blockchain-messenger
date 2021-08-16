namespace sakurai.Interface.IFactory
{
    public interface IHashFactory
    {
        string GenerateHash(string timestamp, string lastHash, string data);
    }
}
