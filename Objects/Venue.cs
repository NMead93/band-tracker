using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Tracker.Objects
{
    public class Venue
    {
        private int _id;
        private string _name;

        public Venue(string name, int id = 0)
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

        public override bool Equals(System.Object otherVenue)
        {
            if (!(otherVenue is Venue))
            {
                return false;
            }
            else
            {
                Venue newVenue = (Venue) otherVenue;
                bool idEquality = this.GetId() == newVenue.GetId();
                bool nameEquality = this.GetName() == newVenue.GetName();

                return (idEquality && nameEquality);
            }
        }

        public List<Band> GetBands()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT bands.* FROM venues JOIN bands_venues ON (venues.id = bands_venues.venue_id) JOIN bands ON (bands_venues.band_id = bands.id) WHERE venues.id = @VenueId;", conn);

            SqlParameter VenueId = new SqlParameter("@VenueId", this.GetId().ToString());

            cmd.Parameters.Add(VenueId);
            SqlDataReader rdr = cmd.ExecuteReader();
            List<Band> bandList = new List<Band>{};

            while(rdr.Read())
            {
                int bandId = rdr.GetInt32(0);
                string bandName = rdr.GetString(1);

                Band tempBand = new Band(bandName, bandId);
                bandList.Add(tempBand);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return bandList;
        }


        public static List<Venue> GetAll()
        {
            List<Venue> VenueList = new List<Venue> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Venue newVenue = new Venue(name, id);
                VenueList.Add(newVenue);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return VenueList;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

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

        public static Venue Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @VenueId;", conn);
            SqlParameter venueIdParameter = new SqlParameter();
            venueIdParameter.ParameterName = "@VenueId";
            venueIdParameter.Value = id.ToString();
            cmd.Parameters.Add(venueIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundVenueId = 0;
            string foundVenueName = null;

            while (rdr.Read())
            {
                foundVenueId = rdr.GetInt32(0);
                foundVenueName = rdr.GetString(1);
            }
            Venue foundVenue = new Venue(foundVenueName, foundVenueId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundVenue;
        }

        public void AddBand(Band newBand)
       {
           SqlConnection conn = DB.Connection();
           conn.Open();

           SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (band_id, venue_id) VALUES (@BandId, @VenueId);", conn);

           SqlParameter bandIdParameter = new SqlParameter("@BandId", newBand.GetId());
           SqlParameter venueIdParameter = new SqlParameter("@VenueId", this.GetId());

           cmd.Parameters.Add(bandIdParameter);
           cmd.Parameters.Add(venueIdParameter);

           cmd.ExecuteNonQuery();

           if (conn != null)
           {
               conn.Close();
           }
       }

       public void Update(string newName)
       {
         SqlConnection conn = DB.Connection();
         conn.Open();

         SqlCommand cmd = new SqlCommand("UPDATE venues SET name = @NewName OUTPUT INSERTED.name WHERE id = @VenueId;", conn);

         SqlParameter newNameParameter = new SqlParameter();
         newNameParameter.ParameterName = "@NewName";
         newNameParameter.Value = newName;
         cmd.Parameters.Add(newNameParameter);


         SqlParameter venueIdParameter = new SqlParameter();
         venueIdParameter.ParameterName = "@VenueId";
         venueIdParameter.Value = this.GetId();
         cmd.Parameters.Add(venueIdParameter);
         SqlDataReader rdr = cmd.ExecuteReader();

         while(rdr.Read())
         {
           this._name = rdr.GetString(0);
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

       public void DeleteBandFromVenue(Band band)
       {
           SqlConnection conn = DB.Connection();
           conn.Open();

           SqlCommand cmd = new SqlCommand("DELETE FROM bands_venues WHERE band_id = @BandId AND venue_id = @VenueId;", conn);

           SqlParameter bandParameter = new SqlParameter("@BandId", band.GetId());
           SqlParameter venueParameter = new SqlParameter("@VenueId", this.GetId());
           cmd.Parameters.Add(bandParameter);
           cmd.Parameters.Add(venueParameter);

           cmd.ExecuteNonQuery();

           if(conn != null)
           {
               conn.Close();
           }
       }


       public void DeleteSingle()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM venues WHERE id = @VenueId; DELETE FROM bands_venues WHERE venue_id = @VenueId;", conn);

            SqlParameter nameParameter = new SqlParameter("@VenueId", this.GetId());
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
            SqlCommand cmd = new SqlCommand("DELETE FROM venues; DELETE FROM bands_venues", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }


    }
}
