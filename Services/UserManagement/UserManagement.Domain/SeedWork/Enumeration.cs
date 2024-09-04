using System.Reflection;

namespace UserManagement.Domain.SeedWork
{
    public abstract class Enumeration : IComparable
    {
        protected Enumeration(string id, string name) => (Id, Name) = (id, name);

        public string Id { get; private set; }

        public string Name { get; private set; }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        public static T FromValue<T>(string value) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            IEnumerable<T> fields = typeof(T).GetFields(BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

            IEnumerable<T> properties = typeof(T).GetProperties(BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

            IEnumerable<T> allEnumerations = fields.Cast<T>().Concat(properties.Cast<T>());

            return allEnumerations;
        }


        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Name;

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public static bool TryGetFromValueOrName<T>(string valueOrName, out T enumeration) where T : Enumeration
        {
            return TryParse(item => item.Name == valueOrName, out enumeration) ||
                   int.TryParse(valueOrName, out var value) &&
                   TryParse(item => item.Name == value.ToString(), out enumeration);
        }
        private static bool TryParse<TEnumeration>(Func<TEnumeration, bool> predicate, out TEnumeration enumeration) where TEnumeration : Enumeration
        {
            enumeration = GetAll<TEnumeration>().FirstOrDefault(predicate);
            return enumeration != null;
        }
    }
}
