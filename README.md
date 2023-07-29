CozynibiHotel - WEB API

This is ASP.NET CORE 6 WEB API using Generic Repository Pattern and Unit of work

# I.Technologies:
    - Language: C#
    - Framework: ASP.NET CORE 6 WEB API, Entity Framework
    - Database: SQL Server

# II.Extension Libraries
    - MailKit, MimeKit
    - QRCoder
    - Auto Mapper
    - Microsoft.AspNetCore.Authentication.JwtBearer
    - Microsoft.AspNetCore.SignalR

# III. System Libraries
    - JWT (System.IdentityModel.Tokens.Jwt)

# IV. Solution details
1. Solution structure
- This have 7 project files: 
    + 1 Main API Project (API)
    + 3 Project Class Libruaries (.Core, .Infrastructure, .Services)
    + 3 Independent Class Libruary (HUG.CRUD, HUG.EmailServices, HUG.QRCodeServices)

- The projects and files are created following Generic Repository Pattern and Unit of work
    + This pattern makes these projects are very usable
    + The code has been clear and can be maintain easily
    + Business logic and code's logic is shared for these components

- StartUp Project: ConizibiHotel.API
- Overview files logical connections:
--> Controller(API URL REQUEST) -> Services(HANDLE INPUTS AND RETURN AN OUTPUT) -> Repository(WORKS WITH DB(CRUD)) -> (DB)

- Details Description:
    + HUG.CRUD has all method and services to handle these entites in the DB, using Generic class and interface that allow every type of Entity can be used and register the DI to use in the whole project, in this using abtract class so whenever you want to change the logic of an method or class you can declare and override that method
    + HUG.EmailServices: has method to send emails to clients
    + HUG.QRCodeServices: has QR services contains generate QR code by an input, ..
    --> These 3 class libraries are independent module means CAN BE USED IN MANY OTHER PROJECT
3. Set up
    - In Program file: set up CORS allow some host to access and using the resources
4. Base Services 
    - CRUD (Create, Read, Update, Delete)
    - Upload File, images, ..
    - Communicating with DB with these relationships between these entities: 
        + One - by - One (1:1)
        + One - to - Many (1: N)
        + Many- to - many (N:N) 

5. What makes this project special ?
    - Security: 
        + Authentication: (ACCESS TOKEN and REFESH TOKEN)
        --> using JWT to create a access token with a secret key
        --> using SHA256 to hash the token
        --> provide refesh token to keep user in using in a long time and access token can be change
        + Authorize: 

    - RealTime:
        + Websocket SignalR: whenever client send a form(request), admin will immediately see the notifications
        + Create a tunnel call "/hub", this the middle connection between server and client 
    
    - Sending Email:
        + MailKit and MimeKit, to create a request to Google server at port 587 or 465 in smtp server, that allow us to
        send email to other with the content and template we want. In this project Checkin_code and QR wwill be sent when
        confirm booking

    - Sending SMS:
        + Twilio provides a virtual number and free version that help us can send sms to other phone number, in this project 
        this service is for confirm with the custommers

    - QR Create and Verify
        + QRCoder, to hash a string to QR
        + to QR code is stronger we will using JWT again to generate the code includes object of information so when a device 
        scan the code will be a token, you have to scan it and having secrete key then the infor will be seen.
        ![qr](https://github.com/hugnt/CozynibiHotel-Web-API/assets/103843426/e9149fca-726a-4014-9a0e-ff4c4ff3c7a1)
        
    - Export Bill 
        + Export to pdf, words, ...

    - Analysis from Dashboard
        + The most eating food
        + The money earning from foods
        + Number of custommer
        + Rating rooms
# V. How to use?
    - This solution is only includes API, you can test them in POST MAN, Or for having a great travel, please see and clone the ADMIN TEMPLATE of this project in: https://github.com/hugnt/CozynibiHotel-Admin-ASP.NET-CORE-MVC-6.0
    (Please read the instruction in that project to clone and using)

    - STEP 1: CLONE THIS PROJECT 
    - STEP 2: (the appsettings.json file is ignore by me) so please create appsettings.json file (in CozinibiHotel.API project) and set up like this:
    {
        "Appsettings": {
            "SecretKey": "<Your secret key for jwt>"
        },
        "ConnectionStrings": {
            "DefaultConnection": "<Your data connecttion>"
        },
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
            }
        },
        "EmailSettings": {
            "Email": "<Your email>",
            "Password": "<Your password of google account take that google provide for u to using (if you dont know how to take it please see the link instrution I took below)>",
            "Host": "smtp.gmail.com",
            "DisplayName": "<Your Display name>",
            "Port": 587 <Or 465 if 587 is fail to connect>
        },
        "AllowedHosts": "*"
    }
    - STEP 3: Setting start up project is CozinibiHotel.API and RUN
    - STEP 4: Open POST MAN for testing or open Admin template and using

# VI. References and link
    - Set up and take the google email password for access ASP.NET can be sent email:
        + enable app password: https://nchuyvn.com/s%E1%BA%A3n%20ph%E1%BA%A9m/cach-tao-app-password-cho-smtp-gmail/
        + using in ASP.NET CORE: https://www.youtube.com/watch?v=0e325O9mzP8

