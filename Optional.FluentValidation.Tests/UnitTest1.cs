using FluentValidation;
using Nness.Text.Json;

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
                RuleFor(model => model.List).NotEmpty();
            }
        }

        [Fact]
        public void Test1()
        {
        }
    }
}