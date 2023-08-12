using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using SQLFormatter.Models;
using System.Linq;

namespace SQLFormatter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SQLFormattingController : ControllerBase
    {
        [HttpPost]
        public string FormatSql([FromBody] RequestWithOptionsAndSQL requestWithOptionsAndSQL)
        {
            string formattedSql = requestWithOptionsAndSQL.Sql;

            if (requestWithOptionsAndSQL.Options.SpacesAroundOperators)
            {
                formattedSql = Regex.Replace(formattedSql, @"([=><!+\-*/%])", " $1 ");
            }

            if (requestWithOptionsAndSQL.Options.SpacesAroundParentheses)
            {
                formattedSql = Regex.Replace(formattedSql, @"(\(|\))", " $1 ");
            }

            if (requestWithOptionsAndSQL.Options.SpaceAfterComma)
            {
                formattedSql = Regex.Replace(formattedSql, @",", ", ");
            }

            if (requestWithOptionsAndSQL.Options.SpaceBeforeComma)
            {
                formattedSql = Regex.Replace(formattedSql, @",", " ,");
            }

            if (requestWithOptionsAndSQL.Options.AlignEqualOperators)
            {
                formattedSql = AlignEqualSigns(formattedSql);
            }

            return formattedSql.Trim();
        }

        private string AlignEqualSigns(string input)
        {
            string[] lines = input.Split(';');
            int firstEqualIndex = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("SET") && lines[i].Contains("="))
                {
                    firstEqualIndex = lines[i].IndexOf('=');
                    string emptyString = "";

                    string firstPartOfSQL = lines[i].Substring(0, firstEqualIndex + 1);
                    string restOfSQL = lines[i].Substring(firstEqualIndex + 1);

                    restOfSQL = Regex.Replace(restOfSQL, @"=", "\n" + emptyString.PadRight(firstEqualIndex, ' ') + "=");

                    lines[i] = firstPartOfSQL + restOfSQL;
                }
            }

            return string.Join('\n', lines);
        }
    }
}
