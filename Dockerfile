FROM sonarqube/dotnetcore
ARG token
COPY . ./
RUN dotnet restore ReactiveFileWatcher.sln
RUN dotnet sonarscanner begin /k:"ReactiveFileWatcher" /d:sonar.host.url="http://sonarqube:9000" /d:sonar.login="$token"
RUN dotnet build
RUN dotnet test ./CoreTests/CoreTests.csproj
RUN dotnet sonarscanner end /d:sonar.login="$token"