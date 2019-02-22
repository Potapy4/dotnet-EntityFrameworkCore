﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Relational.Query.PipeLine.SqlExpressions
{
    public class SqlCastExpression : SqlExpression
    {
        public SqlCastExpression(
            SqlExpression operand,
            Type type,
            RelationalTypeMapping typeMapping)
            : base(type, typeMapping, false, true)
        {
            Check.NotNull(operand, nameof(operand));

            Operand = operand.ConvertToValue(true);
        }

        private SqlCastExpression(
            SqlExpression operand,
            Type type,
            RelationalTypeMapping typeMapping,
            bool treatAsValue)
            : base(type, typeMapping, false, treatAsValue)
        {
            Operand = operand;
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var operand = (SqlExpression)visitor.Visit(Operand);

            return operand != Operand
                ? new SqlCastExpression(operand, Type, TypeMapping)
                : this;
        }

        public override SqlExpression ConvertToValue(bool treatAsValue)
        {
            return new SqlCastExpression(Operand, Type, TypeMapping, treatAsValue);
        }

        public SqlExpression Operand { get; }
    }
}
