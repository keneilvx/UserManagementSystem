# UserManagementSystem

## Setup

Project requires no setup, as it uses an in-memory database with EF. If you need to add additional users for testing, users can do the following:

Go to `UserManagementSystem.Infrastructure\Migrations\UserData.cs` to add more users, just copy and paste & edit 

## API overview and authentication

The login process is handled by the method `LoginAsync(LoginRequestDTO request)`. When a user tries to log in, this method first checks if the email exists in the database:
```
var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
```
If User Exists 
```
!BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
```
If the login is successful, LoginAsync does two key things:
Returns the user
update the users' `lastLoginAt`

## Webhook

This service fires a webhook every time a user logs in successfully. It gathers all users who have logged in in the last 30 minutes and sends them in a JSON payload to a configured URL. If the webhook fails to send, the error gets logged — retries aren’t implemented, but you’ll see failures in the logs.

trigger - After a user logs in user LastLoginAt is updated and saved

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

`POST` - `api/auth/register`
```
{
  "username": "string",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "password": "string",
  "createdAt": "2025-10-30T22:51:38.546Z",
  "lastLoginAt": "2025-10-30T22:51:38.546Z"
}
```

`POST`- `api/auth/login`

```
{
  "email": "string",
  "password": "string"
}
```


`POST` - `api/webhook/user-logged-in`


```
{
  "event": "string",
  "timestamp": "2025-10-30T22:48:02.521Z",
  "activeUsers": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "username": "string",
      "firstName": "string",
      "lastName": "string",
      "email": "string",
      "createdAt": "2025-10-30T22:48:02.521Z",
      "lastLoginAt": "2025-10-30T22:48:02.521Z"
    }
  ]
}
```

`GET` - `users`

