using Microsoft.Extensions.Options;
using NUnit.Framework;
using SQLFormatter.Controllers;
using SQLFormatter.Models;

namespace SQLFormatter.Tests
{
    public class SQLFormatterTests
    {
        [TestFixture]
        public class SqlFormattingControllerTests
        {
            private SQLFormattingController _controller;

            [SetUp]
            public void Setup()
            {
                _controller = new SQLFormattingController();
            }

            [Test]
            public void FormatSql_ShouldFormatWithSpacesAroundOperators()
            {
                var request = new RequestWithOptionsAndSQL
                {
                    Sql = "SELECT * FROM employees WHERE salary>10000 AND department='IT'",
                    Options = new FormattingOptions
                    {
                        SpacesAroundOperators = true
                    }
                };

                var result = _controller.FormatSql(request);

                Assert.That(result, Is.EqualTo("SELECT  *  FROM employees WHERE salary > 10000 AND department = 'IT'"));
            }

            [Test]
            public void FormatSql_ShouldFormatWithSpacesAroundParentheses()
            {
                var request = new RequestWithOptionsAndSQL
                {
                    Sql = "SELECT id, name FROM products WHERE (category='Electronics' AND price>500) OR (category='Clothing' AND price>100)",
                    Options = new FormattingOptions
                    {
                        SpacesAroundParentheses = true
                    }
                };

                var result = _controller.FormatSql(request);

                Assert.That(result, Is.EqualTo("SELECT id, name FROM products WHERE  ( category='Electronics' AND price>500 )  OR  ( category='Clothing' AND price>100 )"));
            }

            [Test]
            public void FormatSql_ShouldFormatWithSpaceAfterComma()
            {
                var request = new RequestWithOptionsAndSQL
                {
                    Sql = "SELECT id, name, age FROM users WHERE city='Sarajevo' AND country='BiH'",
                    Options = new FormattingOptions
                    {
                        SpaceAfterComma = true
                    }
                };

                var result = _controller.FormatSql(request);

                Assert.That(result, Is.EqualTo("SELECT id,  name,  age FROM users WHERE city='Sarajevo' AND country='BiH'"));
            }

            [Test]
            public void FormatSql_ShouldFormatWithSpaceBeforeComma()
            {
                var request = new RequestWithOptionsAndSQL
                {
                    Sql = "SELECT id,name,age FROM users WHERE city='Sarajevo' AND country='BiH'",
                    Options = new FormattingOptions
                    {
                        SpaceBeforeComma = true
                    }
                };

                var result = _controller.FormatSql(request);

                Assert.That(result, Is.EqualTo("SELECT id ,name ,age FROM users WHERE city='Sarajevo' AND country='BiH'"));
            }

            [Test]
            public void FormatSql_ShouldAlignEqualSigns()
            {
                var request = new RequestWithOptionsAndSQL
                {
                    Sql = "UPDATE users SET id=1,name='Sinisa',age=29 WHERE id=2",
                    Options = new FormattingOptions
                    {
                        AlignEqualOperators = true
                    }
                };

                var result = _controller.FormatSql(request);

                Assert.That(result, Is.EqualTo("UPDATE users SET id=1,name\n" +
                                               "                   ='Sinisa',age\n" +
                                               "                   =29 WHERE id\n" +
                                               "                   =2"));
            }
        }
    }
}
