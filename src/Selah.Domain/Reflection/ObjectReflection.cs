using Dapper;
using System.Text;

namespace Selah.Domain.Reflection
{
    public static class ObjectReflection
    {
        /*
         * static method to data access objects to match PostgreSQL naming conventions
         */
        public static DynamicParameters ConvertToSnakecase<T>(T entity) where T : class
        {
            var parameters = new DynamicParameters();
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                // Convert PascalCase property name to snake_case column name
                var columnName = propertyInfo.Name.ToSnakeCase();

                // Add parameter with snake_case column name and property value
                parameters.Add(columnName, propertyInfo.GetValue(entity));
            }

            return parameters;
        }

        private static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var sb = new StringBuilder();
            sb.Append(char.ToLower(input[0]));

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    sb.Append("_");
                }

                sb.Append(char.ToLower(input[i]));
            }

            return sb.ToString();
        }
    }
}
