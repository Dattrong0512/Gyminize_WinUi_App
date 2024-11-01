using GraphQL.Types;
namespace Gyminize_API.GraphQl.Types
{
    public class CustomerInputType : InputObjectGraphType
    {
        public CustomerInputType()
        {
            Name = "CustomerInput";
            Field<StringGraphType>("customer_name");
            Field<IntGraphType>("auth_type");
            Field<StringGraphType>("username");
            Field<StringGraphType>("customer_password");
         
        }
    }
}
