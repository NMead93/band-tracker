using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Tracker.Objects;

namespace Tracker
{
    public class BandTest : IDisposable
    {
        public BandTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_Save_CheckBandSaveToDb()
        {
            Band tempBand = new Band("rock on");
            List<Band> allBands = new List<Band>{tempBand};
            tempBand.Save();

            Assert.Equal(allBands, Band.GetAll());
        }

        [Fact]
       public void TEST_FindReturnsBand()
       {
           Band tempBand = new Band("Rock Hard");
           tempBand.Save();

           Assert.Equal(tempBand, Band.Find(tempBand.GetId()));
       }

        public void Dispose()
        {
            Band.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
