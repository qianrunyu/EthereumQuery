This solution contains 2 projects: EthereumQuery (.net core 3.1 on function app) and EthereumQuery.test (unit tests)

HOW TO RUN:

1, Clone solution to local drive

2, Make EthereumQuery  as the start up project and port 7777 is unused.

3, Build the solution. 

4, click F5 to run it.The function app will listen incoming http request on  http://localhost:7777/api/blocknum/{blockNum}/address/{addr}

Note: If there is a package restore error message after clicking F5, try run "dotnet restore" in CLI.
