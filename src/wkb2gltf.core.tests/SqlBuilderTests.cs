﻿using NUnit.Framework;

namespace Wkb2Gltf.Tests
{
    public class SqlBuilderTests
    {
        [Test]
        public void TestOptionalSqlFields()
        {
            Assert.IsTrue (SqlBuilder.GetOptionalColumnsSql("haha", "hoho")== ", haha, hoho");
            Assert.IsTrue(SqlBuilder.GetOptionalColumnsSql("", "hoho") == ", hoho");
            Assert.IsTrue(SqlBuilder.GetOptionalColumnsSql("", "") == "");
            Assert.IsTrue(SqlBuilder.GetOptionalColumnsSql("haha", "") == ", haha");
        }
    }
}
