using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.Generation;

namespace NJsonSchema.CodeGeneration.Tests.CSharp
{
    [TestClass]
    public class CSharpObjectPropertyRequiredTests
    {
        private class ClassWithRequiredObject
        {
            [Required]
            public object Property { get; set; }
        }

        [TestMethod]
        public async Task When_property_is_required_then_required_attribute_is_rendered_in_Swagger_mode()
        {
            //// Arrange
            var schema = await JsonSchema4.FromTypeAsync<ClassWithRequiredObject>(new JsonSchemaGeneratorSettings
            {
                NullHandling = NullHandling.Swagger
            });
            var schemaData = schema.ToJson();

            //// Act
            var generator = new CSharpGenerator(schema, new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Poco,
                NullHandling = NullHandling.Swagger
            });
            var code = generator.GenerateFile("MyClass");

            //// Assert
            Assert.IsTrue(code.Contains("[System.ComponentModel.DataAnnotations.Required]"));
            Assert.IsTrue(code.Contains("public object Property { get; set; }"));
        }

        [TestMethod]
        public async Task When_property_is_required_then_required_attribute_is_rendered()
        {
            //// Arrange
            var schema = await JsonSchema4.FromTypeAsync<ClassWithRequiredObject>();
            var schemaData = schema.ToJson();

            //// Act
            var generator = new CSharpGenerator(schema, new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Poco
            });
            var code = generator.GenerateFile("MyClass");

            //// Assert
            Assert.IsTrue(code.Contains("[System.ComponentModel.DataAnnotations.Required]"));
            Assert.IsTrue(code.Contains("public object Property { get; set; }"));
        }

        private class ClassWithoutRequiredObject
        {
            public object Property { get; set; }
        }

        [TestMethod]
        public async Task When_property_is_not_required_then_required_attribute_is_not_rendered_in_Swagger_mode()
        {
            //// Arrange
            var schema = await JsonSchema4.FromTypeAsync<ClassWithoutRequiredObject>(new JsonSchemaGeneratorSettings
            {
                NullHandling = NullHandling.Swagger
            });
            var schemaData = schema.ToJson();

            //// Act
            var generator = new CSharpGenerator(schema, new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Poco,
                NullHandling = NullHandling.Swagger
            });
            var code = generator.GenerateFile("MyClass");

            //// Assert
            Assert.IsFalse(code.Contains("[Required]"));
            Assert.IsTrue(code.Contains("public object Property { get; set; }"));
        }


        [TestMethod]
        public async Task When_property_is_not_required_then_required_attribute_is_not_rendered()
        {
            //// Arrange
            var schema = await JsonSchema4.FromTypeAsync<ClassWithoutRequiredObject>();
            var schemaData = schema.ToJson();

            //// Act
            var generator = new CSharpGenerator(schema, new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Poco
            });
            var code = generator.GenerateFile("MyClass");

            //// Assert
            Assert.IsFalse(code.Contains("[Required]"));
            Assert.IsTrue(code.Contains("public object Property { get; set; }"));
        }
    }
}