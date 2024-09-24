using Xunit;

namespace NNMotor.Tests
{
    public static class TestExtensions
    {
        public static T ShouldNotNull<T>(this T obj)
        {
            Assert.NotNull(obj);
            return obj;
        }

        public static T ShouldEqual<T>(this T actual, object expected)
        {
            Assert.Equal(expected, actual);
            return actual;
        }

        public static void ShouldEqual(this object actual, object expected)
        {
            Assert.Equal(expected, actual);
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, Action testAction)
        {
            return Assert.Throws(exceptionType, testAction);
        }

        public static void ShouldBe<T>(this object actual)
        {
            Assert.IsType<T>(actual);
        }

        public static void ShouldBeNull(this object actual)
        {
            Assert.Null(actual);
        }

        public static void ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.Same(expected, actual);
        }

        public static void ShouldBeNotBeTheSameAs(this object actual, object expected)
        {
            Assert.NotSame(expected, actual);
        }

        public static T CastTo<T>(this object source)
        {
            return (T)source;
        }

        public static void ShouldBeTrue(this bool source)
        {
            Assert.True(source);
        }

        public static void ShouldBeFalse(this bool source)
        {
            Assert.False(source);
        }

        public static void AssertSameStringAs(this string actual, string expected)
        {
            if (!string.Equals(actual, expected, StringComparison.InvariantCultureIgnoreCase))
            {
                var message = $"Expected {expected} but was {actual}";
                throw new Exception(message);
            }
        }

        public static T PropertiesShouldEqual<T>(this T actual, T expected, params string[] filters)
        {
            var properties = typeof(T).GetProperties().ToList();

            var filterByEntities = new List<string>();
            var values = new Dictionary<string, object>();

            foreach (var propertyInfo in properties.ToList())
            {
                //skip by filter
                if (filters.Any(f => f == propertyInfo.Name || f + "Id" == propertyInfo.Name) || propertyInfo.Name == "Id")
                    continue;
                var value = propertyInfo.GetValue(actual);
                values.Add(propertyInfo.Name, value);

                if (value == null)
                    continue;

                //skip array and System.Collections.Generic types
                if (value.GetType().IsArray || value.GetType().Namespace == "System.Collections.Generic")
                {
                    properties.Remove(propertyInfo);
                    continue;
                }

                //skip BaseEntity types and entity Id
                filterByEntities.Add(propertyInfo.Name + "Id");
                properties.Remove(propertyInfo);
            }

            foreach (var propertyInfo in properties.Where(p => values.ContainsKey(p.Name)))
            {
                if (filterByEntities.Any(f => f == propertyInfo.Name))
                    continue;

                try
                {
                    Assert.Equal(values[propertyInfo.Name], propertyInfo.GetValue(expected));
                }
                catch (Exception)
                {
                    throw new Exception($"The property \"{typeof(T).Name}.{propertyInfo.Name}\" of these objects is not equal");
                }
            }

            return actual;
        }
    }
}
