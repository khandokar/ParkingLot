using ApplicationCore.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ApplicationCore.Specifications
{
    public class ParkInByTagNumber : Specification<ParkIn>
    {
        private string tagNumber;

        public ParkInByTagNumber(string tagNumber)
        {
            this.tagNumber = tagNumber;

            Where = "TagNumber=@TagNumber";

            OrderBy = "Id";
        }

        public override List<SqlParameter> ToParameters()
        {

            List<SqlParameter> param = new List<SqlParameter>();

            SqlParameter tagNameParam = new SqlParameter();
            tagNameParam.SqlDbType = SqlDbType.VarChar;
            tagNameParam.ParameterName = "@TagNumber";
            tagNameParam.Value = tagNumber;

            param.Add(tagNameParam);

            return param;
        }

        public override Expression<Func<ParkIn, bool>> ToExpression()
        {
            return pi => pi.TagNumber == tagNumber;
        }
    }
}
