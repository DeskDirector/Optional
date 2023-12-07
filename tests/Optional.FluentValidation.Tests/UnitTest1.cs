using FluentValidation;
using DeskDirector.Text.Json;
using DeskDirector.Text.Json.Validation;

namespace Optional.FluentValidation.Tests
{
    public class UnitTest1
    {
        public class Model
        {
            public Optional<List<string>> List { get; set; }

            public Optional<ICollection<string>> Alternative { get; set; }

            public Optional<string[]> Array { get; set; }

            public OptionalCollection<string> Collection { get; set; }
        }

        public class ModelValidator : AbstractValidator<Model>
        {
            public ModelValidator()
            {
                RuleFor(model => model.List).NotEmpty().NotNull();
                RuleFor(model => model.Alternative).NotEmpty().NotNull();
                RuleFor(model => model.Alternative).NotEmpty().NotNull();
                RuleFor(model => model.Collection).NotEmpty().NotNull();
            }
        }

        [Fact]
        public void Test1()
        {
        }
    }
}