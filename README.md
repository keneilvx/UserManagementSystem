# UserManagementSystem

## Setup

Simple project for implementing a simple User management system 



The project requires minimal  setup, as it uses an in-memory database with EF. If you need to add additional users for testing, users can do the following:

clone project 

Go to `UserManagementSystem.Infrastructure\Migrations\UserData.cs` to add more users, just copy and paste & edit 

## API overview and authentication

Authentication via JWT Tokens 
Login Endpoint: `POST /api/auth/login`

Register Endpoint: `POST /api/auth/register`

Protected Endpoints: Require `[Authorize]` attribute

Swagger Authorization: Click the lock icon and paste return access token from the login response 

sample user post request

```C#
{
  "email": "john.doe@examplemail.com",
  "password": "test123"
}
```


The login process is handled by the method `LoginAsync(LoginRequestDTO request)`. When a user tries to log in, this method first checks if the email exists in the database:
```
var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
```
If User Exists 
```
!BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
```
If the login is successful, LoginAsync does two key things:
Returns the user access Token 
update the users' `lastLoginAt`

## Webhook

This service fires a webhook every time a user logs in successfully. It gathers all users who have logged in in the last 30 minutes and sends them in a JSON payload to a configured URL. If the webhook fails to send, the error gets logged — retries aren’t implemented, but you’ll see failures in the logs.

trigger - After a user logs in user's LastLoginAt is updated and saved

`http://localhost:5298/api/webhook/user-logged-in` - `POST` - request body can be sent the the URL configured in the code. This can be changed to use an  environment variable instead.

`SendLoginWebhookAsync(users)` - does the heavy lifting it takes in the user object. 

## Logging overview `GenericLogger<T>`

is a Generic Logger that you DI into your existing projects, allowing you to pass any existing class 

It uses Microsoft's `Microsoft.Extensions.Logging` - `ILogger<T>`  and extends it with the following methods:

`LogInformation(string message, params object[] args)`

`LogWarning(string message, params object[] args)`

`LogError(string message, params object[] args)`

`LogException(Exception ex, string message = "")`

## Data contracts

### Entities
Token 
```C#

   public class Token
   {
       public string AccessToken { get; set; } = string.Empty;

   }
```

```C#
 public class User
 {
     public Guid Id { get; set; }
     public string Username { get; set; }
     public string FirstName { get; set; }
     public string LastName { get; set; }
     public string Email { get; set; }
     public string Password { get; set; }
     public DateTime CreatedAt { get; set; }
     public DateTimeOffset LastLoginAt { get; set; }
 }
```

### DTOs 
##### User DTOs 

```C#
 public class CreateUserDTO
 {
     public Guid Id { get; set; }
     public string Username { get; set; }
     public string FirstName { get; set; }
     public string LastName { get; set; }
     public string Email { get; set; }
     public string Password { get; set; }
     public DateTime CreatedAt { get; set; }
     public DateTimeOffset LastLoginAt { get; set; }
 }

```

```C#
  public class ReadUserDTO
  {
      public Guid Id { get; set; }
      public string Username { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Email { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTimeOffset LastLoginAt { get; set; }

  }

```


```C#

 public class UpdateUserDTO
 {
     public Guid Id { get; set; }
     public string Username { get; set; }
     public string FirstName { get; set; }
     public string LastName { get; set; }
     public string Email { get; set; }
     public DateTime CreatedAt { get; set; }
     public DateTimeOffset LastLoginAt { get; set; }
 }

```
```C#
    public class LoginRequestDTO
  {
      public string Email { get; set; }
      public string Password { get; set; }
  }
```

Webhook 
```C#
  public class UserLoggedInEventDTO
  {
      public string Event { get; set; }
      public DateTime Timestamp { get; set; }
      public List<ReadUserDTO>? ActiveUsers { get; set; }

  }
```


