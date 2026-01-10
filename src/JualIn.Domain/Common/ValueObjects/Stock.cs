using System.Numerics;
using CommunityToolkit.Diagnostics;

namespace JualIn.Domain.Common.ValueObjects
{
    public class Stock<T>(T value)
        where T : struct, INumber<T>
    {
        private T _value = value;

        public T Value => _value;

        public void Increase(T quantity)
        {
            _value += quantity;
        }

        public void Decrease(T quantity)
        {
            T newValue = _value - quantity;

            Guard.IsGreaterThanOrEqualTo(newValue, T.Zero, nameof(newValue));

            _value = newValue;
        }

        public void Update(T value)
        {
            Guard.IsGreaterThanOrEqualTo(value, T.Zero, nameof(value));

            _value = value;
        }

        public bool IsLowStock(T threshold)
            => _value <= threshold;

        public bool IsOutOfStock()
            => _value == T.Zero;

        public static bool operator ==(Stock<T> self, T value) => self.Value == value;
        public static bool operator !=(Stock<T> self, T value) => self.Value != value;
        public static bool operator >=(Stock<T> self, T value) => self.Value >= value;
        public static bool operator <=(Stock<T> self, T value) => self.Value <= value;
        public static bool operator >(Stock<T> self, T value) => self.Value > value;
        public static bool operator <(Stock<T> self, T value) => self.Value < value;

        public bool Equals(Stock<T>? other)
            => other is not null && Value == other.Value;

        public override bool Equals(object? obj)
            => Equals(obj as Stock<T>);

        public override int GetHashCode()
            => Value.GetHashCode();
    }
}
