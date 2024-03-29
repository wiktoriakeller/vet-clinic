# Vet Clinic
Application for veterinary clinic management.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Entity relationship diagram](#entity-relationship-diagram)
* [Screenshots](#screenshots)

## General info
This project allows you to manage a veterinary clinic. It implements entity diagram shown [below](#entity-relationship-diagram). This application allows a user to manage information about:
* employees and their positions (e.g. doctor, secretary, technician)
* facilities and offices
* patients and their species
* owners and organizations
* appointments
* drugs
* offered services.

With this application you can schedule an appointment for a given patient - a free doctor will be automatically assigned (as well as a free office in selected facility), but it is possible to later change the assigned vet to another one on the appointment update page. Moreover <strong>Vet Clinic</strong> lets you calculate income for every facility for a given year and provides easy to access search bars on every relevant page as well as an access to a given patient history of a treatment.

## Technologies
* ASP.NET Core in .NET 5
* Dapper
* JavaScript
* HTML5
* CSS3
* Bootstrap v4.6
* Oracle Database

## Entity relationship diagram
<img src="https://github.com/wiktoriakeller/vet-clinic/blob/main/Images/databaseSchema.png" width="700"/>

## Screenshots
<img src="https://github.com/wiktoriakeller/vet-clinic/blob/main/Images/example.gif" width="700"/>
<img src="https://github.com/wiktoriakeller/vet-clinic/blob/main/Images/home.jpg" width="700"/>
<img src="https://github.com/wiktoriakeller/vet-clinic/blob/main/Images/employees.jpg" width="700"/>
<img src="https://github.com/wiktoriakeller/vet-clinic/blob/main/Images/patient.jpg" width="700"/>
<img src="https://github.com/wiktoriakeller/vet-clinic/blob/main/Images/list-of-services.jpg" width="700"/>

