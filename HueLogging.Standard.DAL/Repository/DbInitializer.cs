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

			// todo: seed the api key if the information is availalble from the root.
			// might not actually need this.
			// need to look at sql server in container solution
		

		}
    }
}
