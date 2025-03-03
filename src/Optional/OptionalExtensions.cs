namespace DeskDirector.Text.Json
{
    public static class OptionalExtensions
    {
        public static Optional<TTarget> Map<T, TTarget>(this Optional<T> input, Func<T, TTarget> map)
        {
            ArgumentNullException.ThrowIfNull(map);

            if (!input.IsSet(out T? value)) {
                return Optional<TTarget>.Undefined;
            }

            if (input.IsNull() || value is null) {
                return Optional<TTarget>.Null;
            }

            TTarget target = map(value);
            if (target is null) {
                throw new InvalidOperationException(
                    $"Map function converted {typeof(T).Name} to {typeof(TTarget).Name} as NULL."
                );
            }

            return new Optional<TTarget>(target);
        }

        public static OptionalCollection<TTarget> Map<T, TTarget>(this OptionalCollection<T> input,
            Func<T, TTarget> map)
        {
            ArgumentNullException.ThrowIfNull(map);

            if (!input.IsSet(out IReadOnlyCollection<T>? value)) {
                return OptionalCollection<TTarget>.Undefined;
            }

            if (input.IsNull() || value is null) {
                return OptionalCollection<TTarget>.Null;
            }

            TTarget[] targets = value.Select(map).ToArray();

            return new OptionalCollection<TTarget>(targets);
        }
    }
}