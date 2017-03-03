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

        Delete["/"] =_=> {
            Band.DeleteAll();
            Venue.DeleteAll();
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

        Delete["/venues"] =_=> {
            Venue foundVenue = Venue.Find(Request.Form["venue-id"]);
            List<Band> bandsOfVenue = foundVenue.GetBands();
            foundVenue.DeleteSingle();
            foreach(var band in bandsOfVenue)
            {
                if (!Band.PlayingInVenue(band.GetId()))
                {
                    band.DeleteSingle();
                }
            }
            return View["venue-management", Venue.GetAll()];
        };

        Get["/venues/{id}"] =parameter=> {
            Venue foundVenue = Venue.Find(parameter.id);
            return View["single-venue.cshtml", foundVenue];
        };

        Post["/venues/{id}"] =parameter=> {
            Venue foundVenue = Venue.Find(parameter.id);
            Band newBand = new Band(Request.Form["band-name"]);
            if(!Band.CheckExistence(newBand.GetName()))
            {
                newBand.Save();
                foundVenue.AddBand(newBand);
            }
            else
            {
                Band foundBand = Band.FindByName(Request.Form["band-name"]);
                foundVenue.AddBand(foundBand);
            }
            return View["single-venue.cshtml", foundVenue];
        };

        Delete["/venues/{id}"] =parameter=> {
            Venue foundVenue = Venue.Find(parameter.id);
            Band foundBand = Band.Find(Request.Form["band-id"]);
            foundVenue.DeleteBandFromVenue(foundBand);
            if (!Band.PlayingInVenue(foundBand.GetId()))
            {
                foundBand.DeleteSingle();
            }
            return View["single-venue.cshtml", foundVenue];
        };

        Patch["/venues/{id}"] =parameter=> {
            Venue foundVenue = Venue.Find(parameter.id);
            foundVenue.Update(Request.Form["venue-name"]);
            return View["single-venue.cshtml", foundVenue];
        };

        Get["/bands"] =_=> {
            return View["band-management.cshtml", Band.GetAll()];
        };


    }
  }
}
