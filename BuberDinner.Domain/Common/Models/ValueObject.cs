namespace BuberDinner.Domain.Common.Models
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Get the components that define equality for this value object.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Overrides the default Equals method to compare value objects based on their components.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            // Check for null and type compatibility
            if (obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            // Cast the object to ValueObject for comparison
            var valueObject = (ValueObject)obj;

            // Compare the equality components
            return GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
        }

        public bool Equals(ValueObject? other)
        {
            return Equals((object?)other);
        }
    }

    public class Price : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Price(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}