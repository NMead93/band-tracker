using System.Collections.Generic;
using System;
using Nancy;
using Nancy.ViewEngines.Razor;
using Tracker.Objects;

namespace Tracker
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
        Get["/"] =_=> {
            return View["index.cshtml"];
        };

        Get["/venues"] =_=> {
            return View["venue-management.cshtml", Venue.GetAll()];
        };

        Post["/venues"] =_=> {
            Venue newVenue = new Venue(Request.Form["venue-name"]);
            newVenue.Save();
            return View["venue-management", Venue.GetAll()];
        };

        Get["/venues/{id}"] =parameter=> {
            Venue foundVenue = Venue.Find(parameter.id);
            return View["single-venue.cshtml", foundVenue];
        };

        Post["/venues/{id}"] =parameter=> {
            Venue foundVenue = Venue.Find(parameter.id);
            Band newBand = new Band(Request.Form["band-name"]);
            newBand.Save();
            foundVenue.AddBand(newBand);
            return View["single-venue.cshtml", foundVenue];
        };

        Delete["/venues/{id}"] =parameter=> {
            Venue foundVenue = Venue.Find(parameter.id);
            Band foundBand = Band.Find(Request.Form["band-id"]);
            foundVenue.DeleteBandFromVenue(foundBand);
            return View["single-venue.cshtml", foundVenue];
        };

        Get["/bands"] =_=> {
            return View["band-management-cshtml", Band.GetAll()];
        };


    }
  }
}
