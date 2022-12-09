#region OpenHolidays API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using Enbrea.Csv;
using Enbrea.Progress;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OpenHolidaysApi.DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Manager for importing raw data to database
    /// </summary>
    public class ImportManager
    {
        private readonly AppConfiguration _appConfiguration;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ProgressReport _progressReport;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportManager"/> class.
        /// </summary>
        /// <param name="appConfiguration">Configuration data</param>
        public ImportManager(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _dbContextFactory = new PooledDbContextFactory<AppDbContext>(AppDbContextOptionsFactory.CreateDbContextOptions(_appConfiguration.Database));
            _progressReport = ProgressReportFactory.CreateProgressReport(ProgressUnit.Count);
        }

        /// <summary>
        /// Executes the data import
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A task that represents the asynchronous import operation.</returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = _dbContextFactory.CreateDbContext();

                // Start...
                _progressReport.Caption($"Import global data");

                // Get base import folder
                var importFolder = new DirectoryInfo(_appConfiguration.Sources.RootFolderName);

                // Import countries
                var countryFile = new FileInfo(Path.Combine(importFolder.FullName, _appConfiguration.Sources.CountriesFileName));

                if (countryFile.Exists)
                {
                    await ImportToDatabaseAsync<CsvCountry>(dbContext, countryFile, cancellationToken);
                }

                // Import languages
                var languagesFile = new FileInfo(Path.Combine(importFolder.FullName, _appConfiguration.Sources.LanguagesFileName));

                if (languagesFile.Exists)
                {
                    await ImportToDatabaseAsync<CsvLanguage>(dbContext, languagesFile, cancellationToken);
                }

                // Everything is fine!
                _progressReport.Success("Data successfully imported!");

                // Import regional data
                foreach (var regionalSource in _appConfiguration.Sources.Regional)
                {
                    // Start...
                    _progressReport.NewLine();
                    _progressReport.Caption($"Import {regionalSource.CountryCode} data");

                    // Get base regional folder
                    var regionalFolder = new DirectoryInfo(Path.Combine(importFolder.FullName, regionalSource.CountryFolderName));

                    // Import subdivisions
                    if (!string.IsNullOrEmpty(regionalSource.SubdivisionsFileName))
                    {
                        var subdivisionFile = new FileInfo(Path.Combine(regionalFolder.FullName, regionalSource.SubdivisionsFileName));

                        if (subdivisionFile.Exists)
                        {
                            await ImportToDatabaseAsync<CsvSubdivision>(dbContext, subdivisionFile, cancellationToken);
                        }
                    }

                    // Import organizational units
                    if (!string.IsNullOrEmpty(regionalSource.OUnitsFileName))
                    {
                        var oUnitFile = new FileInfo(Path.Combine(regionalFolder.FullName, regionalSource.OUnitsFileName));

                        if (oUnitFile.Exists)
                        {
                            await ImportToDatabaseAsync<CsvOUnit>(dbContext, oUnitFile, cancellationToken);
                        }
                    }

                    // Import holidays
                    var holidayFolder = new DirectoryInfo(Path.Combine(regionalFolder.FullName, regionalSource.HolidaysFolderName));

                    if (holidayFolder.Exists)
                    {
                        foreach (var csvFile in holidayFolder.GetFiles("*.csv"))
                        {
                            await ImportToDatabaseAsync<CsvHoliday>(dbContext, csvFile, cancellationToken);
                        }
                    }

                    // Everything is fine!
                    _progressReport.Success("Data successfully imported!");
                }
            }
            catch (Exception ex)
            {
                _progressReport.NewLine();
                _progressReport.Error($"Import failed. {ex.Message}");
                throw;
            }
        }

        private async Task ImportToDatabaseAsync<T>(AppDbContext dbContext, FileInfo csvFile, CancellationToken cancellationToken)
            where T : CsvBase
        {
            uint recordCount = 0;

            try
            {
                _progressReport.Start($"Import file {csvFile.Name}...");

                using var strReader = csvFile.OpenText();

                var csvReader = new CsvTableReader(strReader, new CsvConfiguration() { Separator = ';' });

                csvReader.AddConverter<ICollection<string>>(new StringListConverter());
                csvReader.AddConverter<ICollection<CsvLocalizedText>>(new LocalizedTextListConverter());

                await csvReader.ReadHeadersAsync();

                await foreach (var csvRecord in csvReader.ReadAllAsync<T>())
                {
                    await csvRecord.AddToDatabase(dbContext, cancellationToken);

                    recordCount++;

                    _progressReport.Continue(recordCount++);
                }

                _progressReport.Finish(recordCount);
            }
            catch 
            {
                _progressReport.Cancel();
                throw;
            }
        }
    }
}
