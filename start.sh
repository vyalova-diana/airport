cd ./logs
killall python3
rm 1.txt
rm 2.txt
rm 3.txt
rm 4.txt
rm 5.txt
rm 6.txt
rm 7.txt
rm 8.txt
rm 9.txt
rm 10.txt
rm 11.txt
rm 12.txt
rm 13.txt
rm 14.txt
rm 15.txt
rm 16.txt
killall dotnet
cd ..
sudo chmod 777 ./5/controllerStatus.txt
sudo chmod 777 ./5/vehicleStatus.txt
ruby 4/app.rb 2>&1 | tee logs/3.txt;
python3 3/app.py 2>&1 | tee logs/4.txt &
python3 8/app.py 2>&1 | tee logs/8.txt &
dotnet run --project ./2/BusApi/BusApi.csproj 2>&1 | tee logs/2.txt &
dotnet run --project ./1/FlightPassengerApi/FlightPassengerApi.csproj 2>&1 | tee logs/1.txt &
dotnet run --project ./5/RefuellerBackend/RefuelBackend.csproj 2>&1 | tee logs/5.txt &
dotnet run --project ./5/RefuelService/RefuelService.csproj 2>&1 | tee logs/5.txt &
dotnet run --project ./6/FollowMeService/FollowMeService.csproj 2>&1 | tee logs/6.txt &
dotnet run --project ./13/WebApi/WebApi/WebApi.csproj 2>&1 | tee logs/13.txt1 &
dotnet run --project ./16/TicketWindow/TicketWindow.csproj 2>&1 | tee logs/16.txt &
dotnet run --project ./14/WebApplication/WebApplication.csproj 2>&1 | tee logs/14.txt &
dotnet run --project ./12/CateringService/CateringService.csproj 2>&1 | tee logs/12.txt &
dotnet run --project ./9/WebApplication_CheckIn/WebApplication_CheckIn.csproj 2>&1 | tee logs/9.txt &
dotnet run --project ./15/WebApiAirportNew/WebApiAirportNew.csproj 2>&1 | tee logs/15.txt &
python3 -m http.server 3221
