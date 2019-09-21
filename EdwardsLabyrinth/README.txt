
This is my implementation of the summer problem 2018.
Albin Bramstång

*******
*Build*
*******

The application requires .Net Core 2.0. It can be downloaded at https://www.microsoft.com/net/download.
To build, run either Build.bat or Build.sh.

*****
*Run*
*****

The project is run by typing:
dotnet run --project EdwardsLabyrinth\EdwardsLabyrinth.csproj

It will read input until an empty string is served.

It's also possible to pass a file like so:
cat file.txt | dotnet run --project EdwardsLabyrinth/EdwardsLabyrinth.csproj

3 example maps are provided, together with Run.bat and Run.sh for easy running. 

Have fun!