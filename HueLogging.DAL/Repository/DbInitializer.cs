using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.DAL.Repository
{
    public class DbInitializer
    {
		public static void Seed(HueLoggingContext context)
		{
			context.Database.Migrate();

		}
    }
}
