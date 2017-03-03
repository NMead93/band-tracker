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

        [Fact]
       public void TEST_FindReturnsVenue()
       {
           Venue tempVenue = new Venue("Grand Palace");
           tempVenue.Save();

           Assert.Equal(tempVenue, Venue.Find(tempVenue.GetId()));
       }

       [Fact]
        public void TEST_DeleteSingle_DeleteSingleVenueFromExistence()
        {
            Venue tempVenue = new Venue("Hilton");
            tempVenue.Save();
            Venue tempVenue2 = new Venue("White House");
            tempVenue2.Save();
            Band hefner = new Band("hefner");
            hefner.Save();
            tempVenue.AddBand(hefner);
            tempVenue.DeleteSingle();
            List<Venue> testList = new List<Venue>{tempVenue2};
            Assert.Equal(testList, Venue.GetAll());
        }

        public void Dispose()
        {
            Band.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
