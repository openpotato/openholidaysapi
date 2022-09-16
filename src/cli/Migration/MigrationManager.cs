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

using Enbrea.Progress;
using Microsoft.EntityFrameworkCore;
using OpenHolidaysApi.DataLayer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Manager for migrating/creating the database
    /// </summary>
    public class MigrationManager
    {
        private readonly IDbContextFactory<OpenHolidaysApiDbContext> _dbContextFactory;
        private readonly ProgressReport _progressReport;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationManager"/> class.
        /// </summary>
        /// <param name="appConfiguration">Configuration data</param>
        public MigrationManager(AppConfiguration appConfiguration)
        {
            _dbContextFactory = new OpenHolidaysApiDbContextFactory(appConfiguration.Database);
            _progressReport = ProgressReportFactory.CreateProgressReport(ProgressUnit.Count);
        }

        /// <summary>
        /// Executes the migration 
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A task that represents the asynchronous migration operation.</returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            { 
                using var dbContext = _dbContextFactory.CreateDbContext();

                _progressReport.Caption("Migration");

                _progressReport.Start("Delete existing database...");
                await dbContext.Database.EnsureDeletedAsync(cancellationToken);
                _progressReport.Finish(); 

                _progressReport.Start("Creating new database...");
                await dbContext.Database.MigrateAsync(cancellationToken);
                _progressReport.Finish();

                _progressReport.Success("Database newly created!");
                _progressReport.NewLine();
            }
            catch (Exception ex)
            {
                _progressReport.NewLine();
                _progressReport.Error($"Migration failed. {ex.Message}");
                throw;
            }
        }
    }
}
