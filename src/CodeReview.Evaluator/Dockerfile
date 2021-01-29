#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY ["ReviewItEasy.Evaluator/*.csproj", "ReviewItEasy.Evaluator/"]
RUN dotnet restore "ReviewItEasy.Evaluator/ReviewItEasy.Evaluator.csproj" --force

COPY ReviewItEasy.Evaluator/. ./ReviewItEasy.Evaluator/
RUN dotnet publish ./ReviewItEasy.Evaluator/ReviewItEasy.Evaluator.csproj -c Release -o ./ReviewItEasy.Evaluator/out

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime

RUN apt-get update
RUN apt-get --yes install nuget

VOLUME /app/Dictionaries
WORKDIR /app
COPY --from=build /app/ReviewItEasy.Evaluator/out .
ENTRYPOINT ["dotnet", "ReviewItEasy.Evaluator.dll"]
CMD ["--help"]