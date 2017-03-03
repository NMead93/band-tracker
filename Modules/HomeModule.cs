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

        Get["/bands"] =_=> {
            return View["band-management-cshtml", Band.GetAll()];
        };
    }
  }
}
