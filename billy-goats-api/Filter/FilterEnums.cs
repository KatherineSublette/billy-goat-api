using System;
namespace BillyGoats.Api.Filter
{
    public enum FilterOperations
    {
        Eq,
        Neq,
        Gt,
        Gte,
        Lt,
        Lte,
        Sw,
        Ew,
        Ct,
        Nct,
        HasKey,
        NoKey
    }

    public enum LogicalOperations
    {
        AND,
        OR
    }

    public enum Aggregations
    {
        Count,
        Any,
        Sum,
        Max,
        Min
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }

    public static class SpecialDate
    {
        const string YesterdayBegin = "~yesterday-begin";
        const string TodayBegin = "~today-begin";
        const string TomorrowBegin = "~tomorrow-begin";

        const string LastWeekBegin = "~lastweek-begin";
        const string ThisWeekBegin = "~thisweek-begin";
        const string NextWeekBegin = "~nextweek-begin";

        const string LastMonthBegin = "~lastmonth-begin";
        const string ThisMonthBegin = "~thismonth-begin";
        const string NextMonthBegin = "~nextmonth-begin";

        const string LastQuarterBegin = "~lastquarter-begin";
        const string ThisQuarterBegin = "~thisquarter-begin";
        const string NextQuarterBegin = "~nextquarter-begin";

        const string LastYearBegin = "~lastyear-begin";
        const string ThisYearBegin = "~thisyear-begin";
        const string NextYearBegin = "~nextyear-begin";

        const string LastFiscalQuarterBegin = "~lastfiscalquarter-begin";
        const string ThisFiscalQuarterBegin = "~thisfiscalquarter-begin";
        const string NextFiscalQuarterBegin = "~nextfiscalquarter-begin";

        const string LastFiscalYearBegin = "~lastfiscalyear-begin";
        const string ThisFiscalYearBegin = "~thisfiscalyear-begin";
        const string NextFiscalYearBegin = "~nextfiscalyear-begin";



        const string YesterdayEnd = "~yesterday-end";
        const string TodayEnd = "~today-begin-end";
        const string TomorrowEnd = "~tomorrow-end";

        const string LastWeekEnd = "~lastweek-end";
        const string ThisWeekEnd = "~thisweek-end";
        const string NextWeekEnd = "~nextweek-end";

        const string LastMonthEnd = "~lastmonth-end";
        const string ThisMonthEnd = "~thismonth-end";
        const string NextMonthEnd = "~nextmonth-end";

        const string LastQuarterEnd = "~lastquarter-end";
        const string ThisQuarterEnd = "~thisquarter-end";
        const string NextQuarterEnd = "~nextquarter-end";

        const string LastYearEnd = "~lastyear-end";
        const string ThisYearEnd = "~thisyear-end";
        const string NextYearEnd = "~nextyear-end";

        const string LastFiscalQuarterEnd = "~lastfiscalquarter-end";
        const string ThisFiscalQuarterEnd = "~thisfiscalquarter-end";
        const string NextFiscalQuarterEnd = "~nextfiscalquarter-end";

        const string LastFiscalYearEnd = "~lastfiscalyear-end";
        const string ThisFiscalYearEnd = "~thisfiscalyear-end";
        const string NextFiscalYearEnd = "~nextfiscalyear-end";

        public static string ToDate(this string svalue, string fiscalYearStart)
        {
            double mod(double num, double divisor)
            {
                return (num % divisor + num) % divisor;
            }
            var today = DateTime.Today;
            var weekBegin = today.AddDays(-mod((int)today.DayOfWeek-1, 7));//ISO weeks begin on Monday
            var monthBegin = today.AddDays(-DateTime.Today.Day + 1);
            var yearBegin = monthBegin.AddMonths(-today.Month + 1);
            int quarter = (today.Month -1) / 3;
            var quarterBegin = yearBegin.AddMonths(quarter * 3);
            var fmd = fiscalYearStart.Split("/");

            var fiscalYearBegin = yearBegin.AddMonths(int.Parse(fmd[0]) - 1).AddDays(int.Parse(fmd[1]) - 1);
            if (today < fiscalYearBegin)
            {
                // still in last fiscal Year
                fiscalYearBegin = fiscalYearBegin.AddYears(-1);
            }
            quarter = today >= fiscalYearBegin && today < fiscalYearBegin.AddMonths(3) ? 0 :
                                today >= fiscalYearBegin.AddMonths(3) && today < fiscalYearBegin.AddMonths(6) ? 1 :
                                today >= fiscalYearBegin.AddMonths(6) && today < fiscalYearBegin.AddMonths(9) ? 2 : 3;

            var fiscalQuaterBegin = fiscalYearBegin.AddMonths(quarter * 3);

            var ret = today;
            switch (svalue)
            {
                case YesterdayBegin:
                    ret = today.AddDays(-1);
                    break;
                case TodayBegin:
                    ret = today;
                    break;
                case TomorrowBegin:
                    ret = today.AddDays(1);
                    break;

                case LastWeekBegin:
                    ret = weekBegin.AddDays(-7);
                    break;
                case ThisWeekBegin:
                    ret = weekBegin;
                    break;
                case NextWeekBegin:
                    ret = weekBegin.AddDays(7);
                    break;

                case LastMonthBegin:
                    ret = monthBegin.AddMonths(-1);
                    break;
                case ThisMonthBegin:
                    ret = monthBegin;
                    break;
                case NextMonthBegin:
                    ret = monthBegin.AddMonths(1);
                    break;

                case LastQuarterBegin:
                    ret = quarterBegin.AddMonths(-3);
                    break;
                case ThisQuarterBegin:
                    ret = quarterBegin;
                    break;
                case NextQuarterBegin:
                    ret = quarterBegin.AddMonths(3);
                    break;

                case LastFiscalQuarterBegin:
                    ret = fiscalQuaterBegin.AddMonths(-3);
                    break;
                case ThisFiscalQuarterBegin:
                    ret = fiscalQuaterBegin;
                    break;
                case NextFiscalQuarterBegin:
                    ret = fiscalQuaterBegin.AddMonths(3);
                    break;

                case LastYearBegin:
                    ret = yearBegin.AddYears(-1); 
                    break;
                case ThisYearBegin:
                    ret = yearBegin;
                    break;
                case NextYearBegin:
                    ret = yearBegin.AddYears(1);
                    break;

                case LastFiscalYearBegin:
                    ret = fiscalYearBegin.AddYears(-1);
                    break;
                case ThisFiscalYearBegin:
                    ret = fiscalYearBegin;
                    break;
                case NextFiscalYearBegin:
                    ret = fiscalYearBegin.AddYears(1);
                    break;




                case YesterdayEnd:
                    ret = today;
                    break;
                case TodayEnd:
                    ret = today.AddDays(1);
                    break;
                case TomorrowEnd:
                    ret = today.AddDays(2);
                    break;

                case LastWeekEnd:
                    ret = weekBegin;
                    break;
                case ThisWeekEnd:
                    ret = weekBegin.AddDays(7);
                    break;
                case NextWeekEnd:
                    ret = weekBegin.AddDays(14);
                    break;

                case LastMonthEnd:
                    ret = monthBegin;
                    break;
                case ThisMonthEnd:
                    ret = monthBegin.AddMonths(1);
                    break;
                case NextMonthEnd:
                    ret = monthBegin.AddMonths(2);
                    break;

                case LastQuarterEnd:
                    ret = quarterBegin;
                    break;
                case ThisQuarterEnd:
                    ret = quarterBegin.AddMonths(3); 
                    break;
                case NextQuarterEnd:
                    ret = quarterBegin.AddMonths(6);
                    break;

                case LastFiscalQuarterEnd:
                    ret = fiscalQuaterBegin;
                    break;
                case ThisFiscalQuarterEnd:
                    ret = fiscalQuaterBegin.AddMonths(3);
                    break;
                case NextFiscalQuarterEnd:
                    ret = fiscalQuaterBegin.AddMonths(6); 
                    break;

                case LastYearEnd:
                    ret = yearBegin;
                    break;
                case ThisYearEnd:
                    ret = yearBegin.AddYears(1);
                    break;
                case NextYearEnd:
                    ret = yearBegin.AddYears(2);
                    break;

                case LastFiscalYearEnd:
                    ret = fiscalYearBegin;
                    break;
                case ThisFiscalYearEnd:
                    ret = fiscalYearBegin.AddMonths(3);
                    break;
                case NextFiscalYearEnd:
                    ret = fiscalYearBegin.AddMonths(6);
                    break;
                default:
                    break;
            }
            return ret.ToShortDateString();
        }
    }
}
