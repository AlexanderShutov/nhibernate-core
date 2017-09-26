﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1192
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is Oracle8iDialect);
		}

		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(new ObjectA { FontType = Status.Bold | Status.Italic, Name = "Object1" });
				session.Save(new ObjectA { FontType = Status.Italic, Name = "Object2" });
				session.Save(new ObjectA { FontType = Status.Underlined, Name = "Object2" });
				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Delete("from ObjectA");
				transaction.Commit();
			}
		}

		[Test]
		public async Task BitwiseAndWorksCorrectlyAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var query = session.CreateQuery("from ObjectA o where (o.FontType & 1) > 0");
				var result = await (query.ListAsync());
				
				Assert.That(result, Has.Count.EqualTo(1));
				query = session.CreateQuery("from ObjectA o where (o.FontType & 2) > 0");
				result = await (query.ListAsync());
				Assert.That(result, Has.Count.EqualTo(2));

				query = session.CreateQuery("from ObjectA o where (o.FontType & 4) > 0");
				result = await (query.ListAsync());
				Assert.That(result, Has.Count.EqualTo(1));
			}
		}

		[Test]
		public async Task BitwiseOrWorksCorrectlyAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var query = session.CreateQuery("from ObjectA o where (o.FontType | 2)  = (1|2) ");
				var result = await (query.ListAsync());
				Assert.That(result, Has.Count.EqualTo(1));
			}
		}
	}
}