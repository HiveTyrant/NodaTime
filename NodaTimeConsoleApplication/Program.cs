using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.TimeZones;

namespace NodaTimeConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var now = SystemClock.Instance.Now;

            DateTimeZone tz = DateTimeZoneProviders.Bcl.GetSystemDefault();
            ZonedDateTime zonedNow = now.InZone(tz);
            Console.WriteLine("Current local time (Bcl): {0}", zonedNow);

            var tzdbZones = TzdbDateTimeZoneSource.Default.GetIds().OrderBy(id => id);
            var stdZones = TimeZoneInfo.GetSystemTimeZones().ToList();

            // Obsolete way of create a list of Tzdb to Bcl mappings (at least some of 'em)
            // from http://stackoverflow.com/questions/14968554/convert-nodatime-datetimezone-into-timezoneinfo
            var tzdbTateTimeZoneSource = TzdbDateTimeZoneSource.Default;
            var tzdbToBclMap = new SortedDictionary<string, string>();
            foreach (var bclZone in TimeZoneInfo.GetSystemTimeZones())
            {
                var nodaId = tzdbTateTimeZoneSource.MapTimeZoneId(bclZone);
                if (nodaId != null) tzdbToBclMap[nodaId] = bclZone.Id;
            }


            BclDateTimeZoneSource bclDateTimeZoneSource = new BclDateTimeZoneSource();
            var bclZones = bclDateTimeZoneSource.GetIds().OrderBy(id => id).ToList();
            foreach (var bclZone in bclZones)
            {

                
            }



            var x1 = TzdbDateTimeZoneSource.Default.CanonicalIdMap;
            var x2 = TzdbDateTimeZoneSource.Default.Aliases;
            var x3 = TzdbDateTimeZoneSource.Default.TzdbVersion;
            var x4 = TzdbDateTimeZoneSource.Default.ZoneLocations;
            var x5 = TzdbDateTimeZoneSource.Default.WindowsMapping;

            // Alternative way to convert between Tzdb and Bcl
            //string bclZoneId = TzdbDateTimeZoneSource.Default..WindowsMapping..TranslateToBcl(tzdbZoneId);

            //string tzdbZoneId = TzdbDateTimeZoneSource.Default.WindowsMapping.TranslateToTzdb(bclZoneId);

            tz = DateTimeZoneProviders.Bcl["Central Europe Standard Time"];
            zonedNow = now.InZone(tz);
            Console.WriteLine("CEST local time (Bcl): {0}", zonedNow);

            tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            zonedNow = now.InZone(tz);
            Console.WriteLine("Current local time (Tzdb): {0}", zonedNow);

            tz = DateTimeZoneProviders.Tzdb["Europe/London"];
            zonedNow = now.InZone(tz);
            Console.WriteLine("London current time (Tzdb): {0}", zonedNow);


            foreach (var id in DateTimeZoneProviders.Tzdb.Ids)
            {
                DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[id];
                ZonedDateTime zonedDateTime = now.InZone(timeZone);
                Console.WriteLine("Time in {0} with offest {1}: {2}", timeZone, zonedDateTime.Offset,
                    zonedDateTime.ToString("yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture));
            }

            Console.ReadLine();
        }
    }
}
