//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Oxite
{
    public class ArchiveData
    {
        private static readonly string _defaultString = DateTime.Now.Year.ToString();

        private static readonly Regex archiveDataRegex =
            new Regex(@"^(?<year>\d{4})(?:/(?<month>\d{1,2})?(?:/(?<day>\d{1,2})?)?)?(?:/(?:page(?<page>\d+))?)?$",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ArchiveData()
        {
        }

        public ArchiveData(string rawData)
        {
            if (!string.IsNullOrEmpty(rawData))
            {
                Match archiveDataMatch = archiveDataRegex.Match(rawData);

                int year = 0;
                if (archiveDataMatch.Groups["year"].Success &&
                    int.TryParse(archiveDataMatch.Groups["year"].Value, out year))
                {
                    Year = year;
                }

                int month = 0;
                if (archiveDataMatch.Groups["month"].Success &&
                    int.TryParse(archiveDataMatch.Groups["month"].Value, out month))
                {
                    Month = month;
                }

                int day = 0;
                if (archiveDataMatch.Groups["day"].Success &&
                    int.TryParse(archiveDataMatch.Groups["day"].Value, out day))
                {
                    Day = day;
                }

                int page = 0;
                if (archiveDataMatch.Groups["page"].Success &&
                    int.TryParse(archiveDataMatch.Groups["page"].Value, out page))
                {
                    Page = page;
                }
                else
                {
                    Page = 1;
                }
            }
        }

        public int Page { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public static string DefaultString
        {
            get
            {
                return _defaultString;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Year > 0)
            {
                sb.AppendFormat("{0}/", Year);
            }

            if (Month > 0)
            {
                sb.AppendFormat("{0}/", Month);
            }

            if (Day > 0)
            {
                sb.AppendFormat("{0}/", Day);
            }

            if (Page > 1)
            {
                sb.AppendFormat("page{0}/", Page);
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }
    }
}