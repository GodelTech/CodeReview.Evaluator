#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /app
COPY ["CodeReview.Evaluator/*.csproj", "CodeReview.Evaluator/"]
RUN dotnet restore "CodeReview.Evaluator/CodeReview.Evaluator.csproj" --force

COPY CodeReview.Evaluator/. ./CodeReview.Evaluator/
RUN dotnet publish ./CodeReview.Evaluator/CodeReview.Evaluator.csproj -c Release -o ./CodeReview.Evaluator/out

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime

WORKDIR /app
COPY --from=build /app/CodeReview.Evaluator/out .
ENTRYPOINT ["dotnet", "CodeReview.Evaluator.dll"]
CMD ["--help"]