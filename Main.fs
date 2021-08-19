namespace DataDeft.Lambda

// Internal

// External

open Falco
open Falco.HostBuilder
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection


module Main =

  let configureServices (services: IServiceCollection) =
    services.AddFalco() |> ignore


  let configureApp (endpoints: HttpEndpoint list) (ctx: WebHostBuilderContext) (app: IApplicationBuilder) =

    let devMode = true

    app.UseWhen(devMode, (fun app -> app.UseDeveloperExceptionPage()))
       .UseWhen(not (devMode),
                (fun app ->
                  app.UseFalcoExceptionHandler
                    (Response.withStatusCode 500
                     >> Response.ofPlainText "Server error"))).UseHttpsRedirection().UseFalco(endpoints)
    |> ignore


  type LambdaEntryPoint() =

    inherit Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction()

    override this.Init(builder: IWebHostBuilder) =
      builder
        .ConfigureServices(configureServices)
        .Configure(configureApp Handler.endpointList)
      |> ignore


  let configureWebHost (endpoints: HttpEndpoint list) (webHost: IWebHostBuilder) =

    webHost.UseEnvironment("Development") |> ignore

    webHost
      .ConfigureServices(configureServices)
      .Configure(configureApp endpoints)

  [<EntryPoint>]
  let main args =
    webHost args {
      configure configureWebHost
      endpoints Handler.endpointList
    }
    0
