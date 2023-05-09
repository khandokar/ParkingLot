using ApplicationCore.Entities;
using System.Data;
using System.Data.SqlClient;

namespace ApplicationCore.Specifications
{
    public class ParkInByTagNumber : Specification<ParkIn>
    {
        private string _tagNumber;

        public ParkInByTagNumber(string tagNumber)
        {
            _tagNumber = tagNumber;

            Where = "TagNumber=@TagNumber";

            OrderBy = "Id";
        }
        public override List<SqlParameter> ToParameters()
        {

            List<SqlParameter> param = new List<SqlParameter>();

            SqlParameter tagNameParam = new SqlParameter();
            tagNameParam.SqlDbType = SqlDbType.VarChar;
            tagNameParam.ParameterName = "@TagNumber";
            tagNameParam.Value = _tagNumber;

            param.Add(tagNameParam);

            return param;
        }
    }
}
