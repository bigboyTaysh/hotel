start /d "./eureka" java -jar ./target/service-registration-and-discovery-service-0.0.1-SNAPSHOT.jar
start /d "./Hotel/Identity.Service" dotnet run
start /d "./Hotel/Rooms.Service" dotnet run
start /d "./Hotel/Customers.Service" dotnet run
start /d "./Hotel/Reservations.Service" dotnet run
start /d "./Hotel/HotelApp" dotnet run
start /d chrome http://localhost:8761
start /d chrome https://localhost:44331/
