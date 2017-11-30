using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using NodaTime;
using NodaTime.TimeZones;
using NodaTimeWpfApplication.Utils;

namespace NodaTimeWpfApplication.ViewModels
{
    public class MainViewViewModel : ViewModelBase
    {
        #region Private fields and properties

        private IOrderedEnumerable<string> _tzdbZones;
        private string _selectedTzdb;
        private List<string> _stdZones;
        private string _selectedStd;
        private ICommand _debugTestCommand;

        #endregion

        #region Construction and initialization

        public override void Initialize()
        {
            TzdbZones = TzdbDateTimeZoneSource.Default.GetIds().OrderBy(id => id);
            StdZones = TimeZoneInfo.GetSystemTimeZones().OrderBy(i => i.BaseUtcOffset).ThenBy(i => i.StandardName).Select(i => $"{i.StandardName} - {i.DisplayName}" ).ToList();
        }

        #endregion

        #region Public properties

        #region Bindable properties used from MainViewUserControl

        public ICommand DebugTestCommand => _debugTestCommand ?? (_debugTestCommand = new RelayCommand(arg => Test(), null));
            
        public IOrderedEnumerable<string> TzdbZones
        {
            get => _tzdbZones;
            set => SetProperty(ref _tzdbZones, value);
        }

        public string SelectedTzdb
        {
            get => _selectedTzdb;
            set => SetProperty(ref _selectedTzdb, value);
        }

        public List<string> StdZones
        {
            get => _stdZones;
            set => SetProperty(ref _stdZones, value);
        }

        public string SelectedStd
        {
            get => _selectedStd;
            set => SetProperty(ref _selectedStd, value);
        }

        #endregion

        #region ViewModels for sub-views
        #endregion

        #endregion

        #region Helper methods

        public void Test()
        {
            DateTimeZone tz;
            Instant now = SystemClock.Instance.GetCurrentInstant();

            tz = DateTimeZoneProviders.Bcl.GetSystemDefault(); // = Romance Standard Time => (UTC+01:00) Brussels, Copenhagen, Madrid, Paris
            ZonedDateTime zonedNow = now.InZone(tz);
            Console.WriteLine("Current local time (Bcl): {0}", zonedNow);

            tz = DateTimeZoneProviders.Bcl["Central Europe Standard Time"];
            zonedNow = now.InZone(tz);
            Console.WriteLine("CEST local time (Bcl): {0}", zonedNow);

            tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            zonedNow = now.InZone(tz);
            Console.WriteLine("Current local time (Tzdb): {0}", zonedNow);

            tz = DateTimeZoneProviders.Tzdb["Europe/Warsaw"];
            zonedNow = now.InZone(tz);
            Console.WriteLine("Europe/Warsaw current time (Tzdb): {0}", zonedNow);

            // Obsolete way of create a list of Tzdb to Bcl mappings (at least some of 'em)
            // from http://stackoverflow.com/questions/14968554/convert-nodatime-datetimezone-into-timezoneinfo
            TzdbDateTimeZoneSource tzdbTateTimeZoneSource = TzdbDateTimeZoneSource.Default;
            ReadOnlyCollection<TimeZoneInfo> systemTimeZones = TimeZoneInfo.GetSystemTimeZones();
            SortedDictionary<string, string> tzdbToBclMap = new SortedDictionary<string, string>();
            foreach (TimeZoneInfo bclZone in systemTimeZones)
            {
                //var nodaId = tzdbTateTimeZoneSource.MapTimeZoneId(bclZone);
                if (tzdbTateTimeZoneSource.WindowsMapping.PrimaryMapping.TryGetValue(bclZone.Id, out var nodaId)) // "Dateline Standard Time" => "Etc/GMT+12"
                    tzdbToBclMap[nodaId] = bclZone.Id;
            }

            var NodaTimeZoneLocations = tzdbTateTimeZoneSource.ZoneLocations;

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

            foreach (var id in DateTimeZoneProviders.Tzdb.Ids)
            {
                DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[id];
                ZonedDateTime zonedDateTime = now.InZone(timeZone);
                Console.WriteLine("Time in {0} with offest {1}: {2}", timeZone, zonedDateTime.Offset, zonedDateTime.ToString("yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region Overrides
        #endregion
    }
}