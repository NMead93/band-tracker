using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Tracker.Objects
{
    public class Band
    {
        private int _id;
        private string _name;

        public Band(string name, int id = 0)
        {
            _id = id;
            _name = name;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public override bool Equals(System.Object otherBand)
        {
            if (!(otherBand is Band))
            {
                return false;
            }
            else
            {
                Band newBand = (Band) otherBand;
                bool idEquality = this.GetId() == newBand.GetId();
                bool nameEquality = this.GetName() == newBand.GetName();

                return (idEquality && nameEquality);
            }
        }

        public List<Venue> GetVenues()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.band_id) JOIN venues ON (bands_venues.venue_id = venues.id) WHERE bands.id = @BandId;", conn);

            SqlParameter BandId = new SqlParameter("@BandId", this.GetId().ToString());

            cmd.Parameters.Add(BandId);
            SqlDataReader rdr = cmd.ExecuteReader();
            List<Venue> venueList = new List<Venue>{};

            while(rdr.Read())
            {
                int venueId = rdr.GetInt32(0);
                string venueName = rdr.GetString(1);

                Venue tempVenue = new Venue(venueName, venueId);
                venueList.Add(tempVenue);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return venueList;
        }

        public static List<Band> GetAll()
        {
            List<Band> BandList = new List<Band> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Band newBand = new Band(name, id);
                BandList.Add(newBand);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return BandList;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

            SqlParameter nameParameter = new SqlParameter("@Name", this.GetName());

            cmd.Parameters.Add(nameParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static Band Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", conn);
            SqlParameter bandIdParameter = new SqlParameter();
            bandIdParameter.ParameterName = "@BandId";
            bandIdParameter.Value = id.ToString();
            cmd.Parameters.Add(bandIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundBandId = 0;
            string foundBandName = null;

            while (rdr.Read())
            {
                foundBandId = rdr.GetInt32(0);
                foundBandName = rdr.GetString(1);
            }
            Band foundBand = new Band(foundBandName, foundBandId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundBand;
        }

        public static Band FindByName(string name)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE name = @BandName;", conn);
            SqlParameter bandNameParameter = new SqlParameter();
            bandNameParameter.ParameterName = "@BandName";
            bandNameParameter.Value = name;
            cmd.Parameters.Add(bandNameParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundBandId = 0;
            string foundBandName = null;

            while (rdr.Read())
            {
                foundBandId = rdr.GetInt32(0);
                foundBandName = rdr.GetString(1);
            }
            Band foundBand = new Band(foundBandName, foundBandId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundBand;
        }

        public static bool PlayingInVenue(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands_venues WHERE band_id = @BandId;", conn);

            SqlParameter idParameter = new SqlParameter("@BandId", id);
            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();
            bool exists = false;

            while (rdr.Read())
            {
                exists = true;
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return exists;
        }

        public static bool CheckExistence(string name)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE name = @BandName;", conn);

            SqlParameter nameParameter = new SqlParameter("@BandName", name);
            cmd.Parameters.Add(nameParameter);

            SqlDataReader rdr = cmd.ExecuteReader();
            bool exists = false;

            while (rdr.Read())
            {
                exists = true;
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return exists;
        }

        public void DeleteSingle()
         {
             SqlConnection conn = DB.Connection();
             conn.Open();

             SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @BandId; DELETE FROM bands_venues WHERE band_id = @BandId;", conn);

             SqlParameter nameParameter = new SqlParameter("@BandId", this.GetId());
             cmd.Parameters.Add(nameParameter);

             cmd.ExecuteNonQuery();

             if(conn != null)
             {
                 conn.Close();
             }
         }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM bands;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }


    }
}
