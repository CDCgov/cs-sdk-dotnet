namespace CS.Sdk.Generators
{
    public interface IGenerator
    {
        string Generate(string profileIdentifier, string conditionCode);
    }
}
