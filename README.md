# MynChain
A sample project to demonstrate the implementation of Blockchain's concepts

**Tech Stack**
- .NET Core 2.1
- C#
- MongoDb
- Swagger
- Visual Studio

**What you'll find**
- A WebApi project (**MynChain.Api**) that implement the Business Logic (**MynChain**)
- Datas are written inside a MongoDb instance on your localhost through the **MynChain.Data.MongoDb**, which models are stored inside the **MynChain.Models** class project

**What you'll not find**
- It's a demonstration purpose project, so you'll not find any implementation related to security/repository patterns, no authentication methods and, especially, is not ready for any kind of production; a lot of work, eventually, must be done

**How to use it**
- Open the solution
- Launch the project called **MynChain.Api**
- Once launched:
  - A MongoDb database will be created on your localohost instance 
    - you can change the connection string inside **MynChain.Data.MongoDb/MongoDb.cs/Connect()**
  - The genesis block will be created 
    - so you'll find a new document inside the block's collection of the db
- How to create a new block on the chain
  - call the "**/mine**" endpoint
    - you'll find the new block inside the "blocks" collection on your MondoDb instance
- How to create a new transaction on the block
  - call the "**/transactions/add**" endpoint passing a body request using the model **Models.Transaction**
  - once made, a new collection called "transactions" will be created on your MongoDb instance
    - in this collection you'll find all the transactions that are not yet forged inside a block
  - call the "**/mine**" endpoint to forge the transaction inside a block
    - this is made automatically, so you cannot choose from here the block on which the transaction must be forged in

