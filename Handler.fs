namespace DataDeft.Lambda

// Internal

// External

open Falco
open Falco.Routing


module Handler =

  let notImplementedHashMap =
    {| NotImplemented = "NotImplemented" |}


  let notImplemented: HttpHandler =

    notImplementedHashMap
    |> Response.ofJson

    // > GET /echo HTTP/1.1
    // > Host: localhost:5001
    // > User-Agent: curl/7.54.0
    // > Accept: */*
    // >
    // < HTTP/1.1 200 OK
    // < Date: Thu, 19 Aug 2021 12:49:22 GMT
    // < Content-Type: application/json; charset=utf-8
    // < Server: Kestrel
    // < Content-Length: 35
    // <
    // * Connection #0 to host localhost left intact
    // {"NotImplemented":"NotImplemented"}⏎


  let endpointList =

    [
      // Utility services
      get     "/echo"   notImplemented

    ]

