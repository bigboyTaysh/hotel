# Hotel management application

This web application supports hotel management, including:
* customers management
* room management
* reservations management

Architecture is based on microservices. It contains single api interfaces as a services with separate databases, an intermediary interface and a client application. Each services is secured and the ability to get data is verified using a **JWT tokens (access and refresh)** issued by Identity.Service when logging in. [ClientApp](https://github.com/bigboyTaysh/hotel/tree/master/Hotel/HotelApp/ClientApp) uses the **silent refresh** to refresh the access token with a refresh token after the access token has expired. 

(Docker & Kubernetes cluster on [docker](https://github.com/bigboyTaysh/hotel/tree/docker))

### Services:
* [Identity.Service](https://github.com/bigboyTaysh/hotel/tree/master/Hotel/Identity.Service), which is responsible for users management, logging in and issuing a JWT token.
* [Customers.Service](https://github.com/bigboyTaysh/hotel/tree/master/Hotel/Customers.Service), which is responsible for customers management. 
* [Rooms.Service](https://github.com/bigboyTaysh/hotel/tree/master/Hotel/Rooms.Service), which is responsible for rooms management. 
* [Reservations.Service](https://github.com/bigboyTaysh/hotel/tree/master/Hotel/Reservations.Service), which is responsible for reservations management. 

### Apps:
* [HotelApp](https://github.com/bigboyTaysh/hotel/tree/master/Hotel/HotelApp) is an api interface that mediates between the client application and services.
* [ClientApp](https://github.com/bigboyTaysh/hotel/tree/master/Hotel/HotelApp/ClientApp) is an client spa web app. 

## Technology stack
* _**ASP.NET Core Web API**_ for every services and intermediary api
* _**MongoDB**_ as databases in every services
* _**Angular**_ for client app
* _**JWT**_

## Guide
For use this application is requred to have installed Angular, .NET and MongoDB  
To run the entire application with all services, you can run the `start.bat` file.  
After that: 
* on https://localhost:44331/ is running client application.  
* on http://localhost:8761/ there is eureka server where information about registered services will appear.  
