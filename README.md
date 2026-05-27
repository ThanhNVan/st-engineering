1st step: <br>
  &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;go to appsettings.json and change the connection string into yours.

2nd step: <br> 
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :bulb:run: `dotnet ef database update --project Napoleon.Fos.Infrastructure --startup-project Napoleon.Fos.Presentation.WebApi`

3rd step:  <br>
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a. run the solution <br>
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;b. you will see 'SeedData' api, run it. <br>
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;c. you will see a list of email, you can use one of them to login, with the default password is '12345678' <br> <br>
  
This Project uses Clean Architecture, together with CQRS to separate the concern between reading and modifying data.<br>

:memo: **Note:** Simple workflow <br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; HttpRequest &rarr; MiddleWare &rarr; Controller &rarr; Command/Query Handler &rarr; Infrastructure (Repos) &rarr; Database