using System.Runtime.ExceptionServices;
using GraphQL.Types;
using Gyminize_API.Data.Model;

namespace Gyminize_API.GraphQl.Types
{
    public class Customer_healthType : ObjectGraphType<Customer_health>
    {
        public Customer_healthType()
        {

            Field(x => x.customer_id, type: typeof(IntGraphType)).Description("Id of customer");
            Field(x => x.gender, type: typeof(IntGraphType)).Description("Gender");
            Field(x => x.age, type: typeof(IntGraphType)).Description("Age");
            Field(x => x.activity_level, type: typeof(IntGraphType)).Description("Activity Level");
            Field(x => x.body_fat, type: typeof(DecimalGraphType)).Description("Body Fat");
            Field(x => x.tdee, type: typeof(DecimalGraphType)).Description("TDEE");
        }
    }
}


