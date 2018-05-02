using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Standard.DAL.Repository
{
    public class DbInitializer
    {
		public static void Seed(HueLoggingContext context)
		{
			context.Database.Migrate();

		}
    }
}
