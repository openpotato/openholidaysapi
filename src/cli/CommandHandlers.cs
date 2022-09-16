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

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OpenHolidaysApi.CLI
{
    public static class CommandHandlers
    {
        public static async Task InitDb(AppConfiguration appConfiguration, bool populate)
        {
            await Execute(async (cancellationToken) =>
            {
                var migrationManager = new MigrationManager(appConfiguration);
                await migrationManager.ExecuteAsync(cancellationToken);

                if (populate)
                {
                    var importManager = new ImportManager(appConfiguration);
                    await importManager.ExecuteAsync(cancellationToken);
                }
            });
        }

        public static async Task PopulateDb(AppConfiguration appConfiguration)
        {
            await Execute(async (cancellationToken) =>
            {
                var importManager = new ImportManager(appConfiguration);
                await importManager.ExecuteAsync(cancellationToken);
            });
        }

        private static async Task Execute(Func<CancellationToken, Task> action)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancellationTokenSource.Cancel();
            };

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                await action(cancellationTokenSource.Token);
            }
            catch 
            {
                Environment.ExitCode = 1;
            }

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}.");
            Console.WriteLine();
        }
    }
}