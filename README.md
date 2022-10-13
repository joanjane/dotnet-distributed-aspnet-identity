# dotnet-distributed-aspnet-identity

Sample AspNet Identity solution that distributes user authentication in an API and provides a
solution to verify credentials in SPAs using with a Cookie based authentication using BFF pattern.

**NOTE:** This project covers a password authentication mechanism for simple scenarios. There are
more robust alternatives using standard protocols like OpenID Connect, this solution is only for 
demonstration purposes. Also note that API should not be exposed without adding an authentication 
mechanism between BFF and API or communicating within a private network.

## EF Migrations
### Restore dotnet tool (from repo root):
`dotnet tool restore`

### Add a new migration (from repo root):

```sh
dotnet ef migrations add {Migration Name} -p .\src\PoC.DistributedAspNetIdentity.Api\PoC.DistributedAspNetIdentity.Api.csproj -s .\src\PoC.DistributedAspNetIdentity.Api\PoC.DistributedAspNetIdentity.Api.csproj -c ApplicationDbContext -o Data/Migrations
```