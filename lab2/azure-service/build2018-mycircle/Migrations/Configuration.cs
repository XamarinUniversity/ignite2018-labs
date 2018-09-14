namespace build2018_mycircle.Migrations
{
    using build2018_mycircle.DataObjects;
    using Microsoft.Azure.Mobile.Server.Tables;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<build2018_mycircle.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(build2018_mycircle.Models.MobileServiceContext context)
        {
            //  This method will be called after migrating to the latest version.
/*
            List<CircleMessage> items = new List<CircleMessage>
            {
                new CircleMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    IsRoot = true,
                    Author = "System",
                    Text = "Welcome to My Circle!",
		    Color = "Orange",
                    ThreadId = Guid.NewGuid().ToString()
                }
            };

            // Add/Update items
            items.ForEach(i => context.CircleMessages.AddOrUpdate(i));
*/
            base.Seed(context);
        }
    }
}
