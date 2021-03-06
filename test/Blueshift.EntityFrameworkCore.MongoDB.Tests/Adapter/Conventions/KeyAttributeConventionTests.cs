﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Blueshift.EntityFrameworkCore.MongoDB.Adapter.Conventions;
using Blueshift.EntityFrameworkCore.MongoDB.SampleDomain;
using MongoDB.Bson.Serialization;
using Xunit;

namespace Blueshift.EntityFrameworkCore.MongoDB.Tests.Adapter.Conventions
{
    public class KeyAttributeConventionTests
    {
        [Theory]
        [InlineData(nameof(Employee.Id), true)]
        [InlineData(nameof(Employee.FirstName), false)]
        [InlineData(nameof(Employee.Age), false)]
        public void Sets_id_member_when_key_attribute_present(string memberName, bool keyExpected)
        {
            MemberInfo memberInfo = typeof(Employee)
                .GetTypeInfo()
                .GetProperty(memberName);
            Assert.NotNull(memberInfo);
            Assert.Equal(keyExpected, memberInfo.IsDefined(typeof(KeyAttribute), false));
            var keyAttributeConvention = new KeyAttributeConvention();
            var bsonClasspMap = new BsonClassMap<Employee>();
            BsonMemberMap bsonMemberMap = bsonClasspMap.MapMember(memberInfo);
            keyAttributeConvention.Apply(bsonMemberMap);
            Assert.Equal(keyExpected, bsonClasspMap.IdMemberMap == bsonMemberMap);
        }
    }
}