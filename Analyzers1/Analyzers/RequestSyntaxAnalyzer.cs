﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers;

/// <summary>
/// A sample analyzer that reports the company name being used in class declarations.
/// Traverses through the Syntax Tree and checks the name (identifier) of each class node.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RequestSyntaxAnalyzer : DiagnosticAnalyzer
{
    // Preferred format of DiagnosticId is Your Prefix + Number, e.g. CA1234.
    public const string DiagnosticId = "FL0001";

    // Feel free to use raw strings if you don't need localization.
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.FL0001Title),
        Resources.ResourceManager, typeof(Resources));

    // The message that will be displayed to the user.
    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.FL0001MessageFormat), Resources.ResourceManager,
            typeof(Resources));

    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.FL0001Description), Resources.ResourceManager,
            typeof(Resources));

    // The category of the diagnostic (Design, Naming etc.).
    private const string Category = "Naming";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    // Keep in mind: you have to list your rules here.
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        // You must call this method to avoid analyzing generated code.
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        // You must call this method to enable the Concurrent Execution.
        context.EnableConcurrentExecution();

        // Subscribe to the Syntax Node with the appropriate 'SyntaxKind' (ClassDeclaration) action.
        // To figure out which Syntax Nodes you should choose, consider installing the Roslyn syntax tree viewer plugin Rossynt: https://plugins.jetbrains.com/plugin/16902-rossynt/
        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ClassDeclaration);

        // Check other 'context.Register...' methods that might be helpful for your purposes.
    }

    /// <summary>
    /// Executed for each Syntax Node with 'SyntaxKind' is 'ClassDeclaration'.
    /// </summary>
    /// <param name="context">Operation context.</param>
    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        // The Roslyn architecture is based on inheritance.
        // To get the required metadata, we should match the 'Node' object to the particular type: 'ClassDeclarationSyntax'.
        if (context.Node is not ClassDeclarationSyntax classDeclarationNode)
            return;

        // 'Identifier' means the token of the node. In this case, the identifier of the 'ClassDeclarationNode' is the class name.
        var classDeclarationIdentifier = classDeclarationNode.Identifier;

        if (!HasInterface(classDeclarationNode, "IRequest"))
        {
            return;
        }
        
        if(classDeclarationNode.Identifier.Text.EndsWith("Request"))
        {
            return;
        }

        var diagnostic = Diagnostic.Create(Rule,
            // The highlighted area in the analyzed source code. Keep it as specific as possible.
            classDeclarationIdentifier.GetLocation(),
            // The value is passed to 'MessageFormat' argument of your 'Rule'.
            classDeclarationIdentifier.Text);

        // Reporting a diagnostic is the primary outcome of the analyzer.
        context.ReportDiagnostic(diagnostic);
        
    }
    
    /// <summary>Indicates whether or not the class has a specific interface.</summary>
    /// <returns>Whether or not the SyntaxList contains the attribute.</returns>
    private bool HasInterface(ClassDeclarationSyntax source, string interfaceName)
    {
        IEnumerable<BaseTypeSyntax> baseTypes = 
            source.BaseList?.Types.Select(baseType => baseType) ?? Enumerable.Empty<BaseTypeSyntax>();
        
        return baseTypes.Any(baseType => baseType.ToString() ==interfaceName);
    }
}