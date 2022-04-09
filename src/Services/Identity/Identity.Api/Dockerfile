FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY "Zawodowcy.sln" "Zawodowcy.sln"

COPY "src/Services/Identity/Identity.Api/Identity.Api.csproj" "src/Services/Identity/Identity.Api/Identity.Api.csproj"
COPY "src/Services/Identity/Identity.Application/Identity.Application.csproj" "src/Services/Identity/Identity.Application/Identity.Application.csproj"
COPY "src/Services/Identity/Identity.Domain/Identity.Domain.csproj" "src/Services/Identity/Identity.Domain/Identity.Domain.csproj"
COPY "src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj" "src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj"

RUN dotnet restore "Zawodowcy.sln"

COPY . .
WORKDIR /src/Services/Identity/Identity.Api
RUN mkdir /usr/local/nvm
ENV NVM_DIR=/usr/local/nvm
ENV NODE_VERSION=16.13.0
ENV NVM_INSTALL_PATH=${NVM_DIR}/versions/node/v${NODE_VERSION}
RUN rm /bin/sh && ln -s /bin/bash /bin/sh && \
    curl --silent -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash
RUN source $NVM_DIR/nvm.sh \
   && nvm install $NODE_VERSION \
   && nvm alias default $NODE_VERSION \
   && nvm use default
    
ENV NODE_PATH=${NVM_INSTALL_PATH}/lib/node_modules
ENV PATH=${PATH}:${NVM_INSTALL_PATH}/bin
RUN cd client-ui && npm install react-scripts && \
    npm install -g npm && npm install && cd .. &&  \
    dotnet build && dotnet publish --no-restore -c Release -o /app
COPY /src/Services/Identity/Identity.Api/client-ui /app/client-ui
FROM build AS publish

FROM publish AS final
WORKDIR /app
COPY --from=publish /app .
RUN cd client-ui && npm install

ENTRYPOINT ["dotnet", "Identity.Api.dll"]
