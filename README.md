# Jemena Gas Meters Tracking System

## Introduction

This a mobile application prototype which is built keeping the gas meter technicians(GST) of Jemena as the end-users. It aims to digitise the 
entire lifcycle of a gas meter, right from the time it leaves the warehouse with a GST till the time it is installed at a
customers house.It is built using React Native with Redux for the front end and .NET web API as the backend. SQLite is used the 
database.

## Demo video

https://youtu.be/cnWWMJBE5Lc

## Features

- The GST can pickup meters from the warehouse and assign it on his name, simply by entering the gas meter ID or by scanning the 
barcode on it.
- The meter status can be changed to 'Faulty' or 'Expired' from 'Active', in case the technician detects any problems in a particular 
meter.
- While recording a meter status as 'Installed', the technician would be asked to enter the address of the house where it was installed
to solve the issues of cross-metering.
- Meters can be exchanged betwwen GSTs. And while doing so, they would be asked to record the name, company name of the person whom the 
meters were transfered to.

## Tools required for setup/running

- An iphone or an andriod phone with Expo app installed
- Xcode/Andriod Studio (optional)
- Visual Studio Code (optional)

## DB structure and enums in the code

### Tables
- Meters table is a record of all the meters which the company has. MeterStatus column corresponds to the following enum in the 
backend: `INHOUSE: 1, PICKED UP: 2, RETURNED: 3, TRANSFERED: 4, INSTALLED: 5`.
MeterCondition column corresponds to the following enum in the 
backend: `ACTIVE: 1, FAULTY: 2, EXPIRED: 3`
- MeterHistories table records all activities done on the meter. It too has a MeterStatus column and entry of MIRN 100, Payroll ID 640 
and MeterStatus 2 would mean that meter number 100 was picked up by a technician with ID 640.
- Installations table records the address where the meter was installed and its primary key is referenced by the MeterHistories location column as a
foreign key whenever a meter is installed.
- Transfers table records the name and the company name of the GST to which the person was transfered meters. Its PK is refernced by the
MeterHistories's TransfereeId column whenever meters are transfered among GSTs.


## Setup

### Run the application on a phone connecting to API in Azure
- Make sure you have the Expo app installed on your phone.
- Navigate to `https://expo.io/@vineet39/jgm-app` from a web browser on your computer. This is where the front-end is hosted.
- Scan the barcode which appears on the web page. Scan the barcode on the webpage with the camera app if you an iPhone.Use the Expo app to scan the barcode if you have an Andriod phone. In both cases, the Expo app must be installed on your phone.
- This will only work till the backend remains hosted on `https://jgm.azurewebsites.net/`. Once the free trial ends shift to running the app in localhost.

#### Check the data in the database
- Navigate to `https://jgm.azurewebsites.net/swagger/index.html` in your browser.
- Click on the first GET API under each heading to view the data in that table. For example, to view the data in the meter table
click on `GET /api/Meters`, Try it out and then Execute.

### Run the app on localhost

#### Clone
- Clone this repo to your local machine using `https://github.com/rmit-s3734938-vineet-bugtani/JemenaGasMeters.git` or simply 
download the project folder.

#### Running the front end
- Open the folder where the repository was cloned using Visual Studio Code
- Navigate to the client app folder using `cd JemenaGasMeters/JemenaGasMeter.ClientApp` in VSCode's integrated terminal
- Install all dependencies with `npm install`
- Make sure that the url in `JemenaGasMeters/JemenaGasMeter.ClientApp/constants/constants.js` is set to `http://127.0.0.1:19000/` i.e. you would be connecting to the backend on localhost and not on Azure.
- Make sure you have a andriod or ios simulator running in your laptop.
- Start the app with `npm start`
- A page would open in your browser. Click `Run on ios simulator` or `Run on andriod simulator` depending on the simualtor
you have open.

#### Running the back end
- Open the folder where the repository was cloned using Visual Studio Code
- Navigate to the client app folder using `cd JemenaGasMeters/JemenaGasMeter.WebApi` in VSCode's integrated terminal
- Start the backend with `dotnet run`

#### Check the data in the database
- Navigate to `http://127.0.0.1:19000/swagger/index.html` in your browser.
- Click on the first GET API under each heading to view the data in that table. For example, to view the data in the meter table
click on `GET /api/Meters`, Try it out and then Execute.
- You can also use any other tool for accessing data in the DB eg: DB Browser for SQLite.

### Deploy backend on Azure
- Go to azure portal.
- Create a resource. Select `Web App`.
- Project details would be:
  Subscription: May vary
  Resource group: Either new or existing
- Instance Details:
  Name: Any suitable name
  ,Publish: `Code`
  ,Runtime stack: `.NET Core 3.1(LTS)`
  ,OS: `Windows`
  ,Region: `Australia Southeast`
- No tags or monitoring required.
- Now in Visual Studio build the JemenaGasMeter.WebApi folder and right click, select Publish
- Select the resource group and the instance we created earlier

### Deploy frontend on an Expo server
- Navigate to `cd JemenaGasMeters/JemenaGasMeter.ClientApp` 
- Make a expo account and type `expo publish`












