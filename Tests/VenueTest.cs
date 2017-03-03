using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Tracker.Objects;

namespace Tracker
{
    public class VenueTest : IDisposable
    {
        public VenueTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_Save_CheckVenueSaveToDb()
        {
            Venue tempVenue = new Venue("Hilton");
            List<Venue> allVenues = new List<Venue>{tempVenue};
            tempVenue.Save();

            Assert.Equal(allVenues, Venue.GetAll());
        }



        public void Dispose()
        {
            Band.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
