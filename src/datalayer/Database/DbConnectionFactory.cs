#region OpenHolidays API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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

using Npgsql;
using System.Data.Common;

namespace OpenHolidaysApi.DataLayer
{
    public static class DbConnectionFactory
    {
        public static DbConnection CreateNpgsqlConnection(IDbConfiguration configuration)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder() {
                Host = configuration.Server,
                Port = configuration.Port != null ? (int)configuration.Port : NpgsqlConnection.DefaultPort,
                Database = configuration.Database,
                Username = configuration.UserName,
                Password = configuration.Password
            };
                 
            return new NpgsqlConnection(connectionStringBuilder.ConnectionString);
        }
    }
}
