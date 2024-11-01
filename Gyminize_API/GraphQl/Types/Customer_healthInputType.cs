using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class Customer_healthInputType : InputObjectGraphType
    {
        public Customer_healthInputType()
        {
            Name = "Customer_healthInput";
            Field<IntGraphType>("customer_id");
            Field<IntGraphType>("gender");
            Field<IntGraphType>("age");
            Field<IntGraphType>("activity_level");
            Field<DecimalGraphType>("body_fat");
            Field<DecimalGraphType>("TDEE");
        }

    }
}


