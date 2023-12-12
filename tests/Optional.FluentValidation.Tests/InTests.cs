namespace Optional.FluentValidation.Tests
{
    public class InTests
    {
        public class Model
        {
            public int Id { get; set; }

            public int? NullableId { get; set; }

            public Optional<int> OptionalId { get; set; }
        }

        public class ModelValidator : AbstractValidator<Model>
        {
            public ModelValidator()
            {
                RuleFor(model => model.Id).In([1, 2]);
                RuleFor(model => model.NullableId).In([1, 2]);
                RuleFor(model => model.OptionalId).In(new HashSet<int> { 1, 2 });
            }
        }

        private readonly ModelValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Id_Is_Not_In_Set()
        {
            Model model = new() { Id = 3 };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Id);
        }

        [Fact]
        public void Should_Have_Error_When_NullableId_Is_Not_In_Set()
        {
            Model model = new() { NullableId = 3 };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.NullableId);
        }

        [Fact]
        public void Should_Have_Error_When_OptionalId_Is_Not_In_Set()
        {
            Model model = new() { OptionalId = 3 };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.OptionalId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Id_Is_In_Set()
        {
            Model model = new() { Id = 1 };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.Id);
        }

        [Fact]
        public void Should_Not_Have_Error_When_NullableId_Is_In_Set()
        {
            Model model = new() { NullableId = 1 };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.NullableId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_OptionalId_Is_In_Set()
        {
            Model model = new() { OptionalId = 1 };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.OptionalId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_NullableId_Is_Null()
        {
            Model model = new() { NullableId = default };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.NullableId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_OptionalId_Is_Null()
        {
            Model model = new() { OptionalId = default };
            TestValidationResult<Model>? result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.OptionalId);
        }
    }
}