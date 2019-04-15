ruby 3/app.rb 2>&1 | tee logs/3.txt;
./8/main 2>&1 | tee logs/8.txt;
./4/main 2>&1 | tee logs/4.txt;
dotnet run ./1/FlightPassengerApi/FlightPassengerApi.csproj 2>&1 | tee logs/1.txt;
