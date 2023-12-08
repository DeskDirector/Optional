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
                RuleFor(model => model.Id).In(new HashSet<int> { 1, 2 });
                RuleFor(model => model.NullableId).In(new HashSet<int?> { 1, 2 });
                RuleFor(model => model.OptionalId).In(new HashSet<int> { 1, 2 });
            }
        }

        private readonly ModelValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Id_Is_Not_In_Set()
        {
            var model = new Model { Id = 3 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.Id);
        }

        [Fact]
        public void Should_Have_Error_When_NullableId_Is_Not_In_Set()
        {
            var model = new Model { NullableId = 3 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.NullableId);
        }

        [Fact]
        public void Should_Have_Error_When_OptionalId_Is_Not_In_Set()
        {
            var model = new Model { OptionalId = 3 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(m => m.OptionalId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Id_Is_In_Set()
        {
            var model = new Model { Id = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.Id);
        }

        [Fact]
        public void Should_Not_Have_Error_When_NullableId_Is_In_Set()
        {
            var model = new Model { NullableId = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.NullableId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_OptionalId_Is_In_Set()
        {
            var model = new Model { OptionalId = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.OptionalId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_NullableId_Is_Null()
        {
            var model = new Model { NullableId = default };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.NullableId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_OptionalId_Is_Null()
        {
            var model = new Model { OptionalId = default };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(m => m.OptionalId);
        }
    }
}