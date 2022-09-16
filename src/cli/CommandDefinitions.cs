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

using System.CommandLine;

namespace OpenHolidaysApi.CLI
{
    public static class CommandDefinitions
    {
        public static Command InitDb(AppConfiguration appConfiguration)
        {
            var command = new Command("initdb", "Creates a new OpenHolidaysAPI database")
            {
                new Option<bool>(new[] { "--populate", "-p" }, "Populate database with data")
                {
                    IsRequired = false
                }
            };

            command.SetHandler(async (bool populate)
                => await CommandHandlers.InitDb(appConfiguration, populate),
                    command.Options[0] as Option<bool>);

            return command;
        }

        public static Command PopulateDb(AppConfiguration appConfiguration)
        {
            var command = new Command("populatedb", "Populates an OpenHolidaysAPI database");

            command.SetHandler(async ()
                => await CommandHandlers.PopulateDb(appConfiguration));

            return command;
        }
    }
}
