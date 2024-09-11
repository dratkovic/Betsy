# Betsy

For the purpose of defining my seniority level this is example of betting application implementation.

> It is requested to implement small betting application for sports gambling. 
> It must be implemented using .Net 8 and Vue.js

## Domain
- **User**
	- **User** have a **Wallet** in which **Balance** cannot go below 0
	- **User** can put more money in his/her **Wallet**

- **Match**
	- **Match** is scheduled on **StartDate** 
	- **Match** contains one or two names. For football it will be **NameOne** vs **NameTwo**. For individual sports like Athletics it will have only **NameOne**.

- **Offer**
	- Betting **Offer** can be creating for some **Match**.  
	- **Offer** contains multiple **BettingType**s that have **Name** and **Quota**. For football match example would be  1 | 2 | x | x2 | x1 i 12 and each name would have some quota.
	- There could be also a **Special** Offer that has some better quotas created for the Match

- **Ticket**
	- **User** can bet on some **Ticket** by choosing multiple **BettingTypes** from **Offers**
	- **Ticket** contains **TicketAmount** , **Vig** which is manipulation expenses (5%), **Stake** which is (TicketAmount - Vig) and **TotalQuota** which is calculated by multiplying all quotas from **SelectedBettingTypes** that Tickect contains.
	- **Ticket** can contain only one **Special** Offer
	- **Ticket** cannot contain special offer and normal offer for the same match
	- **Ticket** that contains **BettingType** for Special Offer must contain 5 more **BettingTypes** that have **Quota** >= 1.1


## Use Cases
- Authentication
	- ***Register*** email / password + metadata creates user
	- ***Login***  email / password

- Offers
	- **Get All** - get all offers that are not special and that match start is at least 5 min in the future
	- **Get All Special** - get all special offers that match start is at least 5 min in the future

- Tickets
	- **Create** - pay and create a ticket by selected types from offers
	- **GetAll** - Get all played tickets
	- **Get** - Get ticket by Id

	
- Pagination
	- All Get request should be paginated. Default page size is 10. Query contains page, pageSize and filter

## Running
- Make sure that local docker instance is running
- Make sure that **Betsy.AppHost** is set as a startup project 
- Initial user is seeded with (u: **jerry@betsy.hr** p: **P@ssword12** ) 
- In solution files you can find postman collection (*Betsy.postman_collection.json*) with all requests

## Architecture
Even though my personal choice for code architecture would be Vertical Slice, I decided to implement api as Clean Architecture. Assumptions are that your company is producing enterprise solutions and that Clean Architecture is still more represented in enterprise solutions. From my point it is a better suited for the bigger projects. 

Code is written using .NET 8 and C# 12. For unit tests I wrote a few to demonstrate knowledge of writing it. Integration tests are written for the whole api. SQL Server is used for the database in combination with the EF Core as ORM. Flow control is done by using ErrorOr framework to avoid using Exceptions as it, having performance in mind. Cache is implemented by using Output Cache in combination with Redis as a distributed cache. This makes scaling easy for reads in case of peaks. Telemetry is implemented using OpenTelemetry. Prometheus is used as collector and Grafana for visualization and metrics.

  Orchestration is done by the cool Aspire .NET technology.