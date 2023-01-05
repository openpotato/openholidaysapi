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

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Extension methods for <see cref="ControllerBase"/>
    /// </summary>
    public static class BaseControllerExtensions
    {
        /// <summary>
        /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response.
        /// </summary>
        /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
        public static ObjectResult ExceptionProblem(this ControllerBase controller)
        {
            var context = controller.HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context?.Error != null)
            {
                if (context.Error is Exception exception)
                {
                    return controller.Problem(statusCode: 500, detail: exception.Message);
                }
            }

            return controller.Problem(statusCode: 500);
        }
    }
}
