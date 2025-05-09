using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace LabApi.SourceGenerators.Tests;

public class InitializeWrapperSourceGeneratorTests
{
    private const string SourceClassText = $$"""
                                       using Generators;

                                       namespace TestNamespace;

                                       private class TestClass
                                       {
                                       
                                           [{{Core.GeneratorNamespace}}.InitializeWrapper(15)]
                                           private static void SecondInitialize()
                                           { 
                                           }
                                       
                                           [{{Core.GeneratorNamespace}}.InitializeWrapper(0)]
                                           private static void FirstInitialize()
                                           {
                                           }
                                       
                                           [{{Core.GeneratorNamespace}}.InitializeWrapper(100)]
                                           private static void ThirdInitialize()
                                           {
                                           }
                                       
                                           [InitializeWrapper]
                                           private static void FourthInitialize()
                                           {
                                           }
                                       }
                                       """;

    private const string ExpectedGeneratedClassText = """
                                                      // <auto-generated>
                                                      // This code was generated by a source generator.
                                                      // Changes to this file will be lost during the next build.
                                                      // Instead, please modify the methods marked with the [InitializeWrapper] attribute, or the source generator itself.
                                                      // Methods marked with the [InitializeWrapper] attribute will be called during LabApi startup.
                                                      // </auto-generated>
                                                      namespace LabApi.Loader;
                                                      public static partial class PluginLoader
                                                      {
                                                          static partial void InitializeWrappers()
                                                          {
                                                              TestNamespace.TestClass.FirstInitialize(); // 0
                                                              TestNamespace.TestClass.SecondInitialize(); // 15
                                                              TestNamespace.TestClass.ThirdInitialize(); // 100
                                                              TestNamespace.TestClass.FourthInitialize(); // 128
                                                          }
                                                      }
                                                      """;

    [Fact]
    public void InitializeWrapperSourceGenerator_GeneratesExpectedCode()
    {
        InitializeWrapperSourceGenerator generator = new ();

        CSharpGeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        Compilation compilation = CSharpCompilation.Create(nameof(InitializeWrapperSourceGeneratorTests),
            new[] { CSharpSyntaxTree.ParseText(SourceClassText) },
            new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            });

        // Run generators and retrieve all results.
        GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();

        SyntaxTree generatedFileSyntax = runResult.GeneratedTrees.Single(t => t.FilePath.EndsWith("PluginLoader.g.cs"));

        Assert.Equal(ExpectedGeneratedClassText, generatedFileSyntax.GetText().ToString(), ignoreLineEndingDifferences: true);
    }
}