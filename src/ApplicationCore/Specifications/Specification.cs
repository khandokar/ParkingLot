﻿using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ApplicationCore.Specifications
{
    public abstract class Specification<T>
    {
        public string? Where { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }

        public string? OrderBy { get; set; }

        public abstract List<SqlParameter> ToParameters();

        public abstract Expression<Func<T, bool>> ToExpression();


    }
}
