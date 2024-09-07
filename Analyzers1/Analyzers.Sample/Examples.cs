// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Analyzers.Sample;

public class Examples
{
    public void BasicExample()
    {
        var name = new GetMobileRequestNameNumber();
    }
}

public interface IRequest;

public sealed class GetMobileRequestNameNumber : IRequest;