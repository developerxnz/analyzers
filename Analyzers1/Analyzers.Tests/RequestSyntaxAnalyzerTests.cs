using System.Threading.Tasks;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
        Analyzers.RequestSyntaxAnalyzer>;

namespace Analyzers1.Tests;

public class RequestSyntaxAnalyzerTests
{
    [Fact]
    public async Task RequestSuffix_AlertDiagnostic()
    {
        const string text = @"
public class Program
{
    public void Main()
    {
        var myRequest = new GetPhoneNumber();
    }
}

public interface IRequest {};

public class GetPhoneNumber : IRequest {};
";
        var expected = Verifier.Diagnostic()
            .WithSpan(12, 14, 12, 28)
            .WithArguments("GetPhoneNumber");
        
        await Verifier.VerifyAnalyzerAsync(text, expected);
    }
}