namespace Optional.FluentValidation.Tests
{
    public class OptionalCollectionTests
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
                RuleFor(model => model.Array).NotEmpty().NotNull();
                RuleFor(model => model.Collection).NotEmpty().NotNull();

                RuleFor(model => model.List).Count<Model, List<string>, string>(1, 2);
                RuleFor(model => model.Alternative).Count<Model, ICollection<string>, string>(1, 2);
                RuleFor(model => model.Array).Count<Model, string[], string>(1, 2);
                RuleFor(model => model.Collection).Count(1, 2);
            }
        }

        private readonly ModelValidator _validator = new ModelValidator();

        [Fact]
        public void Should_Have_Error_When_List_Is_Null()
        {
            var model = new Model { List = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.List);
        }

        [Fact]
        public void Should_Have_Error_When_Alternative_Is_Null()
        {
            var model = new Model { Alternative = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Alternative);
        }

        [Fact]
        public void Should_Have_Error_When_Array_Is_Null()
        {
            var model = new Model { Array = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Array);
        }

        [Fact]
        public void Should_Have_Error_When_Collection_Is_Null()
        {
            var model = new Model { Collection = new OptionalCollection<string>(OptionalState.Null) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Collection);
        }

        [Fact]
        public void Should_Have_Error_When_List_Is_Empty()
        {
            var model = new Model { List = new List<string>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.List);
        }

        [Fact]
        public void Should_Have_Error_When_Alternative_Is_Empty()
        {
            var model = new Model { Alternative = new List<string>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Alternative);
        }

        [Fact]
        public void Should_Have_Error_When_Array_Is_Empty()
        {
            var model = new Model { Array = new string[] { } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Array);
        }

        [Fact]
        public void Should_Have_Error_When_Collection_Is_Empty()
        {
            var model = new Model { Collection = new OptionalCollection<string>(new List<string>()) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Collection);
        }

        // Tests for checking the size of the collection
        [Theory]
        [InlineData(0)] // No elements
        [InlineData(3)] // More than 2 elements
        public void Should_Have_Error_When_List_Count_Is_Invalid(int count)
        {
            var model = new Model { List = Enumerable.Repeat("Item", count).ToList() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.List);
        }

        // Similarly, create tests for Alternative, Array, and Collection properties

        // Tests for valid scenarios
        [Fact]
        public void Should_Not_Have_Error_When_Valid()
        {
            var model = new Model {
                List = new List<string> { "Item1" },
                Alternative = new List<string> { "Item1" },
                Array = new string[] { "Item1" },
                Collection = new OptionalCollection<string>(new List<string> { "Item1" })
            };

            TestValidationResult<Model> result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}