# _Epicodus Band Tracker_

#### _Website to track bands and venues, 3-3-17_

#### By _**Nicholas Mead**_

## Description

_This Website allows users to add venues and bands to specific venues. _

## Technologies Used

_Website uses Html, Materialize css framework, c# with NancyFx_

## SQL Instructions

_In SqlCMD:

_CREATE DATABASE band_tracker _
_GO _
_USE band_tracker _
_GO _
_CREATE TABLE bands (id int identity(1,1), name VARCHAR(255)); _
_CREATE TABLE bands_venues (id INT IDENTITY(1,1), band_id INT, venue_id INT); _
_CREATE TABLE venues (id INT, name VARCHAR(255)); _
_GO _


Copyright (c) 2017 **_Nicholas Mead_**
