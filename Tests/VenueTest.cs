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
            tempVenue.DeleteSingle();
            List<Venue> testList = new List<Venue>{tempVenue2};
            Assert.Equal(testList, Venue.GetAll());
        }

        [Fact]
        public void Test_Update_UpdatesVenueInDatabase()
        {
          //Arrange
          string name = "Mag";
          Venue testVenue = new Venue(name);
          testVenue.Save();
          string newName = "meg";

          //Act
          testVenue.Update(newName);

          string result = testVenue.GetName();

          //Assert
          Assert.Equal(newName, result);
        }

        [Fact]
        public void Test_GetBands_GetBandsOfVenue()
        {
            Band hardy = new Band("hardy");
            hardy.Save();
            Band testBand = new Band("testBand");
            testBand.Save();
            Venue testVenue = new Venue("testVenue");
            testVenue.Save();
            testVenue.AddBand(hardy);
            testVenue.AddBand(testBand);
            List<Band> expected = new List<Band>{hardy, testBand};
            List<Band> actual =  testVenue.GetBands();
            Assert.Equal(expected, actual);
        }

        public void Dispose()
        {
            Band.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
