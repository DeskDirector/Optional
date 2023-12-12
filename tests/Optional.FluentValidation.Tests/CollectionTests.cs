namespace Optional.FluentValidation.Tests
{
    public class CollectionTests
    {
        public class Model
        {
            public List<string>? List { get; set; }

            public ICollection<string>? Collection { get; set; }

            public string[]? Array { get; set; }
        }

        public class ModelValidator : AbstractValidator<Model>
        {
            public ModelValidator()
            {
                RuleFor(model => model.List).NotEmpty().NotNull().Count(1, 2);
                RuleFor(model => model.Collection).NotEmpty().NotNull().Count(1, 2);
                RuleFor(model => model.Array).NotEmpty().NotNull().Count(1, 2);
            }
        }

        private readonly ModelValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_List_Is_Null()
        {
            Model model = new() { List = null };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.List);
        }

        [Fact]
        public void Should_Have_Error_When_Alternative_Is_Null()
        {
            Model model = new() { Collection = null };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Collection);
        }

        [Fact]
        public void Should_Have_Error_When_Array_Is_Null()
        {
            Model model = new() { Array = null };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Array);
        }

        [Fact]
        public void Should_Have_Error_When_List_Is_Empty()
        {
            Model model = new() { List = new List<string>() };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.List);
        }

        [Fact]
        public void Should_Have_Error_When_Alternative_Is_Empty()
        {
            Model model = new() { Collection = new List<string>() };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Collection);
        }

        [Fact]
        public void Should_Have_Error_When_Array_Is_Empty()
        {
            Model model = new() { Array = new string[] { } };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Array);
        }

        [Fact]
        public void Should_Have_Error_When_Collection_Is_Empty()
        {
            Model model = new() { Collection = new List<string>() };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Collection);
        }

        // Tests for checking the size of the collection
        [Theory]
        [InlineData(0)] // No elements
        [InlineData(3)] // More than 2 elements
        public void Should_Have_Error_When_List_Count_Is_Invalid(int count)
        {
            Model model = new() { List = Enumerable.Repeat("Item", count).ToList() };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.List);
        }

        // Similarly, create tests for Alternative, Array, and Collection properties

        // Tests for valid scenarios
        [Fact]
        public void Should_Not_Have_Error_When_Valid()
        {
            Model model = new() {
                List = new List<string> { "Item1" },
                Collection = new List<string> { "Item1" },
                Array = new[] { "Item1" }
            };

            TestValidationResult<Model> result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}